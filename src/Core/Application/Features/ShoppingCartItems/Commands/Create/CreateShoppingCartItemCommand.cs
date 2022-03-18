using Application.Features.ShoppingCartItems.Queries.GetById;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.ShoppingCartItems.Commands.Create
{
    public class CreateShoppingCartItemCommand : IRequest<ShoppingCartItemViewModel>
    {
        public Guid ShoppingCartId { get; set; }
        public int Quantity { get; set; }
        public int Total { get; set; }
        public Guid ItemId { get; set; }
    }

    internal class CreateShoppingCartItemCommandHandler : IRequestHandler<CreateShoppingCartItemCommand, ShoppingCartItemViewModel>
    {
        private readonly ILogger<CreateShoppingCartItemCommandHandler> _logger;
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;


        public CreateShoppingCartItemCommandHandler(IRepositoryWrapper repository, IMapper mapper, ILogger<CreateShoppingCartItemCommandHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }


        public async Task<ShoppingCartItemViewModel> Handle(CreateShoppingCartItemCommand command, CancellationToken cancellationToken)
        {
            var shoppingCartItemEntity = _mapper.Map<ShoppingCartItem>(command);

            await _repository.ShoppingCartItem.CreateAsync(shoppingCartItemEntity);
            await _repository.SaveAsync();

            return _mapper.Map<ShoppingCartItemViewModel>(shoppingCartItemEntity);
        }
    }
}
