using Application.Exceptions;
using Application.Interfaces;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Items.Commands.Delete
{
    public class DeleteItemCommand : IRequest
    {
        public Guid Id { get; set; }

        public class DeleteItemByIdCommandHandler : IRequestHandler<DeleteItemCommand>
        {
            private readonly IRepositoryWrapper _repository;

            public DeleteItemByIdCommandHandler(IRepositoryWrapper repository)
            {
                _repository = repository;
            }

            public async Task<Unit> Handle(DeleteItemCommand command, CancellationToken cancellationToken)
            {
                var item = await _repository.Item.GetByIdAsync(command.Id);
                if (item == null) throw new ApiException($"Item Not Found.");
                await _repository.Item.DeleteAsync(item);
                await _repository.SaveAsync();

                return Unit.Value;
            }
        }
    }
}
