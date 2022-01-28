using Application.Exceptions;
using Application.Interfaces;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.AppUsers.Commands.DeleteAppUserById
{
    public class DeleteAppUserByIdCommand : IRequest
    {
        public string Id { get; set; }

        public class DeleteAppUserByIdCommandHandler : IRequestHandler<DeleteAppUserByIdCommand>
        {
            private readonly IRepositoryWrapper _repository;

            public DeleteAppUserByIdCommandHandler(IRepositoryWrapper repository)
            {
                _repository = repository;
            }

            public async Task<Unit> Handle(DeleteAppUserByIdCommand command, CancellationToken cancellationToken)
            {
                var appUser = await _repository.AccountService.GetAppUserByIdAsync(command.Id);
                if (appUser == null) throw new ApiException($"AppUser Not Found.");
                await _repository.AccountService.DeleteAppUserAsync(appUser);
                await _repository.SaveAsync();

                return Unit.Value;
            }
        }
    }
}
