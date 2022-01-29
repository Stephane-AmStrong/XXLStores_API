using Application.Features.Payments.Queries.GetById;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Payments.Commands.Create
{
    public class CreatePaymentCommand : IRequest<PaymentViewModel>
    {
        public int MoneyAmount { get; set; }
        public DateTime? PaidAt { get; set; }
        public Guid ShoppingCartId { get; set; }
    }

    public class CreatePaymentCommandHandler : IRequestHandler<CreatePaymentCommand, PaymentViewModel>
    {
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;


        public CreatePaymentCommandHandler(IRepositoryWrapper repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }


        public async Task<PaymentViewModel> Handle(CreatePaymentCommand command, CancellationToken cancellationToken)
        {
            var paymentEntity = _mapper.Map<Payment>(command);

            await _repository.Payment.CreateAsync(paymentEntity);
            await _repository.SaveAsync();

            return _mapper.Map<PaymentViewModel>(paymentEntity);
        }
    }
}
