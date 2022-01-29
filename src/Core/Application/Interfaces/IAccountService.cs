using Application.Features.AppUsers.Queries.GetAppUsers;
using Application.Features.AppUsers.Queries.GetPagedList;
using Application.Wrappers;
using Domain.Entities;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IAccountService
    {
        Task<PagedList<AppUser>> GetPagedListAsync(GetAppUsersQuery appUsersQuery);
        Task<int> CountUsersAsync();
        Task<AppUser> GetByIdAsync(string id);
        Task<AuthenticationResponse> RegisterAsync(AppUser appUser, string password);
        Task<AuthenticationResponse> ConfirmEmailAsync(string userId, string token);
        Task<AuthenticationResponse> UpdateAsync(AppUser appUser);
        Task<AuthenticationResponse> DeleteAsync(AppUser appUser);
        Task<string> GenerateEmailConfirmationTokenAsync(AppUser appUser);
        Task<string> EncodeTokenAsync(string token);
        Task<string> DecodeTokenAsync(string encodedToken);
        Task<AuthenticationResponse> AuthenticateAsync(AuthenticationRequest request, string ipAddress);
        Task SignOutAsync();
        Task<AppUser> FindByEmailAsync(string email);
        Task<string> GeneratePasswordResetTokenAsync(AppUser appUser);
        Task<AuthenticationResponse> ForgetPasswordAsync(string email);
        Task<IdentityResult> ResetPasswordAsync(AppUser appUser, string token, string password);
        Task<bool> IsEmailConfirmedAsync(AppUser appUser);

        Task<ICollection<string>> GetUsersWorkstationsAsync(AppUser user);
    }
}
