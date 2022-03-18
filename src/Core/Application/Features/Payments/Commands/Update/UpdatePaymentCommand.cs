using Application.Exceptions;
using Application.Features.Payments.Queries.GetById;
using Application.Interfaces;
using Application.Wrappers;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Payments.Commands.Update
{
    public class UpdatePaymentCommand : IRequest<PaymentViewModel>
    {
        public Guid Id { get; set; }
        public int MoneyAmount { get; set; }
        public DateTime? PaidAt { get; set; }
        public Guid ShoppingCartId { get; set; }
    }

    internal class UpdatePaymentCommandHandler : IRequestHandler<UpdatePaymentCommand, PaymentViewModel>
    {
        private readonly ILogger<UpdatePaymentCommandHandler> _logger;
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;

        public UpdatePaymentCommandHandler(IRepositoryWrapper repository, IMapper mapper, ILogger<UpdatePaymentCommandHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<PaymentViewModel> Handle(UpdatePaymentCommand command, CancellationToken cancellationToken)
        {
            var paymentEntity = await _repository.Payment.GetByIdAsync(command.Id);
            if (paymentEntity == null) throw new ApiException($"Payment with id: {command.Id}, hasn't been found.");

            _mapper.Map(command, paymentEntity);
            await _repository.Payment.UpdateAsync(paymentEntity);
            await _repository.SaveAsync();

            var paymentReadDto = _mapper.Map<PaymentViewModel>(paymentEntity);

            //if (!string.IsNullOrWhiteSpace(paymentReadDto.ImgLink)) paymentReadDto.ImgLink = $"{_baseURL}{paymentReadDto.ImgLink}";

            return paymentReadDto;


        }
    }
}
