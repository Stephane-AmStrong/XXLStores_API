using Application.Features.AppUsers.Queries.GetById;
using Application.Interfaces;
using Application.Models;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.AppUsers.Commands.Create
{
    public class CreateAppUserCommand : IRequest<AppUserViewModel>
    {
        public string Sexe { get; set; }
        public string Name { get; set; }
        public string FirstName { get; set; }
        public string NameEpoux { get; set; }
        public DateTime? DateNaissance { get; set; }
        public string Adresse { get; set; }
        public string Telephone { get; set; }
        public string PersonneNameFirstName { get; set; }
        public string PersonneTelephone { get; set; }
        public string OperatorId { get; set; }
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
