using Application.Interfaces;
using Application.Wrappers;
using AutoMapper;
using Domain.Parameters;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.InventoryLevels.Queries.GetPagedList
{
    public class GetInventoryLevelsQuery : QueryParameters, IRequest<PagedList<InventoryLevelsViewModel>>
    {
        public GetInventoryLevelsQuery()
        {
            OrderBy = "name";
        }

        public string WithTheName { get; set; }
    }

    internal class GetInventoryLevelsQueryHandler : IRequestHandler<GetInventoryLevelsQuery, PagedList<InventoryLevelsViewModel>>
    {
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;

        public GetInventoryLevelsQueryHandler(IRepositoryWrapper repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }


        public async Task<PagedList<InventoryLevelsViewModel>> Handle(GetInventoryLevelsQuery query, CancellationToken cancellationToken)
        {
            var inventoryLevels = await _repository.InventoryLevel.GetPagedListAsync(query);
            return  _mapper.Map<PagedList<InventoryLevelsViewModel>>(inventoryLevels);
        }
    }
}
