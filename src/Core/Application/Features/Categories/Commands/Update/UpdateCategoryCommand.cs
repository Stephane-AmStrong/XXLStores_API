using Application.Exceptions;
using Application.Features.Categories.Queries.GetById;
using Application.Interfaces;
using AutoMapper;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Categories.Commands.Update
{
    public class UpdateCategoryCommand : IRequest<CategoryViewModel>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }


        public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, CategoryViewModel>
        {
            private readonly IRepositoryWrapper _repository;
            private readonly IMapper _mapper;

            public UpdateCategoryCommandHandler(IRepositoryWrapper repository, IMapper mapper)
            {
                _repository = repository;
                _mapper = mapper;
            }

            public async Task<CategoryViewModel> Handle(UpdateCategoryCommand command, CancellationToken cancellationToken)
            {
                var categoryEntity = await _repository.Category.GetByIdAsync(command.Id);
                if (categoryEntity == null) throw new ApiException($"Category with id: {command.Id}, hasn't been found.");

                _mapper.Map(command, categoryEntity);
                await _repository.Category.UpdateAsync(categoryEntity);
                await _repository.SaveAsync();

                var categoryReadDto = _mapper.Map<CategoryViewModel>(categoryEntity);
                //if (!string.IsNullOrWhiteSpace(categoryReadDto.ImgLink)) categoryReadDto.ImgLink = $"{_baseURL}{categoryReadDto.ImgLink}";
                return categoryReadDto;
            }
        }
    }
}
