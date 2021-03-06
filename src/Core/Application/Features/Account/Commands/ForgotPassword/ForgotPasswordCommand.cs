using Application.DataTransfertObjects.Email;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using Application.DataTransfertObjects.Account;
using Newtonsoft.Json;

namespace Application.Features.Account.Commands.ForgotPassword
{
    public class ForgotPasswordCommand : IRequest<string>
    {
        public string Email { get; set; }
        [JsonIgnore]
        public string Origin { get; set; }
    }

    internal class ForgotPasswordCommandHandler : IRequestHandler<ForgotPasswordCommand, string>
    {
        private readonly ILogger<ForgotPasswordCommandHandler> _logger;
        private readonly IRepositoryWrapper _repository;


        public ForgotPasswordCommandHandler(IRepositoryWrapper repository, ILogger<ForgotPasswordCommandHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }


        public async Task<string> Handle(ForgotPasswordCommand command, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Forgot password attempt with Email: {command.Email}");
            var authenticationModel = await _repository.Token.GeneratePasswordResetTokenAsync(command.Email);
            _logger.LogInformation($"Forgot password succeeds");

            if (authenticationModel.IsSuccess)
            {
                _logger.LogInformation($"Email Sending attempt with email: {command.Email}");
                var message = new Message(new string[] { command.Email }, "Reset Password", $"You reset token is:  {authenticationModel.AccessToken}", null);
                await _repository.Email.SendAsync(message);
                _logger.LogInformation($"Email Sending attempt with email: {command.Email}");
            }

            return $"Password reset url has been sent to your email successfully.";
        }
    }
}
