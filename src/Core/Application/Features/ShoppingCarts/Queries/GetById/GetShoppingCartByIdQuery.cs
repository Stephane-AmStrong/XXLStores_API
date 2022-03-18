using Application.Exceptions;
using Application.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.ShoppingCarts.Queries.GetById
{
    public class GetShoppingCartByIdQuery : IRequest<ShoppingCartViewModel>
    {
        public Guid Id { get; set; }
    }

    internal class GetShoppingCartByIdQueryHandler : IRequestHandler<GetShoppingCartByIdQuery, ShoppingCartViewModel>
    {
        private readonly ILogger<GetShoppingCartByIdQueryHandler> _logger;
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;

        public GetShoppingCartByIdQueryHandler(IRepositoryWrapper repository, IMapper mapper, ILogger<GetShoppingCartByIdQueryHandler> logger)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<ShoppingCartViewModel> Handle(GetShoppingCartByIdQuery query, CancellationToken cancellationToken)
        {
            var shoppingCart = await _repository.ShoppingCart.GetByIdAsync(query.Id);
            if (shoppingCart == null) throw new ApiException($"ShoppingCart with id: {query.Id}, hasn't been found.");

            _logger.LogInformation($"Returned ShoppingCart with id: {query.Id}");
            return _mapper.Map<ShoppingCartViewModel>(shoppingCart);
        }
    }
}
