using Application.Exceptions;
using Application.Interfaces;
using AutoMapper;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Items.Queries.GetById
{
    public class GetItemByIdQuery : IRequest<ItemViewModel>
    {
        public Guid Id { get; set; }

        public class GetItemByIdQueryHandler : IRequestHandler<GetItemByIdQuery, ItemViewModel>
        {
            private readonly IRepositoryWrapper _repository;
            private readonly IMapper _mapper;

            public GetItemByIdQueryHandler(IRepositoryWrapper repository, IMapper mapper)
            {
                _repository = repository;
                _mapper = mapper;
            }

            public async Task<ItemViewModel> Handle(GetItemByIdQuery query, CancellationToken cancellationToken)
            {
                var item = await _repository.Item.GetByIdAsync(query.Id);
                if (item == null) throw new ApiException($"Item Not Found.");
                return _mapper.Map<ItemViewModel>(item);
            }
        }
    }
}
