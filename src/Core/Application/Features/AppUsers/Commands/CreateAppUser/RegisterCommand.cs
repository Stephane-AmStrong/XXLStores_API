using Application.Features.AppUsers.Queries.GetAppUserById;
using Application.Interfaces;
using Application.Wrappers;
using AutoMapper;
using Domain.Entities;
using MediatR;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.AppUsers.Commands.CreateAppUser
{
    public class RegisterCommand : IRequest<GetAppUserViewModel>
    {
        public string ImgLink { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string RoleName { get; set; }


        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class CreateAppUserCommandHandler : IRequestHandler<RegisterCommand, GetAppUserViewModel>
    {
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;


        public CreateAppUserCommandHandler(IRepositoryWrapper repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }


        public async Task<GetAppUserViewModel> Handle(RegisterCommand command, CancellationToken cancellationToken)
        {
            var appUserEntity = _mapper.Map<AppUser>(command);

            await _repository.AccountService.RegisterAsync(appUserEntity);
            await _repository.SaveAsync();

            return _mapper.Map<GetAppUserViewModel>(appUserEntity);
        }
    }
}
