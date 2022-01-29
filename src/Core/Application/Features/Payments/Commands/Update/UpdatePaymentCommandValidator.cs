using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using FluentValidation;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Payments.Commands.Update
{
    public class UpdatePaymentCommandValidator : AbstractValidator<UpdatePaymentCommand>
    {
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;

        public UpdatePaymentCommandValidator(IRepositoryWrapper repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;

            RuleFor(p => p.MoneyAmount)
                .NotEmpty().WithMessage("{PropertyName} is required.");

            RuleFor(p => p)
                .MustAsync(BeSufficient).WithMessage("{PropertyName} is insufficient.");

            RuleFor(p => p.PaidAt)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull()
                .Must(BeAValidDate).WithMessage("{PropertyName} is required.");

            RuleFor(p => p.ShoppingCartId)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull()
                .Must(BeAValidGuid).WithMessage("{PropertyName} is required.");

            RuleFor(p => p)
                .MustAsync(IsUnique).WithMessage("{PropertyName} already exists.");
        }

        private async Task<bool> BeSufficient(UpdatePaymentCommand paymentCommand, CancellationToken cancellationToken)
        {
            var shoppingAmount = (await _repository.ShoppingCart.GetByIdAsync(paymentCommand.ShoppingCartId)).ShoppingCartItems.Sum(x => x.Quantity * x.UnitPrice);

            return paymentCommand.MoneyAmount >= shoppingAmount;
        }

        private bool BeAValidGuid(Guid id)
        {
            return !id.Equals(new Guid());
        }

        private bool BeAValidDate(DateTime? date)
        {
            return !date.Equals(default(DateTime)) && date < DateTime.Now;
        }

        private async Task<bool> IsUnique(UpdatePaymentCommand paymentCommand, CancellationToken cancellationToken)
        {
            var payment = _mapper.Map<Payment>(paymentCommand);
            return await _repository.Payment.ExistAsync(payment);
        }
    }
}
