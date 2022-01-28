using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using FluentValidation;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Payments.Commands.CreatePayment
{
    public class CreatePaymentCommandValidator : AbstractValidator<CreatePaymentCommand>
    {
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;

        public CreatePaymentCommandValidator(IRepositoryWrapper repository, IMapper mapper)
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

        private bool BeAValidGuid(Guid id)
        {
            return !id.Equals(new Guid());
        }

        private async Task<bool> IsUnique(CreatePaymentCommand paymentCommand, CancellationToken cancellationToken)
        {
            var payment = _mapper.Map<Payment>(paymentCommand);
            return await _repository.Payment.ExistAsync(payment);
        }
    }
}
