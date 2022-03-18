using Application.Exceptions;
using Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Shops.Commands.Delete
{
    public class DeleteShopCommand : IRequest
    {
        public Guid Id { get; set; }
    }

    internal class DeleteShopByIdCommandHandler : IRequestHandler<DeleteShopCommand>
    {
        private readonly ILogger<DeleteShopByIdCommandHandler> _logger;
        private readonly IRepositoryWrapper _repository;

        public DeleteShopByIdCommandHandler(IRepositoryWrapper repository, ILogger<DeleteShopByIdCommandHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<Unit> Handle(DeleteShopCommand command, CancellationToken cancellationToken)
        {
            var shop = await _repository.Shop.GetByIdAsync(command.Id);
            if (shop == null) throw new ApiException($"Shop with id: {command.Id}, hasn't been found.");
            await _repository.Shop.DeleteAsync(shop);
            await _repository.SaveAsync();

            return Unit.Value;
        }
    }
}
