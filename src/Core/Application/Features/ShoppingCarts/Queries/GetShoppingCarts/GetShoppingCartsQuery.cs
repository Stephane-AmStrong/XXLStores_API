using Application.Interfaces;
using Application.Parameters;
using Application.Wrappers;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.ShoppingCarts.Queries.GetShoppingCarts
{
    public class GetShoppingCartsQuery : QueryStringParameters, IRequest<PagedList<ShoppingCartsViewModel>>
    {
        public GetShoppingCartsQuery()
        {
            OrderBy = "name";
        }

        public string WithTheName { get; set; }
    }

    public class GetAllShoppingCartsQueryHandler : IRequestHandler<GetShoppingCartsQuery, PagedList<ShoppingCartsViewModel>>
    {
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;

        public GetAllShoppingCartsQueryHandler(IRepositoryWrapper repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }


        public async Task<PagedList<ShoppingCartsViewModel>> Handle(GetShoppingCartsQuery query, CancellationToken cancellationToken)
        {
            var shoppingCarts = await _repository.ShoppingCart.GetPagedListAsync(query);
            return  _mapper.Map<PagedList<ShoppingCartsViewModel>>(shoppingCarts);
        }
    }
}
