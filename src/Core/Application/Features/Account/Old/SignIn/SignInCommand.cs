using Application.Features.AppUsers.Queries.GetById;
using Application.Interfaces;
using AutoMapper;
using Domain.Models;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Account.Commands.SignIn
{
    public class SignInCommand : IRequest<AppUserViewModel>
    {
        public string Email { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }

    internal class SignInCommandHandler : IRequestHandler<SignInCommand, AppUserViewModel>
    {
        private readonly ILogger<SignInCommandHandler> _logger;
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;

        public SignInCommandHandler(IRepositoryWrapper repository, IMapper mapper, ILogger<SignInCommandHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }


        public async Task<AppUserViewModel> Handle(SignInCommand command, CancellationToken cancellationToken)
        {
            var loginModel = _mapper.Map<LoginModel>(command);

            var authenticationModel = await _repository.Account.SignInAsync(loginModel);

            return _mapper.Map<AppUserViewModel>(authenticationModel);
        }
    }


}
