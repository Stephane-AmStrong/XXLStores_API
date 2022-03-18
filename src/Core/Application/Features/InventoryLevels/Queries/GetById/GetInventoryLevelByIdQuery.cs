using Application.Exceptions;
using Application.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.InventoryLevels.Queries.GetById
{
    public class GetInventoryLevelByIdQuery : IRequest<InventoryLevelViewModel>
    {
        public Guid Id { get; set; }
    }

    internal class GetInventoryLevelByIdQueryHandler : IRequestHandler<GetInventoryLevelByIdQuery, InventoryLevelViewModel>
    {
        private readonly ILogger<GetInventoryLevelByIdQueryHandler> _logger;
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;

        public GetInventoryLevelByIdQueryHandler(IRepositoryWrapper repository, IMapper mapper, ILogger<GetInventoryLevelByIdQueryHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<InventoryLevelViewModel> Handle(GetInventoryLevelByIdQuery query, CancellationToken cancellationToken)
        {
            var inventoryLevel = await _repository.InventoryLevel.GetByIdAsync(query.Id);
            if (inventoryLevel == null) throw new ApiException($"InventoryLevel with id: {query.Id}, hasn't been found.");

            _logger.LogInformation($"Returned InventoryLevel with id: {query.Id}");
            return _mapper.Map<InventoryLevelViewModel>(inventoryLevel);
        }
    }
}
