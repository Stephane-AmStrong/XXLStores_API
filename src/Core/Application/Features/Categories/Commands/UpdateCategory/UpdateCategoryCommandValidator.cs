using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using FluentValidation;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Categories.Commands.UpdateCategory
{
    public class UpdateCategoryCommandValidator : AbstractValidator<UpdateCategoryCommand>
    {
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;

        public UpdateCategoryCommandValidator(IRepositoryWrapper repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;

            RuleFor(p => p.Name)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull()
                .MaximumLength(50).WithMessage("{PropertyName} must not exceed 50 characters.");

            RuleFor(p => p.Description)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull()
                .MinimumLength(10).WithMessage("{PropertyName} must be at least 10 characters.");

            RuleFor(p => p)
                .MustAsync(IsUnique).WithMessage("{PropertyName} already exists.");
        }

        private async Task<bool> IsUnique(UpdateCategoryCommand categoryCommand, CancellationToken cancellationToken)
        {
            var _event = _mapper.Map<Category>(categoryCommand);
            return !(await _repository.Event.EventExistAsync(_event));
        }
    }
}
