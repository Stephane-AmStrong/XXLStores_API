﻿using Application.Exceptions;
using Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Items.Commands.Delete
{
    public class DeleteItemCommand : IRequest
    {
        public Guid Id { get; set; }
    }

    internal class DeleteItemByIdCommandHandler : IRequestHandler<DeleteItemCommand>
    {
        private readonly ILogger<DeleteItemByIdCommandHandler> _logger;
        private readonly IRepositoryWrapper _repository;

        public DeleteItemByIdCommandHandler(IRepositoryWrapper repository, ILogger<DeleteItemByIdCommandHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<Unit> Handle(DeleteItemCommand command, CancellationToken cancellationToken)
        {
            var item = await _repository.Item.GetByIdAsync(command.Id);
            if (item == null) throw new ApiException($"Item with id: {command.Id}, hasn't been found.");
            await _repository.Item.DeleteAsync(item);
            await _repository.SaveAsync();

            return Unit.Value;
        }
    }
}
