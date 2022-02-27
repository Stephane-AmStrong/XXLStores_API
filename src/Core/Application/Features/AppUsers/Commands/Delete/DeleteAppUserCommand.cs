using Application.Exceptions;
using Application.Interfaces;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.AppUsers.Commands.Delete
{
    public class DeleteAppUserCommand : IRequest
    {
        public string Id { get; set; }

        public class DeleteAppUserByIdCommandHandler : IRequestHandler<DeleteAppUserCommand>
        {
            private readonly IRepositoryWrapper _repository;

            public DeleteAppUserByIdCommandHandler(IRepositoryWrapper repository)
            {
                _repository = repository;
            }

            public async Task<Unit> Handle(DeleteAppUserCommand command, CancellationToken cancellationToken)
            {
                var appUser = await _repository.AppUser.GetByIdAsync(command.Id);
                if (appUser == null) throw new ApiException($"AppUser Not Found.");
                await _repository.AppUser.DeleteAsync(appUser);
                await _repository.SaveAsync();

                return Unit.Value;
            }
        }
    }
}
