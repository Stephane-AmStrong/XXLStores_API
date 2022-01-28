using Application.Exceptions;
using Application.Interfaces;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.ShoppingCartItems.Commands.DeleteShoppingCartItemById
{
    public class DeleteShoppingCartItemByIdCommand : IRequest
    {
        public Guid Id { get; set; }

        public class DeleteShoppingCartItemByIdCommandHandler : IRequestHandler<DeleteShoppingCartItemByIdCommand>
        {
            private readonly IRepositoryWrapper _repository;

            public DeleteShoppingCartItemByIdCommandHandler(IRepositoryWrapper repository)
            {
                _repository = repository;
            }

            public async Task<Unit> Handle(DeleteShoppingCartItemByIdCommand command, CancellationToken cancellationToken)
            {
                var shoppingCartItem = await _repository.ShoppingCartItem.GetByIdAsync(command.Id);
                if (shoppingCartItem == null) throw new ApiException($"ShoppingCartItem Not Found.");
                await _repository.ShoppingCartItem.DeleteAsync(shoppingCartItem);
                await _repository.SaveAsync();

                return Unit.Value;
            }
        }
    }
}
