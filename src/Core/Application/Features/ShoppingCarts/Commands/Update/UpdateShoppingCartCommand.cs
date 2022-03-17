using Application.Exceptions;
using Application.Features.ShoppingCarts.Queries.GetById;
using Application.Interfaces;
using Application.Wrappers;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.ShoppingCarts.Commands.Update
{
    public class UpdateShoppingCartCommand : IRequest<ShoppingCartViewModel>
    {
        public Guid Id { get; set; }
        public DateTime OrderAt { get; set; }
        public int Total { get; set; }
        public int DeliveryFees { get; set; }
        public Guid CustomerId { get; set; }
    }

    public class UpdateShoppingCartCommandHandler : IRequestHandler<UpdateShoppingCartCommand, ShoppingCartViewModel>
    {
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;

        public UpdateShoppingCartCommandHandler(IRepositoryWrapper repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ShoppingCartViewModel> Handle(UpdateShoppingCartCommand command, CancellationToken cancellationToken)
        {
            var shoppingCartEntity = await _repository.ShoppingCart.GetByIdAsync(command.Id);
            if (shoppingCartEntity == null) throw new ApiException($"ShoppingCart with id: {command.Id}, hasn't been found.");
            
            _mapper.Map(command, shoppingCartEntity);
            await _repository.ShoppingCart.UpdateAsync(shoppingCartEntity);
            await _repository.SaveAsync();

            var shoppingCartReadDto = _mapper.Map<ShoppingCartViewModel>(shoppingCartEntity);
            //if (!string.IsNullOrWhiteSpace(shoppingCartReadDto.ImgLink)) shoppingCartReadDto.ImgLink = $"{_baseURL}{shoppingCartReadDto.ImgLink}";
            return shoppingCartReadDto;
        }
    }
}
