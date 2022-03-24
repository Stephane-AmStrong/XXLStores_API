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
        private readonly UserManager<AppUser> _userManager;
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;


        public ResetPasswordCommandHandler(IRepositoryWrapper repository, IMapper mapper, ILogger<ResetPasswordCommandHandler> logger, UserManager<AppUser> userManager)
        {
            _userManager = userManager;
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }


        public async Task<string> Handle(ResetPasswordCommand command, CancellationToken cancellationToken)
        {
            var account = await _userManager.FindByNameAsync(command.Email);
            if (account == null) throw new ApiException($"No Accounts Registered with {command.Email}.");
            var result = await _userManager.ResetPasswordAsync(account, command.Token, command.Password);
            if (result.Succeeded)
            {
                return $"{command.Email}, message: Password Resetted.";
            }
            else
            {
                throw new ApiException($"Error occured while reseting the password.");
            }
        }
    }
}
