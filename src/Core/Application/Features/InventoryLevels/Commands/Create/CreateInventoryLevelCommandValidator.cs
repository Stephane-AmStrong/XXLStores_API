using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using FluentValidation;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.InventoryLevels.Commands.Create
{
    public class CreateInventoryLevelCommandValidator : AbstractValidator<CreateInventoryLevelCommand>
    {
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;

        public CreateInventoryLevelCommandValidator(IRepositoryWrapper repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;


            RuleFor(p => p.ItemId)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull()
                .Must(BeAValidGuid).WithMessage("{PropertyName} is required.");

            RuleFor(p => p)
                .MustAsync(IsUnique).WithMessage("{PropertyName} already exists.");
        }

        private bool BeAValidGuid(Guid id)
        {
            return !id.Equals(new Guid());
        }

        private async Task<bool> IsUnique(CreateInventoryLevelCommand inventoryLevelCommand, CancellationToken cancellationToken)
        {
            var inventoryLevel = _mapper.Map<InventoryLevel>(inventoryLevelCommand);
            return !(await _repository.InventoryLevel.ExistAsync(inventoryLevel));
        }
    }
}
