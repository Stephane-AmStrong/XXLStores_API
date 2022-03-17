using Application.Exceptions;
using Application.Interfaces;
using AutoMapper;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.InventoryLevels.Queries.GetById
{
    public class GetInventoryLevelByIdQuery : IRequest<InventoryLevelViewModel>
    {
        public Guid Id { get; set; }

        public class GetInventoryLevelByIdQueryHandler : IRequestHandler<GetInventoryLevelByIdQuery, InventoryLevelViewModel>
        {
            private readonly IRepositoryWrapper _repository;
            private readonly IMapper _mapper;

            public GetInventoryLevelByIdQueryHandler(IRepositoryWrapper repository, IMapper mapper)
            {
                _repository = repository;
                _mapper = mapper;
            }

            public async Task<InventoryLevelViewModel> Handle(GetInventoryLevelByIdQuery query, CancellationToken cancellationToken)
            {
                var inventoryLevel = await _repository.InventoryLevel.GetByIdAsync(query.Id);
                if (inventoryLevel == null) throw new ApiException($"InventoryLevel with id: {query.Id}, hasn't been found.");
                return _mapper.Map<InventoryLevelViewModel>(inventoryLevel);
            }
        }
    }
}
