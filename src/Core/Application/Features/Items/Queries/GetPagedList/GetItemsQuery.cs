using Application.Interfaces;
using Application.Wrappers;
using AutoMapper;
using Domain.Parameters;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Items.Queries.GetPagedList
{
    public class GetItemsQuery : QueryParameters, IRequest<PagedListResponse<ItemsViewModel>>
    {
        public GetItemsQuery()
        {
            OrderBy = "name";
        }

        public string WithTheName { get; set; }
    }

    internal class GetItemsQueryHandler : IRequestHandler<GetItemsQuery, PagedListResponse<ItemsViewModel>>
    {
        private readonly ILogger<GetItemsQueryHandler> _logger;
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;

        public GetItemsQueryHandler(IRepositoryWrapper repository, IMapper mapper, ILogger<GetItemsQueryHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<PagedListResponse<ItemsViewModel>> Handle(GetItemsQuery query, CancellationToken cancellationToken)
        {
            var items = await _repository.Item.GetPagedListAsync(query);
            var itemsViewModel = _mapper.Map<List<ItemsViewModel>>(items);
            _logger.LogInformation($"Returned Paged List of Items from database.");
            return new PagedListResponse<ItemsViewModel>(itemsViewModel, items.MetaData);
        }
    }
}
