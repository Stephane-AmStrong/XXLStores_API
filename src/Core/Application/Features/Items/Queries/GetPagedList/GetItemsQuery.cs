using Application.Interfaces;
using Application.Wrappers;
using AutoMapper;
using Domain.Parameters;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Items.Queries.GetPagedList
{
    public class GetItemsQuery : QueryParameters, IRequest<PagedList<ItemsViewModel>>
    {
        public GetItemsQuery()
        {
            OrderBy = "name";
        }

        public string WithTheName { get; set; }
    }

    internal class GetItemsQueryHandler : IRequestHandler<GetItemsQuery, PagedList<ItemsViewModel>>
    {
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;

        public GetItemsQueryHandler(IRepositoryWrapper repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }


        public async Task<PagedList<ItemsViewModel>> Handle(GetItemsQuery query, CancellationToken cancellationToken)
        {
            var items = await _repository.Item.GetPagedListAsync(query);
            return  _mapper.Map<PagedList<ItemsViewModel>>(items);
        }
    }
}
