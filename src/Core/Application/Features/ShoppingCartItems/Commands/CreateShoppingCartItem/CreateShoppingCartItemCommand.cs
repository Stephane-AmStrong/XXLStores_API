using Application.Features.ShoppingCartItems.Queries.GetShoppingCartItemById;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using MediatR;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.ShoppingCartItems.Commands.CreateShoppingCartItem
{
    public class CreateShoppingCartItemCommand : IRequest<ShoppingCartItemViewModel>
    {
        public string Name { get; set; }
        public Guid CategoryId { get; set; }
        public Guid ShopId { get; set; }
    }

    public class CreateShoppingCartItemCommandHandler : IRequestHandler<CreateShoppingCartItemCommand, ShoppingCartItemViewModel>
    {
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;


        public CreateShoppingCartItemCommandHandler(IRepositoryWrapper repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
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
