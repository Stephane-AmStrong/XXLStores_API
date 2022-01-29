﻿using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using FluentValidation;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.AppUsers.Commands.Update
{
    public class UpdateAppUserCommandValidator : AbstractValidator<UpdateAppUserCommand>
    {
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;

        public UpdateAppUserCommandValidator(IRepositoryWrapper repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;

            RuleFor(p => p.FirstName)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull()
                .MaximumLength(50).WithMessage("{PropertyName} must not exceed 50 characters.");

            RuleFor(p => p.LastName)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull()
                .MaximumLength(50).WithMessage("{PropertyName} must not exceed 50 characters.");

            RuleFor(p => p.RoleName)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull();
        }
    }
}