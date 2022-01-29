using Application.Interfaces;
using Application.Parameters;
using Application.Wrappers;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Shops.Queries.GetPagedList
{
    public class GetShopsQuery : QueryStringParameters, IRequest<PagedList<ShopsViewModel>>
    {
        public GetShopsQuery()
        {
            OrderBy = "name";
        }

        public string WithTheName { get; set; }
    }

    public class GetAllShopsQueryHandler : IRequestHandler<GetShopsQuery, PagedList<ShopsViewModel>>
    {
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;

        public GetAllShopsQueryHandler(IRepositoryWrapper repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }


        public async Task<PagedList<ShopsViewModel>> Handle(GetShopsQuery query, CancellationToken cancellationToken)
        {
            var shops = await _repository.Shop.GetPagedListAsync(query);
            return  _mapper.Map<PagedList<ShopsViewModel>>(shops);
        }
    }
}
