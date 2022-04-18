using Application.DataTransfertObjects.Account;
using Application.Enums;
using Application.Features.Account.Commands.Authenticate;
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
        Task<AuthenticationModel> RegisterAsync(AppUser user, Roles role, string password, string origin);
        Task<string> ConfirmEmailAsync(string userId, string code);
        Task<AuthenticationModel> AuthenticateAsync(LoginModel loginModel, string ipAddress);
        Task<AuthenticationModel> GeneratePasswordResetTokenAsync(string email);
        Task<ClaimsPrincipal> GetPrincipalFromExpiredTokenAsync(string token);
        Task<string> ResetPassword(ResetPasswordRequest resetPasswordRequest);
        Task<AuthenticationModel> AddToWorkstationAsync(AppUser user, IdentityRole role);
        Task<AuthenticationModel> RemoveFromWorkstationAsync(AppUser user, IdentityRole role);
    }
}
