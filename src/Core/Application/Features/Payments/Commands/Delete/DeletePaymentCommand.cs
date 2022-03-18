using Application.Exceptions;
using Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Payments.Commands.Delete
{
    public class DeletePaymentCommand : IRequest
    {
        public Guid Id { get; set; }
    }

    internal class DeletePaymentByIdCommandHandler : IRequestHandler<DeletePaymentCommand>
    {
        private readonly ILogger<DeletePaymentByIdCommandHandler> _logger;
        private readonly IRepositoryWrapper _repository;

        public DeletePaymentByIdCommandHandler(IRepositoryWrapper repository, ILogger<DeletePaymentByIdCommandHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<Unit> Handle(DeletePaymentCommand command, CancellationToken cancellationToken)
        {
            var payment = await _repository.Payment.GetByIdAsync(command.Id);
            if (payment == null) throw new ApiException($"Payment with id: {command.Id}, hasn't been found.");
            await _repository.Payment.DeleteAsync(payment);
            await _repository.SaveAsync();

            return Unit.Value;
        }
    }
}
