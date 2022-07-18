using Application.DataTransfertObjects.Account;
using Application.Enums;
using Application.Models;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IAccountRepository
    {
        Task<AuthenticationModel> RegisterAsync(AppUser appUser, Roles role, string password, string origin);
        Task<AuthenticationModel> RegisterAsync(AppUser appUser, string roleName, string password, string origin);
        Task<string> ConfirmEmailAsync(string appUserId, string code);
        Task<AppUser> FindByNameAsync(string userName);
        Task<AppUser> FindByEmailAsync(string userEmail);
        Task<AuthenticationModel> AuthenticateAsync(LoginModel loginModel, string ipAddress);
        Task<string> ResetPassword(ResetPasswordRequest resetPasswordRequest);
        Task<AuthenticationModel> AddToWorkstationAsync(AppUser appUser, IdentityRole role);
        Task<AuthenticationModel> RemoveFromWorkstationAsync(AppUser appUser, IdentityRole role);
    }
}
