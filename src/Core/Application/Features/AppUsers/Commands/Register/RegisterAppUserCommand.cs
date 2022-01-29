using Application.Features.AppUsers.Queries.GetAppUserById;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using MediatR;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.AppUsers.Commands.Register
{
    public class RegisterAppUserCommand : IRequest<GetAppUserViewModel>
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

    public class CreateAppUserCommandHandler : IRequestHandler<RegisterAppUserCommand, GetAppUserViewModel>
    {
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;


        public CreateAppUserCommandHandler(IRepositoryWrapper repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }


        public async Task<GetAppUserViewModel> Handle(RegisterAppUserCommand command, CancellationToken cancellationToken)
        {
            var appUserEntity = _mapper.Map<AppUser>(command);

            await _repository.AccountService.RegisterAsync(appUserEntity, command.Password);
            await _repository.SaveAsync();

            return _mapper.Map<GetAppUserViewModel>(appUserEntity);
        }
    }
}
