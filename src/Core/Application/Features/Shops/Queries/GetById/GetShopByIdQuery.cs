using Application.Exceptions;
using Application.Interfaces;
using AutoMapper;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Shops.Queries.GetById
{
    public class GetShopByIdQuery : IRequest<ShopViewModel>
    {
        public Guid Id { get; set; }

        public class GetShopByIdQueryHandler : IRequestHandler<GetShopByIdQuery, ShopViewModel>
        {
            private readonly IRepositoryWrapper _repository;
            private readonly IMapper _mapper;

            public GetShopByIdQueryHandler(IRepositoryWrapper repository, IMapper mapper)
            {
                _repository = repository;
                _mapper = mapper;
            }

            public async Task<ShopViewModel> Handle(GetShopByIdQuery query, CancellationToken cancellationToken)
            {
                var shop = await _repository.Shop.GetByIdAsync(query.Id);
                if (shop == null) throw new ApiException($"Shop Not Found.");
                return _mapper.Map<ShopViewModel>(shop);
            }
        }
    }
}
