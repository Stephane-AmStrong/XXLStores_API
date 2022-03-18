using Application.Features.ShoppingCarts.Queries.GetById;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.ShoppingCarts.Commands.Create
{
    public class CreateShoppingCartCommand : IRequest<ShoppingCartViewModel>
    {
        public DateTime OrderAt { get; set; }
        public int Total { get; set; }
        public int DeliveryFees { get; set; }
        public Guid CustomerId { get; set; }
    }

    internal class CreateShoppingCartCommandHandler : IRequestHandler<CreateShoppingCartCommand, ShoppingCartViewModel>
    {
        private readonly ILogger<CreateShoppingCartCommandHandler> _logger;
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;


        public CreateShoppingCartCommandHandler(IRepositoryWrapper repository, IMapper mapper, ILogger<CreateShoppingCartCommandHandler> logger)
        {
            _repository = repository;
            _logger = logger;
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
