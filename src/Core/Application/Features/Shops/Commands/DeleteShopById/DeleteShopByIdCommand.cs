using Application.Exceptions;
using Application.Interfaces;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Shops.Commands.DeleteShopById
{
    public class DeleteShopByIdCommand : IRequest
    {
        public Guid Id { get; set; }

        public class DeleteShopByIdCommandHandler : IRequestHandler<DeleteShopByIdCommand>
        {
            private readonly IRepositoryWrapper _repository;

            public DeleteShopByIdCommandHandler(IRepositoryWrapper repository)
            {
                _repository = repository;
            }

            public async Task<Unit> Handle(DeleteShopByIdCommand command, CancellationToken cancellationToken)
            {
                var shop = await _repository.Shop.GetByIdAsync(command.Id);
                if (shop == null) throw new ApiException($"Shop Not Found.");
                await _repository.Shop.DeleteAsync(shop);
                await _repository.SaveAsync();

                return Unit.Value;
            }
        }
    }
}
