using Application.Exceptions;
using Application.Interfaces;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Shops.Commands.Delete
{
    public class DeleteShopCommand : IRequest
    {
        public Guid Id { get; set; }

        public class DeleteShopByIdCommandHandler : IRequestHandler<DeleteShopCommand>
        {
            private readonly IRepositoryWrapper _repository;

            public DeleteShopByIdCommandHandler(IRepositoryWrapper repository)
            {
                _repository = repository;
            }

            public async Task<Unit> Handle(DeleteShopCommand command, CancellationToken cancellationToken)
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
