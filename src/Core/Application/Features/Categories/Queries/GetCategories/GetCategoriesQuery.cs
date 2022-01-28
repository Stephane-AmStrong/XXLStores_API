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

namespace Application.Features.Categories.Queries.GetCategories
{
    public class GetCategoriesQuery : QueryStringParameters, IRequest<PagedList<CategoriesViewModel>>
    {
        public GetCategoriesQuery()
        {
            OrderBy = "name";
        }

        public string WithTheName { get; set; }
    }

    public class GetAllCategoriesQueryHandler : IRequestHandler<GetCategoriesQuery, PagedList<CategoriesViewModel>>
    {
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;

        public GetAllCategoriesQueryHandler(IRepositoryWrapper repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }


        public async Task<PagedList<CategoriesViewModel>> Handle(GetCategoriesQuery query, CancellationToken cancellationToken)
        {
            var categories = await _repository.Category.GetPagedListAsync(query);
            return  _mapper.Map<PagedList<CategoriesViewModel>>(categories);
        }
    }
}
