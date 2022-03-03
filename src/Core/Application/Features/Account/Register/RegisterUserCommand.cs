using Application.Features.AppUsers.Queries.GetById;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Account.Register
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

    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, AppUserViewModel>
    {
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;


        public RegisterUserCommandHandler(IRepositoryWrapper repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
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
