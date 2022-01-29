using Application.Exceptions;
using Application.Features.AppUsers.Queries.GetAppUserById;
using Application.Interfaces;
using Application.Wrappers;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.AppUsers.Commands.Update
{
    public class UpdateAppUserCommand : IRequest<GetAppUserViewModel>
    {
        public string Id { get; set; }
        public string ImgLink { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string RoleName { get; set; }
    }

    public class UpdateAppUserCommandHandler : IRequestHandler<UpdateAppUserCommand, GetAppUserViewModel>
    {
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;

        public UpdateAppUserCommandHandler(IRepositoryWrapper repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<GetAppUserViewModel> Handle(UpdateAppUserCommand command, CancellationToken cancellationToken)
        {
            var appUserEntity = await _repository.AccountService.GetByIdAsync(command.Id);

            if (appUserEntity == null)
            {
                throw new ApiException($"AppUser Not Found.");
            }

            _mapper.Map(command, appUserEntity);

            await _repository.AccountService.UpdateAsync(appUserEntity);
            await _repository.SaveAsync();

            var appUserReadDto = _mapper.Map<GetAppUserViewModel>(appUserEntity);

            //if (!string.IsNullOrWhiteSpace(appUserReadDto.ImgLink)) appUserReadDto.ImgLink = $"{_baseURL}{appUserReadDto.ImgLink}";

            return appUserReadDto;


        }
    }

}
