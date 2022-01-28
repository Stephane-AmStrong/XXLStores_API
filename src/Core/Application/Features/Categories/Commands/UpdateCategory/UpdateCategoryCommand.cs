using Application.Exceptions;
using Application.Features.Categories.Queries.GetCategoryById;
using Application.Interfaces;
using Application.Wrappers;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Categories.Commands.UpdateCategory
{
    public class UpdateCategoryCommand : IRequest<GetCategoryViewModel>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }


        public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, GetCategoryViewModel>
        {
            private readonly IRepositoryWrapper _repository;
            private readonly IMapper _mapper;

            public UpdateCategoryCommandHandler(IRepositoryWrapper repository, IMapper mapper)
            {
                _repository = repository;
                _mapper = mapper;
            }

            public async Task<GetCategoryViewModel> Handle(UpdateCategoryCommand command, CancellationToken cancellationToken)
            {
                var productEntity = await _repository.Category.GetByIdAsync(command.Id);

                if (productEntity == null)
                {
                    throw new ApiException($"Category Not Found.");
                }

                _mapper.Map(command, productEntity);

                await _repository.Category.UpdateAsync(productEntity);
                await _repository.SaveAsync();

                var productReadDto = _mapper.Map<GetCategoryViewModel>(productEntity);

                //if (!string.IsNullOrWhiteSpace(productReadDto.ImgLink)) productReadDto.ImgLink = $"{_baseURL}{productReadDto.ImgLink}";

                return productReadDto;


            }
        }
    }
}
