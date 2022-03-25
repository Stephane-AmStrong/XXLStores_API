using Application.DataTransfertObjects.Account;
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
        Task<AuthenticationModel> AuthenticateAsync(LoginModel loginModel, string ipAddress);
        Task<string> ConfirmEmailAsync(string userId, string code);
        Task<AuthenticationModel> GeneratePasswordResetTokenAsync(ForgotPasswordRequest model);
        //Task<AuthenticationModel> GenerateEmailConfirmationTokenAsync(AppUser user, string origin);
        Task<AuthenticationModel> RegisterAsync(AppUser user, string password, string origin);
        Task<string> ResetPassword(ResetPasswordRequest model);
        Task<AuthenticationModel> AddToWorkstationAsync(AppUser user, IdentityRole role);
        Task<AuthenticationModel> RemoveFromWorkstationAsync(AppUser user, IdentityRole role);
    }
}
