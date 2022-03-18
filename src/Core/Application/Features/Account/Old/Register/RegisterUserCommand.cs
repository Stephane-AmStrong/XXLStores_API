using Application.Features.AppUsers.Queries.GetById;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Account.Commands.Register
{
    public class RegisterUserCommand : IRequest<AppUserViewModel>
    {
        public string ImgLink { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string RoleName { get; set; }

        public string Email { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }

    internal class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, AppUserViewModel>
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


        public async Task<AppUserViewModel> Handle(RegisterUserCommand command, CancellationToken cancellationToken)
        {
            var appUser = _mapper.Map<AppUser>(command);

            await _repository.Account.RegisterAsync(appUser, command.Password);
            await _repository.SaveAsync();

            return _mapper.Map<AppUserViewModel>(appUser);
        }
    }
}
