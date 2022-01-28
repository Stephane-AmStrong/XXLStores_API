using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using FluentValidation;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.ShoppingCarts.Commands.UpdateShoppingCart
{
    public class UpdateShoppingCartCommandValidator : AbstractValidator<UpdateShoppingCartCommand>
    {
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;

        public UpdateShoppingCartCommandValidator(IRepositoryWrapper repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;

            RuleFor(p => p.Name)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull()
                .MaximumLength(50).WithMessage("{PropertyName} must not exceed 50 characters.");

            RuleFor(p => p.CategoryId)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull()
                .Must(BeAValidGuid).WithMessage("{PropertyName} is required.");
            
            RuleFor(p => p.ShopId)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull()
                .Must(BeAValidGuid).WithMessage("{PropertyName} is required.");

            RuleFor(p => p)
                .MustAsync(IsUnique).WithMessage("{PropertyName} already exists.");
        }

        private bool BeAValidDate(DateTime date)
        {
            return !date.Equals(default(DateTime)) && date < DateTime.Now;
        }

        private bool BeAValidGuid(Guid id)
        {
            return !id.Equals(new Guid());
        }

        private async Task<bool> IsUnique(UpdateShoppingCartCommand shoppingCartCommand, CancellationToken cancellationToken)
        {
            var shoppingCart = _mapper.Map<ShoppingCart>(shoppingCartCommand);
            return await _repository.ShoppingCart.ExistAsync(shoppingCart);
        }
    }
}
