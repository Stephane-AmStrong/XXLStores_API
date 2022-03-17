using Application.Exceptions;
using Application.Interfaces;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.ShoppingCartItems.Commands.Delete
{
    public class DeleteShoppingCartItemCommand : IRequest
    {
        public Guid Id { get; set; }

        public class DeleteShoppingCartItemByIdCommandHandler : IRequestHandler<DeleteShoppingCartItemCommand>
        {
            private readonly IRepositoryWrapper _repository;

            public DeleteShoppingCartItemByIdCommandHandler(IRepositoryWrapper repository)
            {
                _repository = repository;
            }

            public async Task<Unit> Handle(DeleteShoppingCartItemCommand command, CancellationToken cancellationToken)
            {
                var shoppingCartItem = await _repository.ShoppingCartItem.GetByIdAsync(command.Id);
                if (shoppingCartItem == null) throw new ApiException($"ShoppingCartItem with id: {command.Id}, hasn't been found.");
                await _repository.ShoppingCartItem.DeleteAsync(shoppingCartItem);
                await _repository.SaveAsync();

                return Unit.Value;
            }
        }
    }
}
