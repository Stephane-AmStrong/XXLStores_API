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

namespace Application.Features.ShoppingCartItems.Queries.GetPagedList
{
    public class GetShoppingCartItemsQuery : QueryParameters, IRequest<PagedListResponse<ShoppingCartItemsViewModel>>
    {
        public GetShoppingCartItemsQuery()
        {
            OrderBy = "name";
        }

        public string WithTheName { get; set; }
    }

    internal class GetShoppingCartItemsQueryHandler : IRequestHandler<GetShoppingCartItemsQuery, PagedListResponse<ShoppingCartItemsViewModel>>
    {
        private readonly ILogger<GetShoppingCartItemsQueryHandler> _logger;
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;

        public GetShoppingCartItemsQueryHandler(IRepositoryWrapper repository, IMapper mapper, ILogger<GetShoppingCartItemsQueryHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<PagedListResponse<ShoppingCartItemsViewModel>> Handle(GetShoppingCartItemsQuery query, CancellationToken cancellationToken)
        {
            var shoppingCartItems = await _repository.ShoppingCartItem.GetPagedListAsync(query);
            var shoppingCartItemsViewModel = _mapper.Map<List<ShoppingCartItemsViewModel>>(shoppingCartItems);
            _logger.LogInformation($"Returned Paged List of ShoppingCartItems from database.");
            return new PagedListResponse<ShoppingCartItemsViewModel>(shoppingCartItemsViewModel, shoppingCartItems.MetaData);
        }
    }
}
