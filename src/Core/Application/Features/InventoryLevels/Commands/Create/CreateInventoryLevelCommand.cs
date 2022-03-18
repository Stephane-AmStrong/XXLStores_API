using Application.Features.InventoryLevels.Queries.GetById;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.InventoryLevels.Commands.Create
{
    public class CreateInventoryLevelCommand : IRequest<InventoryLevelViewModel>
    {
        public int InStock { get; set; }
        public int StockAfter { get; set; }
        public DateTime UpdateAt { get; set; }
        public Guid ItemId { get; set; }
    }

    internal class CreateInventoryLevelCommandHandler : IRequestHandler<CreateInventoryLevelCommand, InventoryLevelViewModel>
    {
        private readonly ILogger<CreateInventoryLevelCommandHandler> _logger;
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;


        public CreateInventoryLevelCommandHandler(IRepositoryWrapper repository, IMapper mapper, ILogger<CreateInventoryLevelCommandHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }


        public async Task<InventoryLevelViewModel> Handle(CreateInventoryLevelCommand command, CancellationToken cancellationToken)
        {
            var productEntity = _mapper.Map<InventoryLevel>(command);

            await _repository.InventoryLevel.CreateAsync(productEntity);
            await _repository.SaveAsync();

            return _mapper.Map<InventoryLevelViewModel>(productEntity);
        }
    }
}
