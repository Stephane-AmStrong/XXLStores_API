using Application.Interfaces;
using Domain.Entities;
using Domain.Models;
using Infrastructure.Persistence.Contexts;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Repository
{
    public class AccountOldRepository : RepositoryBase<AppUser>, IAccountOldRepository
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly HttpContext _httpContext;
        //private readonly string _baseURL;


        public AccountOldRepository(ApplicationDbContext appDbContext, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration, IHttpContextAccessor httpContextAccessor) : base(appDbContext)
        {
            _configuration = configuration;
            _userManager = userManager;
            _roleManager = roleManager;
            _httpContext = httpContextAccessor.HttpContext;
            //_baseURL = string.Concat(httpContextAccessor.HttpContext.Request.Scheme, "://", httpContextAccessor.HttpContext.Request.Host);
        }


        public async Task<AuthenticationModel> RegisterAsync(AppUser user, string password)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
            
            var result = await _userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                var generatedToken = await GenerateEmailConfirmationTokenAsync(user);
                var encodeToken = await EncodeTokenAsync(generatedToken);

                return new AuthenticationModel
                {
                    Token = encodeToken,
                    IsSuccess = true,
                };
            }

            return new AuthenticationModel
            {
                Message = "AppUser is not created",
                IsSuccess = false,
                ErrorDetails = result.Errors.Select(errorDescription => errorDescription.Description)
            };
        }


        public async Task<string> GenerateEmailConfirmationTokenAsync(AppUser appUser)
        {
            return await _userManager.GenerateEmailConfirmationTokenAsync(appUser);
        }


        public async Task<string> EncodeTokenAsync(string token)
        {
            var encodedEmailToken = await Task.Run(() => Encoding.UTF8.GetBytes(token));
            return await Task.Run(() => WebEncoders.Base64UrlEncode(encodedEmailToken));
        }


        public async Task<string> DecodeTokenAsync(string encodedToken)
        {
            var decodedToken = await Task.Run(() => WebEncoders.Base64UrlDecode(encodedToken));
            return await Task.Run(() => Encoding.UTF8.GetString(decodedToken));
        }


        public async Task<AuthenticationModel> ConfirmEmailAsync(string userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return new AuthenticationModel
                {
                    IsSuccess = false,
                    Message = "User not found !"
                };
            }

            var result = await _userManager.ConfirmEmailAsync(user, await DecodeTokenAsync(token));

            var userInfo = user.ToDictionaryOld();

            if (result.Succeeded)
            {
                return new AuthenticationModel
                {
                    IsSuccess = true,
                    UserInfo = userInfo,
                    Message = "Email confirmed successfuly!"
                };
            }

            return new AuthenticationModel
            {
                IsSuccess = false,
                Message = "Email confirmation failed",
                ErrorDetails = result.Errors.Select(ex => ex.Description)
            };
        }


        public async Task<AuthenticationModel> SignInAsync(LoginModel loginModel)
        {
            if (loginModel == null) throw new ArgumentNullException(nameof(loginModel));

            var user = await _userManager.FindByEmailAsync(loginModel.Email);

            if (user == null)
            {
                return new AuthenticationModel
                {
                    Message = "There is no user with that email adresse",
                    IsSuccess = false,
                };
            }


            var isEmailConfirmed = await _userManager.IsEmailConfirmedAsync(user);

            if (!isEmailConfirmed)
            {
                return new AuthenticationModel
                {
                    Message = "Cannot sign in without a confirmed email",
                    AppUser = user,
                    IsSuccess = false,
                };
            }


            var resultSucceeded = await _userManager.CheckPasswordAsync(user, loginModel.Password);

            if (!resultSucceeded)
            {
                return new AuthenticationModel
                {
                    Message = "Invalid password !",
                    IsSuccess = false,
                };
            }

            var roleName = (await _userManager.GetRolesAsync(user))[0];
            var role = await _roleManager.FindByNameAsync(roleName);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.GivenName, user.LastName),
                new Claim(ClaimTypes.Surname, user.FirstName),
                new Claim(ClaimTypes.Role, roleName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            if (!string.IsNullOrEmpty(user.ImgLink)) claims.Add(new Claim(ClaimTypes.Uri, user.ImgLink));

            claims.AddRange(await _roleManager.GetClaimsAsync(role));

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);


            var authProperties = new AuthenticationProperties
            {
                //AllowRefresh = <bool>,
                // Refreshing the authentication session should be allowed.

                ExpiresUtc = DateTimeOffset.UtcNow.AddDays(1),
                // The time at which the authentication ticket expires. A 
                // value set here overrides the ExpireTimeSpan option of 
                // CookieAuthenticationOptions set with AddCookie.

                IsPersistent = true,
                // Whether the authentication session is persisted across 
                // multiple requests. When used with cookies, controls
                // whether the cookie's lifetime is absolute (matching the
                // lifetime of the authentication ticket) or session-based.

                //IssuedUtc = <DateTimeOffset>,
                // The time at which the authentication ticket was issued.

                //RedirectUri = <string>
                // The full path or absolute URI to be used as an http 
                // redirect response value.
            };

            await _httpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

            // cookieSignIn

            var userInfo = user.ToDictionaryOld();

            return new AuthenticationModel
            {
                UserInfo = userInfo,
                AppUser = user,
                IsSuccess = true,
            };
        }

        public async Task SignOutAsync()
        {
            await _httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }


        public async Task<AuthenticationModel> ForgetPasswordAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return new AuthenticationModel
                {
                    IsSuccess = false,
                    Message = "No user associated with this email",
                };
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var encodedToken = await EncodeTokenAsync(token);

            var userInfo = user.ToDictionaryOld();

            return new AuthenticationModel
            {
                Token = encodedToken,
                IsSuccess = true,
                UserInfo = userInfo,
                Message = "Password reset url has been sent to your email successfully",
            };

        }


        public async Task<AuthenticationModel> AddToIdentityRoleAsync(AppUser user, IdentityRole role)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
            if (role == null) throw new ArgumentNullException(nameof(role));

            var result = await _userManager.AddToRoleAsync(user, role.Name);

            if (result.Succeeded)
            {
                return new AuthenticationModel
                {
                    IsSuccess = true,
                };
            }

            return new AuthenticationModel
            {
                Message = "IdentityRole not assigned",
                IsSuccess = false,
                ErrorDetails = result.Errors.Select(errorDescription => errorDescription.Description)
            };
        }


        public async Task<AuthenticationModel> RemoveFromIdentityRoleAsync(AppUser user, IdentityRole role)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
            if (role == null) throw new ArgumentNullException(nameof(role));

            var result = await _userManager.RemoveFromRoleAsync(user, role.Name);

            if (result.Succeeded)
            {
                return new AuthenticationModel
                {
                    IsSuccess = true,
                };
            }

            return new AuthenticationModel
            {
                Message = "IdentityRole not removed",
                IsSuccess = false,
                ErrorDetails = result.Errors.Select(errorDescription => errorDescription.Description)
            };
        }

        public async Task<AppUser> FindByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        public async Task<string> GeneratePasswordResetTokenAsync(AppUser appUser)
        {
            return await _userManager.GeneratePasswordResetTokenAsync(appUser);
        }

        public async Task<IdentityResult> ResetPasswordAsync(AppUser appUser, string token, string password)
        {
            return await _userManager.ResetPasswordAsync(appUser, token, password);
        }

        public async Task<bool> IsEmailConfirmedAsync(AppUser appUser)
        {
            return await _userManager.IsEmailConfirmedAsync(appUser);
        }

        public Task<AuthenticationModel> UpdateAsync(AppUser appUser)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetId(ClaimsPrincipal user)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<string>> GetWorkstationsAsync(AppUser user)
        {
            throw new NotImplementedException();
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

    public static class AppUserExtensionOld
    {
        public static Dictionary<string, string> ToDictionaryOld(this AppUser appUser)
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
