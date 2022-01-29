using Application.Features.Items.Queries.GetById;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Items.Commands.Create
{
    public class CreateItemCommand : IRequest<ItemViewModel>
    {
        public string Name { get; set; }
        public Guid CategoryId { get; set; }
        public Guid ShopId { get; set; }
    }

    public class CreateItemCommandHandler : IRequestHandler<CreateItemCommand, ItemViewModel>
    {
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;


        public CreateItemCommandHandler(IRepositoryWrapper repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }


        public async Task<ItemViewModel> Handle(CreateItemCommand command, CancellationToken cancellationToken)
        {
            var itemEntity = _mapper.Map<Item>(command);

            await _repository.Item.CreateAsync(itemEntity);
            await _repository.SaveAsync();

            return _mapper.Map<ItemViewModel>(itemEntity);
        }
    }
}
