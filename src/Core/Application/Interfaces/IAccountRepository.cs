using Application.Wrappers;
using Domain.Entities;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IAccountRepository
    {
        Task<AuthenticationModel> RegisterAsync(AppUser appUser, string password);
        Task<AuthenticationModel> UpdateAsync(AppUser appUser);
        Task<AuthenticationModel> ConfirmEmailAsync(string userId, string token);
        Task<string> GenerateEmailConfirmationTokenAsync(AppUser appUser);
        Task<string> EncodeTokenAsync(string token);
        Task<string> DecodeTokenAsync(string encodedToken);
        Task<AuthenticationModel> SignInAsync(LoginModel loginModel);
        Task SignOutAsync();
        Task<AppUser> FindByEmailAsync(string email);
        Task<string> GeneratePasswordResetTokenAsync(AppUser appUser);
        Task<AuthenticationModel> ForgetPasswordAsync(string email);
        Task<IdentityResult> ResetPasswordAsync(AppUser appUser, string token, string password);
        Task<bool> IsEmailConfirmedAsync(AppUser appUser);
        Task<string> GetId(ClaimsPrincipal user);

        Task<ICollection<string>> GetWorkstationsAsync(AppUser user);
        Task<AuthenticationModel> AddToWorkstationAsync(AppUser user, IdentityRole role);
        Task<AuthenticationModel> RemoveFromWorkstationAsync(AppUser user, IdentityRole role);
    }
}
