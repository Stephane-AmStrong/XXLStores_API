﻿using Application.Features.Account.Register;
using Application.Features.Account.SignIn;
using Application.Features.AppUsers.Commands.Create;
using Application.Features.AppUsers.Queries.GetById;
using Application.Features.AppUsers.Queries.GetPagedList;
using Application.Features.Categories.Commands.Create;
using Application.Features.Categories.Commands.Update;
using Application.Features.Categories.Queries.GetById;
using AutoMapper;
using Domain.Entities;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<AppUser, RegisterUserCommand>().ReverseMap();

            CreateMap<LoginModel, SignInCommand>().ReverseMap();
            CreateMap<AuthenticationModel, AppUserViewModel>().ReverseMap();

            CreateMap<AppUser, CreateAppUserCommand>().ReverseMap();

            CreateMap<AppUser, AppUserViewModel>().ReverseMap();
            CreateMap<AppUser, GetAppUsersViewModel>().ReverseMap();

            CreateMap<Category, CreateCategoryCommand>().ReverseMap();
            CreateMap<Category, CategoryViewModel>().ReverseMap();
            CreateMap<Category, UpdateCategoryCommand>().ReverseMap();
        }
    }
}
