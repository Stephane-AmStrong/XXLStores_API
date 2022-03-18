using Application.Exceptions;
using Application.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Shops.Queries.GetById
{
    public class GetShopByIdQuery : IRequest<ShopViewModel>
    {
        public Guid Id { get; set; }
    }

    internal class GetShopByIdQueryHandler : IRequestHandler<GetShopByIdQuery, ShopViewModel>
    {
        private readonly ILogger<GetShopByIdQueryHandler> _logger;
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;

        public GetShopByIdQueryHandler(IRepositoryWrapper repository, IMapper mapper, ILogger<GetShopByIdQueryHandler> logger)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<ShopViewModel> Handle(GetShopByIdQuery query, CancellationToken cancellationToken)
        {
            var shop = await _repository.Shop.GetByIdAsync(query.Id);
            if (shop == null) throw new ApiException($"Shop with id: {query.Id}, hasn't been found.");

            _logger.LogInformation($"Returned Shop with id: {query.Id}");
            return _mapper.Map<ShopViewModel>(shop);
        }
    }
}
