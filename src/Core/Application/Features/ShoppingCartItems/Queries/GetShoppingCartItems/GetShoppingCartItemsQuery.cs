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

namespace Application.Features.ShoppingCartItems.Queries.GetShoppingCartItems
{
    public class GetShoppingCartItemsQuery : QueryStringParameters, IRequest<PagedList<ShoppingCartItemsViewModel>>
    {
        public GetShoppingCartItemsQuery()
        {
            OrderBy = "name";
        }

        public string WithTheName { get; set; }
    }

    public class GetAllShoppingCartItemsQueryHandler : IRequestHandler<GetShoppingCartItemsQuery, PagedList<ShoppingCartItemsViewModel>>
    {
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;

        public GetAllShoppingCartItemsQueryHandler(IRepositoryWrapper repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }


        public async Task<PagedList<ShoppingCartItemsViewModel>> Handle(GetShoppingCartItemsQuery query, CancellationToken cancellationToken)
        {
            var shoppingCartItems = await _repository.ShoppingCartItem.GetPagedListAsync(query);
            return  _mapper.Map<PagedList<ShoppingCartItemsViewModel>>(shoppingCartItems);
        }
    }
}
