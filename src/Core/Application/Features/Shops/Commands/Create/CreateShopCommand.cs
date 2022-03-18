using Application.Features.Shops.Queries.GetById;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Shops.Commands.Create
{
    public class CreateShopCommand : IRequest<ShopViewModel>
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public Guid OwnerId { get; set; }
    }

    internal class CreateShopCommandHandler : IRequestHandler<CreateShopCommand, ShopViewModel>
    {
        private readonly ILogger<CreateShopCommandHandler> _logger;
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;


        public CreateShopCommandHandler(IRepositoryWrapper repository, IMapper mapper, ILogger<CreateShopCommandHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }


        public async Task<ShopViewModel> Handle(CreateShopCommand command, CancellationToken cancellationToken)
        {
            var shopEntity = _mapper.Map<Shop>(command);

            await _repository.Shop.CreateAsync(shopEntity);
            await _repository.SaveAsync();

            return _mapper.Map<ShopViewModel>(shopEntity);
        }
    }
}
