using Application.Interfaces;
using Application.Wrappers;
using AutoMapper;
using Domain.Parameters;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Categories.Queries.GetPagedList
{
    public class GetCategoriesQuery : QueryParameters, IRequest<PagedListResponse<CategoriesViewModel>>
    {
        public GetCategoriesQuery()
        {
            OrderBy = "name";
        }

        public string WithTheName { get; set; }
    }

    internal class GetCategoriesQueryHandler : IRequestHandler<GetCategoriesQuery, PagedListResponse<CategoriesViewModel>>
    {
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;

        public GetCategoriesQueryHandler(IRepositoryWrapper repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }


        public async Task<PagedListResponse<CategoriesViewModel>> Handle(GetCategoriesQuery query, CancellationToken cancellationToken)
        {
            var categories = await _repository.Category.GetPagedListAsync(query);
            var lstCategoriesViewModel = _mapper.Map<List<CategoriesViewModel>>(categories);

            var categoriesResponse = new PagedListResponse<CategoriesViewModel>(lstCategoriesViewModel, categories.MetaData);
            return categoriesResponse;
        }
    }
}
