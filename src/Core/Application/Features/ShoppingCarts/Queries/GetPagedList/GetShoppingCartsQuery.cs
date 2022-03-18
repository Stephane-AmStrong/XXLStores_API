using Application.Interfaces;
using Application.Wrappers;
using AutoMapper;
using Domain.Parameters;
using MediatR;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<GetShoppingCartsQueryHandler> _logger;
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;

        public GetShoppingCartsQueryHandler(IRepositoryWrapper repository, IMapper mapper, ILogger<GetShoppingCartsQueryHandler> logger)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<PagedListResponse<ShoppingCartsViewModel>> Handle(GetShoppingCartsQuery query, CancellationToken cancellationToken)
        {
            var shoppingCarts = await _repository.ShoppingCart.GetPagedListAsync(query);
            var shoppingCartsViewModel = _mapper.Map<List<ShoppingCartsViewModel>>(shoppingCarts);
            _logger.LogInformation($"Returned Paged List of ShoppingCarts from database.");
            return new PagedListResponse<ShoppingCartsViewModel>(shoppingCartsViewModel, shoppingCarts.MetaData);
        }
    }
}
