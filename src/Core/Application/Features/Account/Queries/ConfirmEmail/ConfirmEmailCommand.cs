using Application.Exceptions;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Account.Queries.ConfirmEmail
{
    public class ConfirmEmailCommand : IRequest<string>
    {
        public string UserId { get; set; }
        public string Code { get; set; }
    }

    internal class ConfirmEmailCommandHandler : IRequestHandler<ConfirmEmailCommand, string>
    {
        private readonly ILogger<ConfirmEmailCommandHandler> _logger;
        private readonly UserManager<AppUser> _userManager;
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;


        public ConfirmEmailCommandHandler(IRepositoryWrapper repository, IMapper mapper, ILogger<ConfirmEmailCommandHandler> logger, UserManager<AppUser> userManager)
        {
            _userManager = userManager;
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }


        public async Task<string> Handle(ConfirmEmailCommand command, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(command.UserId);
            command.Code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(command.Code));

            var result = await _userManager.ConfirmEmailAsync(user, command.Code);
            if (result.Succeeded)
            {
                return $"{user.Id}, message: Account Confirmed for {user.Email}. You can now use the /api/Account/authenticate endpoint.";
            }
            else
            {
                throw new ApiException($"An error occured while confirming {user.Email}.");
            }
        }
    }
}
