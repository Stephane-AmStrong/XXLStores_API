using Application.Exceptions;
using Application.Interfaces;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.ShoppingCarts.Commands.Delete
{
    public class DeleteShoppingCartCommand : IRequest
    {
        public Guid Id { get; set; }

        public class DeleteShoppingCartByIdCommandHandler : IRequestHandler<DeleteShoppingCartCommand>
        {
            private readonly IRepositoryWrapper _repository;

            public DeleteShoppingCartByIdCommandHandler(IRepositoryWrapper repository)
            {
                _repository = repository;
            }

            public async Task<Unit> Handle(DeleteShoppingCartCommand command, CancellationToken cancellationToken)
            {
                var shoppingCart = await _repository.ShoppingCart.GetByIdAsync(command.Id);
                if (shoppingCart == null) throw new ApiException($"ShoppingCart with id: {command.Id}, hasn't been found.");
                await _repository.ShoppingCart.DeleteAsync(shoppingCart);
                await _repository.SaveAsync();

                return Unit.Value;
            }
        }
    }
}
