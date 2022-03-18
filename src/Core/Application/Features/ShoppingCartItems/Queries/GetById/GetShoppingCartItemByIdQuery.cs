using Application.Exceptions;
using Application.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.ShoppingCartItems.Queries.GetById
{
    public class GetShoppingCartItemByIdQuery : IRequest<ShoppingCartItemViewModel>
    {
        public Guid Id { get; set; }
    }

    internal class GetShoppingCartItemByIdQueryHandler : IRequestHandler<GetShoppingCartItemByIdQuery, ShoppingCartItemViewModel>
    {
        private readonly ILogger<GetShoppingCartItemByIdQueryHandler> _logger;
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;

        public GetShoppingCartItemByIdQueryHandler(IRepositoryWrapper repository, IMapper mapper, ILogger<GetShoppingCartItemByIdQueryHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ShoppingCartItemViewModel> Handle(GetShoppingCartItemByIdQuery query, CancellationToken cancellationToken)
        {
            var shoppingCartItem = await _repository.ShoppingCartItem.GetByIdAsync(query.Id);
            if (shoppingCartItem == null) throw new ApiException($"ShoppingCartItem with id: {query.Id}, hasn't been found.");

            _logger.LogInformation($"Returned ShoppingCartItem with id: {query.Id}");
            return _mapper.Map<ShoppingCartItemViewModel>(shoppingCartItem);
        }
    }
}
