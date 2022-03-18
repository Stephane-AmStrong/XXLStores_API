using Application.Exceptions;
using Application.Interfaces;
using Application.Wrappers;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Categories.Commands.Delete
{
    public class DeleteCategoryByIdCommand : IRequest
    {
        public Guid Id { get; set; }
    }

    internal class DeleteCategoryByIdCommandHandler : IRequestHandler<DeleteCategoryByIdCommand>
    {
        private readonly ILogger<DeleteCategoryByIdCommandHandler> _logger;
        private readonly IRepositoryWrapper _repository;

        public DeleteCategoryByIdCommandHandler(IRepositoryWrapper repository, ILogger<DeleteCategoryByIdCommandHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<Unit> Handle(DeleteCategoryByIdCommand command, CancellationToken cancellationToken)
        {
            var product = await _repository.Category.GetByIdAsync(command.Id);
            if (product == null) throw new ApiException($"Category with id: {command.Id}, hasn't been found.");
            await _repository.Category.DeleteAsync(product);
            await _repository.SaveAsync();

            return Unit.Value;
        }
    }
}
