﻿using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using FluentValidation;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.ShoppingCartItems.Commands.Update
{
    public class UpdateShoppingCartItemCommandValidator : AbstractValidator<UpdateShoppingCartItemCommand>
    {
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;

        public UpdateShoppingCartItemCommandValidator(IRepositoryWrapper repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;

            RuleFor(p => p.ItemId)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull()
                .Must(BeAValidGuid).WithMessage("{PropertyName} is required.");
            
            RuleFor(p => p.ShoppingCartId)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull()
                .Must(BeAValidGuid).WithMessage("{PropertyName} is required.");

            RuleFor(p => p)
                .MustAsync(IsUnique).WithMessage("{PropertyName} already exists.");
        }

        private bool BeAValidGuid(Guid id)
        {
            return !id.Equals(new Guid());
        }

        private async Task<bool> IsUnique(UpdateShoppingCartItemCommand shoppingCartItemCommand, CancellationToken cancellationToken)
        {
            var shoppingCartItem = _mapper.Map<ShoppingCartItem>(shoppingCartItemCommand);
            return await _repository.ShoppingCartItem.ExistAsync(shoppingCartItem);
        }
    }
}
