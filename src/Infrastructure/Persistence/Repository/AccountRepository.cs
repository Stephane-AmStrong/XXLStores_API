using Application.DataTransfertObjects.Account;
using Application.Enums;
using Application.Exceptions;
using Application.Interfaces;
using Application.Models;
using Domain.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using Persistence.Contexts;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Persistence.Repository
{
    public class AccountRepository : TokenRepository, IAccountRepository
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly JWTSettings _jwtSettings;


        public AccountRepository
        (
            IdentityContext identityContext,
            UserManager<AppUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IOptions<JWTSettings> jwtSettings
        ) : base(identityContext, userManager, roleManager, jwtSettings)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _jwtSettings = jwtSettings.Value;
        }



        public async Task<AuthenticationModel> AuthenticateAsync(LoginModel loginModel, string ipAddress)
        {
            var appUser = await _userManager.FindByNameAsync(loginModel.Email);
            if (appUser == null) throw new ApiException($"No Accounts Registered with {loginModel.Email}.");

            var authenticationSucceeded = await _userManager.CheckPasswordAsync(appUser, loginModel.Password);
            if (!authenticationSucceeded) throw new ApiException($"Invalid Credentials for '{loginModel.Email}'.");

            var isEmailConfirmed = await _userManager.IsEmailConfirmedAsync(appUser);
            if (!isEmailConfirmed) throw new ApiException($"Account Not Confirmed for '{loginModel.Email}'.");

            var jwtSecurityToken = await GenerateJWToken(appUser);
            var userToken = await GenerateRefreshTokenAsync(ipAddress, appUser.Id);

            return new AuthenticationModel
            {
                UserInfo = new Dictionary<string, string>
                {
                    { "Name", appUser.FirstName +" "+appUser.LastName},
                    { "Email", appUser.Email },
                    { "PhoneNumber", appUser.PhoneNumber },
                },

                AccessToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                ExpireDate = jwtSecurityToken.ValidTo,
                RefreshToken = userToken,
                IsSuccess = true
            };
        }



        public async Task<AuthenticationModel> RegisterAsync(AppUser appUser, Roles role, string password, string origin)
        {
            appUser.UserName = appUser.Email;

            var appUserWithSameUserName = await _userManager.FindByNameAsync(appUser.UserName);
            if (appUserWithSameUserName != null) throw new ApiException($"Username '{appUser.UserName}' is already taken.");

            var userWithSameEmail = await _userManager.FindByEmailAsync(appUser.Email);
            if (userWithSameEmail != null) throw new ApiException($"Email {appUser.Email } is already registered.");

            var result = await _userManager.CreateAsync(appUser, password);
            var errorMessage = new StringBuilder();

            foreach (var error in result.Errors)
            {
                errorMessage.Append(error.Description);
            }

            if (!result.Succeeded) throw new ApiException($"{errorMessage}");
            await _userManager.AddToRoleAsync(appUser, role.ToString());

            //await _userManager.AddToRoleAsync(user, Roles.Vendor.ToString());
            var emailConfirmationToken = await GenerateEmailConfirmationTokenAsync(appUser, origin);
            //TODO: Attach Email Service here and configure it via appsettings
            //await _emailService.SendAsync(new Message { From = "mail@codewithmukesh.com", To = user.Email, Content = $"Please confirm your account by visiting this URL {emailVerificationUri}", Subject = "Confirm Registration" });

            return new AuthenticationModel
            {
                UserInfo = new Dictionary<string, string>
                {
                    { "Name", appUser.FirstName +" "+appUser.LastName},
                    { "Email", appUser.Email },
                    { "PhoneNumber", appUser.PhoneNumber },
                },
                AccessToken = emailConfirmationToken,
                IsSuccess = true
            };
        }



        public async Task<AuthenticationModel> RegisterAsync(AppUser appUser, string roleName, string password, string origin)
        {
            var appUserWithSameUserName = await _userManager.FindByNameAsync(appUser.UserName);
            if (appUserWithSameUserName != null) throw new ApiException($"Username '{appUser.UserName}' is already taken.");

            //var userWithSameEmail = await _userManager.FindByEmailAsync(appUser.Email);
            //if (userWithSameEmail != null) throw new ApiException($"Email {appUser.Email } is already registered.");

            var result = await _userManager.CreateAsync(appUser, password);
            var errorMessage = new StringBuilder();

            foreach (var error in result.Errors)
            {
                errorMessage.Append(error.Description);
            }

            if (!result.Succeeded) throw new ApiException($"{errorMessage}");
            await _userManager.AddToRoleAsync(appUser, roleName);

            //await _userManager.AddToRoleAsync(user, Roles.Vendor.ToString());
            //var emailConfirmationToken = await GenerateEmailConfirmationTokenAsync(appUser, origin);
            //TODO: Attach Email Service here and configure it via appsettings
            //await _emailService.SendAsync(new Message { From = "mail@codewithmukesh.com", To = user.Email, Content = $"Please confirm your account by visiting this URL {emailVerificationUri}", Subject = "Confirm Registration" });

            return new AuthenticationModel
            {
                UserInfo = new Dictionary<string, string>
                {
                    { "Name", appUser.FirstName +" "+appUser.LastName},
                    { "Email", appUser.UserName },
                    { "PhoneNumber", appUser.PhoneNumber },
                },
                AccessToken = "done",
                IsSuccess = true
            };
        }



        private async Task<string> GenerateEmailConfirmationTokenAsync(AppUser appUser, string origin)
        {
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(appUser);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var route = "api/account/confirm-email/";
            var _enpointUri = new Uri(string.Concat($"{origin}/", route));
            var emailVerificationUri = QueryHelpers.AddQueryString(_enpointUri.ToString(), "userId", appUser.Id);
            emailVerificationUri = QueryHelpers.AddQueryString(emailVerificationUri, "code", code);

            return emailVerificationUri;
        }



        public async Task<string> ConfirmEmailAsync(string appUserId, string code)
        {
            var appUser = await _userManager.FindByIdAsync(appUserId);
            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var result = await _userManager.ConfirmEmailAsync(appUser, code);

            if (!result.Succeeded) throw new ApiException($"An error occured while confirming {appUser.Email}.");

            return $"congratulations, your email: {appUser.UserName} has been validated. You can login to your account";
        }



        public async Task<string> ResetPassword(ResetPasswordRequest resetPasswordRequest)
        {
            var account = await _userManager.FindByEmailAsync(resetPasswordRequest.Email);
            if (account == null) throw new ApiException($"No Accounts Registered with {resetPasswordRequest.Email}.");

            var result = await _userManager.ResetPasswordAsync(account, resetPasswordRequest.Token, resetPasswordRequest.Password);

            if (!result.Succeeded) throw new ApiException($"Error occured while reseting the password.");
            return $"{resetPasswordRequest.Email}, message: Password Resetted.";
        }



        public Task<AuthenticationModel> AddToWorkstationAsync(AppUser appUser, IdentityRole role)
        {
            throw new NotImplementedException();
        }



        public Task<AuthenticationModel> RemoveFromWorkstationAsync(AppUser appUser, IdentityRole role)
        {
            throw new NotImplementedException();
        }

        public async Task<AppUser> FindByNameAsync(string userName)
        {
            return await _userManager.FindByNameAsync(userName);
            //return await _userManager.Users.Where(x=> x.UserName.Equals(userName, StringComparison.OrdinalIgnoreCase)).in;
        }

        public async Task<AppUser> FindByEmailAsync(string userEmail)
        {
            return await _userManager.FindByEmailAsync(userEmail);
        }

        public Task<ClaimsPrincipal> GetPrincipalFromExpiredToken(string token)
        {
            throw new NotImplementedException();
        }
    }

    public static class UtilisateurExtension
    {
        public static Dictionary<string, string> ToDictionary(this AppUser utilisateur)
        {
            return new Dictionary<string, string>
                {
                    { "Name", utilisateur.FirstName +" "+utilisateur.LastName},
                    { "Email", utilisateur.Email },
                    { "PhoneNumber", utilisateur.PhoneNumber },
                };
        }
    }
}
