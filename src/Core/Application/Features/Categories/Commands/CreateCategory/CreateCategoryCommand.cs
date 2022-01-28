using Application.Features.Categories.Queries.GetCategoryById;
using Application.Interfaces;
using Application.Wrappers;
using AutoMapper;
using Domain.Entities;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Categories.Commands.CreateCategory
{
    public class CreateCategoryCommand : IRequest<CategoryViewModel>
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, CategoryViewModel>
    {
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;


        public CreateCategoryCommandHandler(IRepositoryWrapper repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }


        public async Task<CategoryViewModel> Handle(CreateCategoryCommand command, CancellationToken cancellationToken)
        {
            var productEntity = _mapper.Map<Category>(command);

            await _repository.Category.CreateAsync(productEntity);
            await _repository.SaveAsync();

            return _mapper.Map<CategoryViewModel>(productEntity);
        }
    }
}
