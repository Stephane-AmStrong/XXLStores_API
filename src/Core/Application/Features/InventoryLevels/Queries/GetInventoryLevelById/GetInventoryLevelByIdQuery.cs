using Application.Exceptions;
using Application.Interfaces;
using AutoMapper;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.InventoryLevels.Queries.GetInventoryLevelById
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
                if (inventoryLevel == null) throw new ApiException($"InventoryLevel Not Found.");
                return _mapper.Map<InventoryLevelViewModel>(inventoryLevel);
            }
        }
    }
}
