using Application.Exceptions;
using Application.Interfaces;
using AutoMapper;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Payments.Queries.GetById
{
    public class GetPaymentByIdQuery : IRequest<PaymentViewModel>
    {
        public Guid Id { get; set; }

        public class GetPaymentByIdQueryHandler : IRequestHandler<GetPaymentByIdQuery, PaymentViewModel>
        {
            private readonly IRepositoryWrapper _repository;
            private readonly IMapper _mapper;

            public GetPaymentByIdQueryHandler(IRepositoryWrapper repository, IMapper mapper)
            {
                _repository = repository;
                _mapper = mapper;
            }

            public async Task<PaymentViewModel> Handle(GetPaymentByIdQuery query, CancellationToken cancellationToken)
            {
                var payment = await _repository.Payment.GetByIdAsync(query.Id);
                if (payment == null) throw new ApiException($"Payment Not Found.");
                return _mapper.Map<PaymentViewModel>(payment);
            }
        }
    }
}
