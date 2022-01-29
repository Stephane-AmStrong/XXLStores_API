using Application.Features.ShoppingCarts.Queries.GetShoppingCartById;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using MediatR;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.ShoppingCarts.Commands.Create
{
    public class CreateShoppingCartCommand : IRequest<ShoppingCartViewModel>
    {
        public DateTime Date { get; set; }
        public int Total { get; set; }
        public int DeliveryFees { get; set; }
        public Guid CustomerId { get; set; }
    }

    public class CreateShoppingCartCommandHandler : IRequestHandler<CreateShoppingCartCommand, ShoppingCartViewModel>
    {
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;


        public CreateShoppingCartCommandHandler(IRepositoryWrapper repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }


        public async Task<ShoppingCartViewModel> Handle(CreateShoppingCartCommand command, CancellationToken cancellationToken)
        {
            var shoppingCartEntity = _mapper.Map<ShoppingCart>(command);

            await _repository.ShoppingCart.CreateAsync(shoppingCartEntity);
            await _repository.SaveAsync();

            return _mapper.Map<ShoppingCartViewModel>(shoppingCartEntity);
        }
    }
}
