using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using FluentValidation;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.InventoryLevels.Commands.Update
{
    public class UpdateInventoryLevelCommandValidator : AbstractValidator<UpdateInventoryLevelCommand>
    {
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;

        public UpdateInventoryLevelCommandValidator(IRepositoryWrapper repository, IMapper mapper)
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

        private async Task<bool> IsUnique(UpdateInventoryLevelCommand inventoryLevelCommand, CancellationToken cancellationToken)
        {
            var inventoryLevel = _mapper.Map<InventoryLevel>(inventoryLevelCommand);
            return !(await _repository.InventoryLevel.ExistAsync(inventoryLevel));
        }
    }
}
