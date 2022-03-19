using Application.DataTransfertObjects.Account;
using Application.Features.Account.Commands.Authenticate;
using Domain.Entities;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IAccountNewRepository
    {
        Task<AuthenticationNewModel> AuthenticateAsync(LoginModel loginModel, string ipAddress);
        Task<string> RegisterAsync(RegisterRequest registerRequest, string origin);
        Task<string> ConfirmEmailAsync(string userId, string code);
        Task ForgotPassword(ForgotPasswordRequest model, string origin);
        Task<string> ResetPassword(ResetPasswordRequest model);
    }
}
