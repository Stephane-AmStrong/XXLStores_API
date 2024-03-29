﻿using Application.Exceptions;
using Application.Features.InventoryLevels.Queries.GetById;
using Application.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.InventoryLevels.Commands.Update
{
    public class UpdateInventoryLevelCommand : IRequest<InventoryLevelViewModel>
    {
        public virtual Guid Id { get; set; }
        public int InStock { get; set; }
        public int StockAfter { get; set; }
        public DateTime UpdateAt { get; set; }
        public Guid ItemId { get; set; }
    }

    internal class UpdateInventoryLevelCommandHandler : IRequestHandler<UpdateInventoryLevelCommand, InventoryLevelViewModel>
    {
        private readonly ILogger<UpdateInventoryLevelCommandHandler> _logger;
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;

        public UpdateInventoryLevelCommandHandler(IRepositoryWrapper repository, IMapper mapper, ILogger<UpdateInventoryLevelCommandHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<InventoryLevelViewModel> Handle(UpdateInventoryLevelCommand command, CancellationToken cancellationToken)
        {
            var productEntity = await _repository.InventoryLevel.GetByIdAsync(command.Id);

            if (productEntity == null) throw new ApiException($"InventoryLevel with id: {command.Id}, hasn't been found.");

            _mapper.Map(command, productEntity);

            await _repository.InventoryLevel.UpdateAsync(productEntity);
            await _repository.SaveAsync();

            var productReadDto = _mapper.Map<InventoryLevelViewModel>(productEntity);

            return productReadDto;
        }
    }
}
