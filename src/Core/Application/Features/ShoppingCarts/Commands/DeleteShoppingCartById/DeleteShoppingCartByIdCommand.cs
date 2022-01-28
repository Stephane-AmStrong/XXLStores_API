using Application.Exceptions;
using Application.Interfaces;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.ShoppingCarts.Commands.DeleteShoppingCartById
{
    public class DeleteShoppingCartByIdCommand : IRequest
    {
        public Guid Id { get; set; }

        public class DeleteShoppingCartByIdCommandHandler : IRequestHandler<DeleteShoppingCartByIdCommand>
        {
            private readonly IRepositoryWrapper _repository;

            public DeleteShoppingCartByIdCommandHandler(IRepositoryWrapper repository)
            {
                _repository = repository;
            }

            public async Task<Unit> Handle(DeleteShoppingCartByIdCommand command, CancellationToken cancellationToken)
            {
                var shoppingCart = await _repository.ShoppingCart.GetByIdAsync(command.Id);
                if (shoppingCart == null) throw new ApiException($"ShoppingCart Not Found.");
                await _repository.ShoppingCart.DeleteAsync(shoppingCart);
                await _repository.SaveAsync();

                return Unit.Value;
            }
        }
    }
}
