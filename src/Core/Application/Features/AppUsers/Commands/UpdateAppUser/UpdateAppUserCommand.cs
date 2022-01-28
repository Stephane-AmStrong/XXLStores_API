using Application.Exceptions;
using Application.Features.AppUsers.Queries.GetAppUserById;
using Application.Interfaces;
using Application.Wrappers;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.AppUsers.Commands.UpdateAppUser
{
    public class UpdateAppUserCommand : IRequest<GetAppUserViewModel>
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public float Price { get; set; }
        public string StatusAppUser { get; set; }
        public int NoPriority { get; set; }

        public Guid CategoryId { get; set; }

        public String AppUserId { get; set; }

        public Guid SponsorId { get; set; }


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
                var appUserEntity = await _repository.AccountService.GetAppUserByIdAsync(command.Id);

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
}
