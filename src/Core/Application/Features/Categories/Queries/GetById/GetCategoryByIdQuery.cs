using Application.Exceptions;
using Application.Interfaces;
using AutoMapper;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Categories.Queries.GetById
{
    public class GetCategoryByIdQuery : IRequest<CategoryViewModel>
    {
        public Guid Id { get; set; }

        public class GetCategoryByIdQueryHandler : IRequestHandler<GetCategoryByIdQuery, CategoryViewModel>
        {
            private readonly IRepositoryWrapper _repository;
            private readonly IMapper _mapper;

            public GetCategoryByIdQueryHandler(IRepositoryWrapper repository, IMapper mapper)
            {
                _repository = repository;
                _mapper = mapper;
            }

            public async Task<CategoryViewModel> Handle(GetCategoryByIdQuery query, CancellationToken cancellationToken)
            {
                var category = await _repository.Category.GetByIdAsync(query.Id);
                if (category == null) throw new ApiException($"Category Not Found.");
                return _mapper.Map<CategoryViewModel>(category);
            }
        }
    }
}
