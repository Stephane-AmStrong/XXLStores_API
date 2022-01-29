using Application.Exceptions;
using Application.Interfaces;
using AutoMapper;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.ShoppingCarts.Queries.GetById
{
    public class GetShoppingCartByIdQuery : IRequest<ShoppingCartViewModel>
    {
        public Guid Id { get; set; }

        public class GetShoppingCartByIdQueryHandler : IRequestHandler<GetShoppingCartByIdQuery, ShoppingCartViewModel>
        {
            private readonly IRepositoryWrapper _repository;
            private readonly IMapper _mapper;

            public GetShoppingCartByIdQueryHandler(IRepositoryWrapper repository, IMapper mapper)
            {
                _repository = repository;
                _mapper = mapper;
            }

            public async Task<ShoppingCartViewModel> Handle(GetShoppingCartByIdQuery query, CancellationToken cancellationToken)
            {
                var shoppingCart = await _repository.ShoppingCart.GetByIdAsync(query.Id);
                if (shoppingCart == null) throw new ApiException($"ShoppingCart Not Found.");
                return _mapper.Map<ShoppingCartViewModel>(shoppingCart);
            }
        }
    }
}
