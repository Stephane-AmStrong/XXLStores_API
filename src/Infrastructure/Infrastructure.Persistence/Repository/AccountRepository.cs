using Application.Exceptions;
using Application.Interfaces;
using Domain.Entities;
using Domain.Models;
using Domain.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.WebUtilities;
using Application.DataTransfertObjects.Account;
using Application.Helpers;

namespace Infrastructure.Persistence.Repository
{
    public class AccountRepository : IAccountRepository
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly JWTSettings _jwtSettings;


        public AccountRepository
        (
            UserManager<AppUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IOptions<JWTSettings> jwtSettings
        )
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _jwtSettings = jwtSettings.Value;
        }



        public async Task<AuthenticationModel> AuthenticateAsync(LoginModel loginModel, string ipAddress)
        {
            var user = await _userManager.FindByNameAsync(loginModel.Email);
            if (user == null) throw new ApiException($"No Accounts Registered with {loginModel.Email}.");

            var authenticationSucceeded = await _userManager.CheckPasswordAsync(user, loginModel.Password);
            if (!authenticationSucceeded) throw new ApiException($"Invalid Credentials for '{loginModel.Email}'.");

            var isEmailConfirmed = await _userManager.IsEmailConfirmedAsync(user);
            if (!isEmailConfirmed) throw new ApiException($"Account Not Confirmed for '{loginModel.Email}'.");

            var jwtSecurityToken = await GenerateJWToken(user);
            var refreshToken = await GenerateRefreshToken(ipAddress);

            return new AuthenticationModel
            {
                AppUser = user,
                Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                RefreshToken = refreshToken,
                UserInfo = user.ToDictionary(),
                IsSuccess = true
            };
        }



        public async Task<AuthenticationModel> RegisterAsync(AppUser user, string password, string origin)
        {
            user.UserName = user.Email;

            var userWithSameUserName = await _userManager.FindByNameAsync(user.UserName);
            if (userWithSameUserName != null) throw new ApiException($"Username '{user.UserName}' is already taken.");

            var userWithSameEmail = await _userManager.FindByEmailAsync(user.Email);
            if (userWithSameEmail != null) throw new ApiException($"Email {user.Email } is already registered.");

            var result = await _userManager.CreateAsync(user, password);
            var errorMessage = new StringBuilder();

            foreach (var error in result.Errors)
            {
                errorMessage.Append(error.Description);
            }

            if (!result.Succeeded) throw new ApiException($"{errorMessage}");

            //await _userManager.AddToRoleAsync(user, Roles.Vendor.ToString());
            var emailConfirmationToken = await GenerateEmailConfirmationTokenAsync(user, origin);
            //TODO: Attach Email Service here and configure it via appsettings
            //await _emailService.SendAsync(new Message { From = "mail@codewithmukesh.com", To = user.Email, Content = $"Please confirm your account by visiting this URL {emailVerificationUri}", Subject = "Confirm Registration" });

            return new AuthenticationModel
            {
                AppUser = user,
                Token = emailConfirmationToken,
                UserInfo = user.ToDictionary(),
                IsSuccess = true
            };
        }

        private async Task<JwtSecurityToken> GenerateJWToken(AppUser user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roleName = (await _userManager.GetRolesAsync(user))[0];
            var role = await _roleManager.FindByNameAsync(roleName);

            var roleClaims = new List<Claim>();
            roleClaims.Add(new Claim(ClaimTypes.Role, roleName));
            roleClaims.AddRange(await _roleManager.GetClaimsAsync(role));


            string ipAddress = IpHelper.GetIpAddress();

            var claims = new[]
            {
                new Claim("ip", ipAddress),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.GivenName, user.LastName),
                new Claim(ClaimTypes.Surname, user.FirstName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            }
            .Union(userClaims)
            .Union(roleClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes),
                signingCredentials: signingCredentials);

            return jwtSecurityToken;
        }

        private async Task<string> GenerateEmailConfirmationTokenAsync(AppUser user, string origin)
        {
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var route = "api/account/confirm-email/";
            var _enpointUri = new Uri(string.Concat($"{origin}/", route));
            var emailVerificationUri = QueryHelpers.AddQueryString(_enpointUri.ToString(), "userId", user.Id);
            emailVerificationUri = QueryHelpers.AddQueryString(emailVerificationUri, "code", code);

            return emailVerificationUri;
        }

        public async Task<string> ConfirmEmailAsync(string userId, string code)
        {
            var user = await _userManager.FindByIdAsync(userId);
            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var result = await _userManager.ConfirmEmailAsync(user, code);
            if (result.Succeeded)
            {
                return $"{user.Id}, message: Account Confirmed for {user.Email}. You can now use the /api/Account/authenticate endpoint.";
            }
            else
            {
                throw new ApiException($"An error occured while confirming {user.Email}.");
            }
        }

        public async Task<RefreshToken> GenerateRefreshToken(string ipAddress)
        {
            var randomTokenString = await RandomTokenString();

            return new RefreshToken
            {
                Token = randomTokenString,
                Expires = DateTime.UtcNow.AddDays(7),
                Created = DateTime.UtcNow,
                CreatedByIp = ipAddress
            };
        }

        public async Task<string> RandomTokenString()
        {
            var random = new Random();
            var randomBytes = new byte[40];

            //random.NextBytes(randomBytes);
            //rngCryptoServiceProvider.GetBytes(randomBytes);

            await Task.Run(() => random.NextBytes(randomBytes));

            using var rngCryptoServiceProvider = new RNGCryptoServiceProvider();
            await Task.Run(() => rngCryptoServiceProvider.GetBytes(randomBytes));

            // convert random bytes to hex string
            return await Task.Run(() => BitConverter.ToString(randomBytes).Replace("-", ""));
        }

        public async Task<AuthenticationModel> GeneratePasswordResetTokenAsync(ForgotPasswordRequest model)
        {
            var account = await _userManager.FindByEmailAsync(model.Email);

            // always return ok response to prevent email enumeration
            if (account == null)
            {
                return new AuthenticationModel
                {
                    IsSuccess = false,
                    Message = "No user associated with this email",
                };
            }

            var code = await _userManager.GeneratePasswordResetTokenAsync(account);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

            /*
            var route = "api/account/reset-password/";
            var _enpointUri = new Uri(string.Concat($"{origin}/", route));
            
            var emailRequest = new Message()
            {
                Content = $"You reset token is - {code}",
                To = model.Email,
                Subject = "Reset Password",
            };
            */

            return new AuthenticationModel
            {
                Token = code,
                IsSuccess = true,
                Message = "Password reset url has been sent to your email successfully",
            };
        }

        public async Task<string> ResetPassword(ResetPasswordRequest model)
        {
            var account = await _userManager.FindByEmailAsync(model.Email);
            if (account == null) throw new ApiException($"No Accounts Registered with {model.Email}.");

            var result = await _userManager.ResetPasswordAsync(account, model.Token, model.Password);
            if (result.Succeeded)
            {
                return $"{model.Email}, message: Password Resetted.";
            }
            else
            {
                throw new ApiException($"Error occured while reseting the password.");
            }
        }

        public Task<AuthenticationModel> AddToWorkstationAsync(AppUser user, IdentityRole role)
        {
            throw new NotImplementedException();
        }

        public Task<AuthenticationModel> RemoveFromWorkstationAsync(AppUser user, IdentityRole role)
        {
            throw new NotImplementedException();
        }
    }

    public static class AppUserExtension
    {
        public static Dictionary<string, string> ToDictionary(this AppUser appUser)
        {
            return new Dictionary<string, string>
                {
                    { "ImgUrl", appUser.ImgLink },
                    { "Name", appUser.FirstName +" "+appUser.LastName},
                    { "Email", appUser.Email },
                    { "PhoneNumber", appUser.PhoneNumber },
                };
        }
    }
}
