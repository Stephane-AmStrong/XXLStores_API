﻿using Application.Exceptions;
using Application.Interfaces;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.InventoryLevels.Commands.DeleteInventoryLevelById
{
    public class DeleteInventoryLevelByIdCommand : IRequest
    {
        public Guid Id { get; set; }

        public class DeleteInventoryLevelByIdCommandHandler : IRequestHandler<DeleteInventoryLevelByIdCommand>
        {
            private readonly IRepositoryWrapper _repository;

            public DeleteInventoryLevelByIdCommandHandler(IRepositoryWrapper repository)
            {
                _repository = repository;
            }

            public async Task<Unit> Handle(DeleteInventoryLevelByIdCommand command, CancellationToken cancellationToken)
            {
                var product = await _repository.InventoryLevel.GetByIdAsync(command.Id);
                if (product == null) throw new ApiException($"InventoryLevel Not Found.");
                await _repository.InventoryLevel.DeleteAsync(product);
                await _repository.SaveAsync();

                return Unit.Value;
            }
        }
    }
}
