using Application.Exceptions;
using Application.Interfaces;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Payments.Commands.DeletePaymentById
{
    public class DeletePaymentByIdCommand : IRequest
    {
        public Guid Id { get; set; }

        public class DeletePaymentByIdCommandHandler : IRequestHandler<DeletePaymentByIdCommand>
        {
            private readonly IRepositoryWrapper _repository;

            public DeletePaymentByIdCommandHandler(IRepositoryWrapper repository)
            {
                _repository = repository;
            }

            public async Task<Unit> Handle(DeletePaymentByIdCommand command, CancellationToken cancellationToken)
            {
                var payment = await _repository.Payment.GetByIdAsync(command.Id);
                if (payment == null) throw new ApiException($"Payment Not Found.");
                await _repository.Payment.DeleteAsync(payment);
                await _repository.SaveAsync();

                return Unit.Value;
            }
        }
    }
}
