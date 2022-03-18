using Application.Exceptions;
using Application.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Payments.Queries.GetById
{
    public class GetPaymentByIdQuery : IRequest<PaymentViewModel>
    {
        public Guid Id { get; set; }
    }

    internal class GetPaymentByIdQueryHandler : IRequestHandler<GetPaymentByIdQuery, PaymentViewModel>
    {
        private readonly ILogger<GetPaymentByIdQueryHandler> _logger;
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;

        public GetPaymentByIdQueryHandler(IRepositoryWrapper repository, IMapper mapper, ILogger<GetPaymentByIdQueryHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<PaymentViewModel> Handle(GetPaymentByIdQuery query, CancellationToken cancellationToken)
        {
            var payment = await _repository.Payment.GetByIdAsync(query.Id);
            if (payment == null) throw new ApiException($"Payment with id: {query.Id}, hasn't been found.");

            _logger.LogInformation($"Returned Payment with id: {query.Id}");
            return _mapper.Map<PaymentViewModel>(payment);
        }
    }
}
