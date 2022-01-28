using Application.Interfaces;
using Application.Parameters;
using Application.Wrappers;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Items.Queries.GetItems
{
    public class GetItemsQuery : QueryStringParameters, IRequest<PagedList<ItemsViewModel>>
    {
        public GetItemsQuery()
        {
            OrderBy = "name";
        }

        public string WithTheName { get; set; }
    }

    public class GetAllItemsQueryHandler : IRequestHandler<GetItemsQuery, PagedList<ItemsViewModel>>
    {
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;

        public GetAllItemsQueryHandler(IRepositoryWrapper repository, IMapper mapper)
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
