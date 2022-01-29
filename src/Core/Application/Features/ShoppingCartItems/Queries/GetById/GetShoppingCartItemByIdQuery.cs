using Application.Exceptions;
using Application.Interfaces;
using AutoMapper;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.ShoppingCartItems.Queries.GetById
{
    public class GetShoppingCartItemByIdQuery : IRequest<ShoppingCartItemViewModel>
    {
        public Guid Id { get; set; }

        public class GetShoppingCartItemByIdQueryHandler : IRequestHandler<GetShoppingCartItemByIdQuery, ShoppingCartItemViewModel>
        {
            private readonly IRepositoryWrapper _repository;
            private readonly IMapper _mapper;

            public GetShoppingCartItemByIdQueryHandler(IRepositoryWrapper repository, IMapper mapper)
            {
                _repository = repository;
                _mapper = mapper;
            }

            public async Task<ShoppingCartItemViewModel> Handle(GetShoppingCartItemByIdQuery query, CancellationToken cancellationToken)
            {
                var shoppingCartItem = await _repository.ShoppingCartItem.GetByIdAsync(query.Id);
                if (shoppingCartItem == null) throw new ApiException($"ShoppingCartItem Not Found.");
                return _mapper.Map<ShoppingCartItemViewModel>(shoppingCartItem);
            }
        }
    }
}
