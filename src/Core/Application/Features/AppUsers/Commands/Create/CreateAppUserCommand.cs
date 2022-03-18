using Application.Features.AppUsers.Queries.GetById;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.AppUsers.Commands.Create
{
    public class CreateAppUserCommand : IRequest<AppUserViewModel>
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

    internal class CreateAppUserCommandHandler : IRequestHandler<CreateAppUserCommand, AppUserViewModel>
    {
        private readonly ILogger<CreateAppUserCommandHandler> _logger;
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;


        public CreateAppUserCommandHandler(IRepositoryWrapper repository, IMapper mapper, ILogger<CreateAppUserCommandHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }


        public async Task<AppUserViewModel> Handle(CreateAppUserCommand command, CancellationToken cancellationToken)
        {
            var appUserEntity = _mapper.Map<AppUser>(command);

            await _repository.AppUser.CreateAsync(appUserEntity);
            await _repository.SaveAsync();

            return _mapper.Map<AppUserViewModel>(appUserEntity);
        }
    }
}
