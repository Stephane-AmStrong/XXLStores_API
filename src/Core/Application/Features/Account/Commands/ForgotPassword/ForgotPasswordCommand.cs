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

namespace Application.Features.Account.Commands.ForgotPassword
{
    public class ForgotPasswordCommand : IRequest
    {
        public string Email { get; set; }
        public string Origin { get; set; }
    }

    internal class ForgotPasswordCommandHandler : IRequestHandler<ForgotPasswordCommand>
    {
        private readonly ILogger<ForgotPasswordCommandHandler> _logger;
        private readonly UserManager<AppUser> _userManager;
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;


        public ForgotPasswordCommandHandler(IRepositoryWrapper repository, IMapper mapper, ILogger<ForgotPasswordCommandHandler> logger, UserManager<AppUser> userManager)
        {
            _userManager = userManager;
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }


        public async Task<Unit> Handle(ForgotPasswordCommand command, CancellationToken cancellationToken)
        {
            var account = await _userManager.FindByEmailAsync(command.Email);

            // always return ok response to prevent email enumeration
            if (account == null) return Unit.Value;

            var code = await _userManager.GeneratePasswordResetTokenAsync(account);
            var route = "api/account/reset-password/";
            var _enpointUri = new Uri(string.Concat($"{command.Origin}/", route));

            var emailRequest = new Message()
            {
                Content = $"You reset token is - {code}",
                To = command.Email,
                Subject = "Reset Password",
            };
            await _repository.Email.SendAsync(emailRequest);

            return Unit.Value;
        }
    }
}
