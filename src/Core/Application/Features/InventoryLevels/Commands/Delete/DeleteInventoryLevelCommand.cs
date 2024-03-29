﻿using Application.Exceptions;
using Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.InventoryLevels.Commands.Delete
{
    public class DeleteInventoryLevelCommand : IRequest
    {
        public Guid Id { get; set; }
    }

    internal class DeleteInventoryLevelByIdCommandHandler : IRequestHandler<DeleteInventoryLevelCommand>
    {
        private readonly ILogger<DeleteInventoryLevelByIdCommandHandler> _logger;
        private readonly IRepositoryWrapper _repository;

        public DeleteInventoryLevelByIdCommandHandler(IRepositoryWrapper repository, ILogger<DeleteInventoryLevelByIdCommandHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<Unit> Handle(DeleteInventoryLevelCommand command, CancellationToken cancellationToken)
        {
            var product = await _repository.InventoryLevel.GetByIdAsync(command.Id);
            if (product == null) throw new ApiException($"InventoryLevel with id: {command.Id}, hasn't been found.");
            await _repository.InventoryLevel.DeleteAsync(product);
            await _repository.SaveAsync();

            return Unit.Value;
        }
    }
}
