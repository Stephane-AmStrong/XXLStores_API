using Application.Exceptions;
using Application.Features.ShoppingCartItems.Queries.GetShoppingCartItemById;
using Application.Interfaces;
using Application.Wrappers;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.ShoppingCartItems.Commands.Update
{
    public class UpdateShoppingCartItemCommand : IRequest<ShoppingCartItemViewModel>
    {
        public Guid Id { get; set; }
        public Guid ShoppingCartId { get; set; }
        public int Quantity { get; set; }
        public int Total { get; set; }
        public Guid ItemId { get; set; }
    }

    public class UpdateShoppingCartItemCommandHandler : IRequestHandler<UpdateShoppingCartItemCommand, ShoppingCartItemViewModel>
    {
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;

        public UpdateShoppingCartItemCommandHandler(IRepositoryWrapper repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ShoppingCartItemViewModel> Handle(UpdateShoppingCartItemCommand command, CancellationToken cancellationToken)
        {
            var shoppingCartItemEntity = await _repository.ShoppingCartItem.GetByIdAsync(command.Id);

            if (shoppingCartItemEntity == null)
            {
                throw new ApiException($"ShoppingCartItem Not Found.");
            }

            _mapper.Map(command, shoppingCartItemEntity);

            await _repository.ShoppingCartItem.UpdateAsync(shoppingCartItemEntity);
            await _repository.SaveAsync();

            var shoppingCartItemReadDto = _mapper.Map<ShoppingCartItemViewModel>(shoppingCartItemEntity);

            //if (!string.IsNullOrWhiteSpace(shoppingCartItemReadDto.ImgLink)) shoppingCartItemReadDto.ImgLink = $"{_baseURL}{shoppingCartItemReadDto.ImgLink}";

            return shoppingCartItemReadDto;


        }
    }
}
