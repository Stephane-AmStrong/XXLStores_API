using Application.Interfaces;
using Application.Wrappers;
using AutoMapper;
using Domain.Parameters;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Shops.Queries.GetPagedList
{
    public class GetShopsQuery : QueryParameters, IRequest<PagedListResponse<ShopsViewModel>>
    {
        public GetShopsQuery()
        {
            OrderBy = "name";
        }

        public string WithTheName { get; set; }
    }

    internal class GetShopsQueryHandler : IRequestHandler<GetShopsQuery, PagedListResponse<ShopsViewModel>>
    {
        private readonly ILogger<GetShopsQueryHandler> _logger;
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;

        public GetShopsQueryHandler(IRepositoryWrapper repository, IMapper mapper, ILogger<GetShopsQueryHandler> logger)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<PagedListResponse<ShopsViewModel>> Handle(GetShopsQuery query, CancellationToken cancellationToken)
        {
            var shops = await _repository.Shop.GetPagedListAsync(query);
            var lstShopsViewModel = _mapper.Map<List<ShopsViewModel>>(shops);
            _logger.LogInformation($"Returned Paged List of Shops from database.");
            return new PagedListResponse<ShopsViewModel>(lstShopsViewModel, shops.MetaData);
        }
    }
}
