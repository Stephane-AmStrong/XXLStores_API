using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using FluentValidation;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Categories.Commands.CreateCategory
{
    public class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
    {
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;

        public CreateCategoryCommandValidator(IRepositoryWrapper repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;

            RuleFor(p => p.Name)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull()
                .MaximumLength(50).WithMessage("{PropertyName} must not exceed 50 characters.");

            RuleFor(p => p.Name)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull()
                .MaximumLength(50).WithMessage("{PropertyName} must not exceed 50 characters.");

            RuleFor(p => p)
                .MustAsync(IsUnique).WithMessage("{PropertyName} already exists.");
        }

        private async Task<bool> IsUnique(CreateCategoryCommand categoryCommand, CancellationToken cancellationToken)
        {
            var _event = _mapper.Map<Category>(categoryCommand);
            return !(await _repository.Event.EventExistAsync(_event));
        }
    }
}
