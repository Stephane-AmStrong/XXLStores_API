using Application.Exceptions;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Security.Cryptography;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Collections.Generic;
using Application.Helpers;
using Microsoft.IdentityModel.Tokens;
using Domain.Settings;
using Microsoft.Extensions.Options;
using System.Text.Json.Serialization;

namespace Application.Features.Account.Commands.Authenticate
{
    public class AuthenticationCommand : IRequest<AuthenticationViewModel>
    {
        public string Email { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [JsonIgnore]
        public string IpAddress { get; set; }
    }


    internal class AuthenticationRequestCommandHandler : IRequestHandler<AuthenticationCommand, AuthenticationViewModel>
    {
        private readonly ILogger<AuthenticationRequestCommandHandler> _logger;
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;


        public AuthenticationRequestCommandHandler(IRepositoryWrapper repository, IMapper mapper, ILogger<AuthenticationRequestCommandHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }


        public async Task<AuthenticationViewModel> Handle(AuthenticationCommand command, CancellationToken cancellationToken)
        {
            var loginModel = _mapper.Map<LoginModel>(command);

            _logger.LogInformation($"Authentication attempt with email: {command.Email}");
            var authenticationModel = await _repository.Account.AuthenticateAsync(loginModel, command.IpAddress);
            _logger.LogInformation($"Authentication succeeds");

            var authenticationViewModel = _mapper.Map<AuthenticationViewModel>(authenticationModel);

            return authenticationViewModel;
        }
    }
}
