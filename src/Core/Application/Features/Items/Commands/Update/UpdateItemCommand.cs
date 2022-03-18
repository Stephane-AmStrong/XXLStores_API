using Application.Exceptions;
using Application.Features.Items.Queries.GetById;
using Application.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Items.Commands.Update
{
    public class UpdateItemCommand : IRequest<ItemViewModel>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid CategoryId { get; set; }
        public Guid ShopId { get; set; }
    }

    internal class UpdateItemCommandHandler : IRequestHandler<UpdateItemCommand, ItemViewModel>
    {
        private readonly ILogger<UpdateItemCommandHandler> _logger;
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;

        public UpdateItemCommandHandler(IRepositoryWrapper repository, IMapper mapper, ILogger<UpdateItemCommandHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ItemViewModel> Handle(UpdateItemCommand command, CancellationToken cancellationToken)
        {
            var itemEntity = await _repository.Item.GetByIdAsync(command.Id);
            if (itemEntity == null) throw new ApiException($"Item with id: {command.Id}, hasn't been found.");

            _mapper.Map(command, itemEntity);
            await _repository.Item.UpdateAsync(itemEntity);
            await _repository.SaveAsync();

            var itemReadDto = _mapper.Map<ItemViewModel>(itemEntity);
            //if (!string.IsNullOrWhiteSpace(itemReadDto.ImgLink)) itemReadDto.ImgLink = $"{_baseURL}{itemReadDto.ImgLink}";
            return itemReadDto;
        }
    }
}
