using Application.Interfaces;
using Application.Wrappers;
using AutoMapper;
using Domain.Parameters;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.InventoryLevels.Queries.GetPagedList
{
    public class GetInventoryLevelsQuery : QueryParameters, IRequest<PagedListResponse<InventoryLevelsViewModel>>
    {
        public GetInventoryLevelsQuery()
        {
            OrderBy = "name";
        }

        public string WithTheName { get; set; }
    }

    internal class GetInventoryLevelsQueryHandler : IRequestHandler<GetInventoryLevelsQuery, PagedListResponse<InventoryLevelsViewModel>>
    {
        private readonly ILogger<GetInventoryLevelsQueryHandler> _logger;
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;

        public GetInventoryLevelsQueryHandler(IRepositoryWrapper repository, IMapper mapper, ILogger<GetInventoryLevelsQueryHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<PagedListResponse<InventoryLevelsViewModel>> Handle(GetInventoryLevelsQuery query, CancellationToken cancellationToken)
        {
            var inventoryLevels = await _repository.InventoryLevel.GetPagedListAsync(query);
            var inventoryLevelsViewModel = _mapper.Map<List<InventoryLevelsViewModel>>(inventoryLevels);
            _logger.LogInformation($"Returned Paged List of InventoryLevels from database.");
            return new PagedListResponse<InventoryLevelsViewModel>(inventoryLevelsViewModel, inventoryLevels.MetaData);
        }
    }
}
