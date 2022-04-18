using Application.DataTransfertObjects.Email;
using Application.Enums;
using Application.Exceptions;
using Application.Features.AppUsers.Queries.GetById;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using Microsoft.AspNetCore.WebUtilities;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Application.DataTransfertObjects.Account;

namespace Application.Features.Account.Commands.ResetPassword
{
    public class ResetPasswordCommand : IRequest<string>
    {
        public string Email { get; set; }
        public string Token { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }

    internal class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, string>
    {
        private readonly ILogger<ResetPasswordCommandHandler> _logger;
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;


        public ResetPasswordCommandHandler(IRepositoryWrapper repository, IMapper mapper, ILogger<ResetPasswordCommandHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }


        public async Task<string> Handle(ResetPasswordCommand command, CancellationToken cancellationToken)
        {
            var resetPasswordRequest = _mapper.Map<ResetPasswordRequest>(command);
            _logger.LogInformation($"Reset Password attempt with email: {command.Email} and resetToken: {command.Token}");

            var resetPasswordMessage = await _repository.Account.ResetPassword(resetPasswordRequest);
            _logger.LogInformation($"Reset Password succeeds");

            return resetPasswordMessage;
        }
    }
}
