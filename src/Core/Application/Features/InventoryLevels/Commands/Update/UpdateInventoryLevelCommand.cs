using Application.Exceptions;
using Application.Features.InventoryLevels.Queries.GetInventoryLevelById;
using Application.Interfaces;
using Application.Wrappers;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
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

    public class UpdateInventoryLevelCommandHandler : IRequestHandler<UpdateInventoryLevelCommand, InventoryLevelViewModel>
    {
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;

        public UpdateInventoryLevelCommandHandler(IRepositoryWrapper repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<InventoryLevelViewModel> Handle(UpdateInventoryLevelCommand command, CancellationToken cancellationToken)
        {
            var productEntity = await _repository.InventoryLevel.GetByIdAsync(command.Id);

            if (productEntity == null)
            {
                throw new ApiException($"InventoryLevel Not Found.");
            }

            _mapper.Map(command, productEntity);

            await _repository.InventoryLevel.UpdateAsync(productEntity);
            await _repository.SaveAsync();

            var productReadDto = _mapper.Map<InventoryLevelViewModel>(productEntity);

            //if (!string.IsNullOrWhiteSpace(productReadDto.ImgLink)) productReadDto.ImgLink = $"{_baseURL}{productReadDto.ImgLink}";

            return productReadDto;


        }
    }

}
