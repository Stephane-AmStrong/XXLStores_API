using Application.DataTransfertObjects.Email;
using Application.Enums;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Converters;
using Application.Models;

namespace Application.Features.Account.Commands.RegisterUser
{
    public class RegisterUserCommand : IRequest<string>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        //public string RoleName { get; set; }
        public string Email { get; set; }

        //[Newtonsoft.Json.JsonConverter(typeof(StringEnumConverter), converterParameters:typeof(CamelCaseNamingStrategy))]
        public Enums.Roles Role { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
        [System.Text.Json.Serialization.JsonIgnore]
        public string Origin { get; set; }

    }

    internal class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, string>
    {
        private readonly ILogger<RegisterUserCommandHandler> _logger;
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;


        public RegisterUserCommandHandler(IRepositoryWrapper repository, IMapper mapper, ILogger<RegisterUserCommandHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }


        public async Task<string> Handle(RegisterUserCommand command, CancellationToken cancellationToken)
        {
            var appUser = _mapper.Map<AppUser>(command);
            _logger.LogInformation($"Registration attempt with email: {command.Email}");

            var authenticationModel = await _repository.Account.RegisterAsync(appUser, command.Role, command.Password, command.Origin);
            _logger.LogInformation($"Registration succeeds");

            _logger.LogInformation($"Email Sending attempt with email: {command.Email}");
            var message = new Message(new string[] { command.Email }, "Confirm Registration", $"Please confirm your account by visiting this URL {authenticationModel.AccessToken}", null);
            await _repository.Email.SendAsync(message);
            _logger.LogInformation($"Email Sending attempt with email: {command.Email}");

            return $"{authenticationModel.UserInfo["Name"]}, message: User Registered. Please check your email for verification action.";
        }
    }
}
