using Application.Interfaces;
using Application.Wrappers;
using AutoMapper;
using Domain.Parameters;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.ShoppingCarts.Queries.GetPagedList
{
    public class GetShoppingCartsQuery : QueryParameters, IRequest<PagedListResponse<ShoppingCartsViewModel>>
    {
        public GetShoppingCartsQuery()
        {
            OrderBy = "name";
        }

        public string WithTheName { get; set; }
    }

    internal class GetShoppingCartsQueryHandler : IRequestHandler<GetShoppingCartsQuery, PagedListResponse<ShoppingCartsViewModel>>
    {
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;

        public GetShoppingCartsQueryHandler(IRepositoryWrapper repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<PagedListResponse<ShoppingCartsViewModel>> Handle(GetShoppingCartsQuery query, CancellationToken cancellationToken)
        {
            var shoppingCarts = await _repository.ShoppingCart.GetPagedListAsync(query);
            var shoppingCartsViewModel = _mapper.Map<List<ShoppingCartsViewModel>>(shoppingCarts);
            return new PagedListResponse<ShoppingCartsViewModel>(shoppingCartsViewModel, shoppingCarts.MetaData);
        }
    }
}
