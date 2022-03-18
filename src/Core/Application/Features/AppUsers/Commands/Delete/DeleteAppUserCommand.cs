using Application.Exceptions;
using Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.AppUsers.Commands.Delete
{
    public class DeleteAppUserCommand : IRequest
    {
        public string Id { get; set; }
    }

    internal class DeleteAppUserByIdCommandHandler : IRequestHandler<DeleteAppUserCommand>
    {
        private readonly ILogger<DeleteAppUserByIdCommandHandler> _logger;
        private readonly IRepositoryWrapper _repository;

        public DeleteAppUserByIdCommandHandler(IRepositoryWrapper repository, ILogger<DeleteAppUserByIdCommandHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<Unit> Handle(DeleteAppUserCommand command, CancellationToken cancellationToken)
        {
            var appUser = await _repository.AppUser.GetByIdAsync(command.Id);
            if (appUser == null) throw new ApiException($"AppUser with id: {command.Id}, hasn't been found.");
            await _repository.AppUser.DeleteAsync(appUser);
            await _repository.SaveAsync();

            return Unit.Value;
        }
    }
}
