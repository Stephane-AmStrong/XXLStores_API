using Application.Exceptions;
using Application.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Items.Queries.GetById
{
    public class GetItemByIdQuery : IRequest<ItemViewModel>
    {
        public Guid Id { get; set; }
    }

    internal class GetItemByIdQueryHandler : IRequestHandler<GetItemByIdQuery, ItemViewModel>
    {
        private readonly ILogger<GetItemByIdQueryHandler> _logger;
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;

        public GetItemByIdQueryHandler(IRepositoryWrapper repository, IMapper mapper, ILogger<GetItemByIdQueryHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ItemViewModel> Handle(GetItemByIdQuery query, CancellationToken cancellationToken)
        {
            var item = await _repository.Item.GetByIdAsync(query.Id);
            if (item == null) throw new ApiException($"Item with id: {query.Id}, hasn't been found.");

            _logger.LogInformation($"Returned Item with id: {query.Id}");
            return _mapper.Map<ItemViewModel>(item);
        }
    }
}
