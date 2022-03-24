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
using System.Text.Json.Serialization;

namespace Application.Features.Account.Commands.RegisterUser
{
    public class RegisterUserCommand : IRequest<string>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        //public string RoleName { get; set; }
        public string Email { get; set; }

        public Roles Role { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
        [JsonIgnore]
        public string Origin { get; set; }

    }

    internal class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, string>
    {
        private readonly ILogger<RegisterUserCommandHandler> _logger;
        private readonly UserManager<AppUser> _userManager;
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;


        public RegisterUserCommandHandler(IRepositoryWrapper repository, IMapper mapper, ILogger<RegisterUserCommandHandler> logger, UserManager<AppUser> userManager)
        {
            _userManager = userManager;
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }


        public async Task<string> Handle(RegisterUserCommand command, CancellationToken cancellationToken)
        {
            var userWithSameUserName = await _userManager.FindByNameAsync(command.Email);
            if (userWithSameUserName != null) throw new ApiException($"Username '{command.Email}' is already taken.");

            var user = new AppUser
            {
                UserName = command.Email,
                FirstName = command.FirstName,
                LastName = command.LastName
            };

            var userWithSameEmail = await _userManager.FindByEmailAsync(command.Email);

            if (userWithSameEmail == null)
            {
                var result = await _userManager.CreateAsync(user, command.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, command.Role.ToString());
                    var verificationUri = await SendVerificationEmail(user, command.Origin);

                    //TODO: Attach Email Service here and configure it via appsettings
                    var message = new Message { From = "mail@codewithmukesh.com", To = user.UserName, Content = $"Please confirm your account by visiting this URL {verificationUri}", Subject = "Confirm Registration" };
                    await _repository.Email.SendAsync(message);

                    return $"{user.Id}, message: User Registered. Please confirm your account by visiting this URL {verificationUri}";
                }
                else
                {
                    var errorMessage = new StringBuilder();

                    foreach (var error in result.Errors)
                    {
                        errorMessage.Append($"{error.Description}");
                        errorMessage.Append(Environment.NewLine);
                    }

                    throw new ApiException($"{errorMessage}");
                }
            }
            else
            {
                throw new ApiException($"Email {command.Email } is already registered.");
            }
        }

        private async Task<string> SendVerificationEmail(AppUser user, string origin)
        {
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var route = "api/account/confirm-email/";
            var _enpointUri = new Uri(string.Concat($"{origin}/", route));
            var verificationUri = QueryHelpers.AddQueryString(_enpointUri.ToString(), "userId", user.Id);
            verificationUri = QueryHelpers.AddQueryString(verificationUri, "code", code);
            //Email Service Call Here
            return verificationUri;
        }
    }
}
