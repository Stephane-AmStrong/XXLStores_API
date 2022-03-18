using Application.Exceptions;
using Application.Features.Shops.Queries.GetById;
using Application.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Shops.Commands.Update
{
    public class UpdateShopCommand : IRequest<ShopViewModel>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public Guid OwnerId { get; set; }
    }

    internal class UpdateShopCommandHandler : IRequestHandler<UpdateShopCommand, ShopViewModel>
    {
        private readonly ILogger<UpdateShopCommandHandler> _logger;
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;

        public UpdateShopCommandHandler(IRepositoryWrapper repository, IMapper mapper, ILogger<UpdateShopCommandHandler> logger)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<ShopViewModel> Handle(UpdateShopCommand command, CancellationToken cancellationToken)
        {
            var shopEntity = await _repository.Shop.GetByIdAsync(command.Id);
            if (shopEntity == null) throw new ApiException($"Shop with id: {command.Id}, hasn't been found.");

            _mapper.Map(command, shopEntity);
            await _repository.Shop.UpdateAsync(shopEntity);
            await _repository.SaveAsync();

            var shopReadDto = _mapper.Map<ShopViewModel>(shopEntity);
            //if (!string.IsNullOrWhiteSpace(shopReadDto.ImgLink)) shopReadDto.ImgLink = $"{_baseURL}{shopReadDto.ImgLink}";
            return shopReadDto;
        }
    }
}
