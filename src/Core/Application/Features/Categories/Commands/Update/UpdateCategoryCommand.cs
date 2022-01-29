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
                var productEntity = await _repository.Category.GetByIdAsync(command.Id);

                if (productEntity == null)
                {
                    throw new ApiException($"Category Not Found.");
                }

                _mapper.Map(command, productEntity);

                await _repository.Category.UpdateAsync(productEntity);
                await _repository.SaveAsync();

                var productReadDto = _mapper.Map<CategoryViewModel>(productEntity);

                //if (!string.IsNullOrWhiteSpace(productReadDto.ImgLink)) productReadDto.ImgLink = $"{_baseURL}{productReadDto.ImgLink}";

                return productReadDto;


            }
        }
    }
}
