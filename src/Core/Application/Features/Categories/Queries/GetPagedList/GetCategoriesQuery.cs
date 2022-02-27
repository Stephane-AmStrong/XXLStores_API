using Application.Interfaces;
using Application.Wrappers;
using AutoMapper;
using Domain.Parameters;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Categories.Queries.GetPagedList
{
    public class GetCategoriesQuery : QueryParameters, IRequest<PagedList<CategoriesViewModel>>
    {
        public GetCategoriesQuery()
        {
            OrderBy = "name";
        }

        public string WithTheName { get; set; }
    }

    internal class GetCategoriesQueryHandler : IRequestHandler<GetCategoriesQuery, PagedList<CategoriesViewModel>>
    {
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;

        public GetCategoriesQueryHandler(IRepositoryWrapper repository, IMapper mapper)
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
