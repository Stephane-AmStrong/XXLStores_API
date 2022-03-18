using Application.Exceptions;
using Application.Features.AppUsers.Queries.GetById;
using Application.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.AppUsers.Commands.Update
{
    public class UpdateAppUserCommand : IRequest<AppUserViewModel>
    {
        public string Id { get; set; }
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

    public class UpdateAppUserCommandHandler : IRequestHandler<UpdateAppUserCommand, AppUserViewModel>
    {
        private readonly ILogger<UpdateAppUserCommandHandler> _logger;
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;

        public UpdateAppUserCommandHandler(IRepositoryWrapper repository, IMapper mapper, ILogger<UpdateAppUserCommandHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<AppUserViewModel> Handle(UpdateAppUserCommand command, CancellationToken cancellationToken)
        {
            var appUserEntity = await _repository.AppUser.GetByIdAsync(command.Id);
            if (appUserEntity == null) throw new ApiException($"AppUser with id: {command.Id}, hasn't been found.");

            _mapper.Map(command, appUserEntity);
            await _repository.AppUser.UpdateAsync(appUserEntity);
            await _repository.SaveAsync();

            var appUserReadDto = _mapper.Map<AppUserViewModel>(appUserEntity);
            //if (!string.IsNullOrWhiteSpace(appUserReadDto.ImgLink)) appUserReadDto.ImgLink = $"{_baseURL}{appUserReadDto.ImgLink}";
            return appUserReadDto;
        }
    }

}
