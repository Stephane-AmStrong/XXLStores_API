using Application.DataTransfertObjects.Account;
using Application.Features.Account.Commands.Authenticate;
using Application.Features.Account.Commands.RefreshAccessToken;
using Application.Features.Account.Commands.RegisterUser;
using Application.Features.Account.Commands.ResetPassword;
using Application.Features.Categories.Commands.Create;
using Application.Features.Categories.Commands.Update;
using Application.Features.Categories.Queries.GetById;
using Application.Features.Categories.Queries.GetPagedList;
using Application.Models;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<AppUser, RegisterUserCommand>().ReverseMap();
            CreateMap<UserToken, UserTokenViewModel>().ReverseMap();
            CreateMap<RefreshTokens, RefreshTokensViewModel>().ReverseMap();
            CreateMap<ResetPasswordRequest, ResetPasswordCommand>().ReverseMap();
            //CreateMap<GenerateRefreshTokenModel, RefreshTokenCommand>().ReverseMap();
            //RefreshTokenCommand -> GenerateRefreshTokenModel

            CreateMap<LoginModel, AuthenticationCommand>().ReverseMap();
            CreateMap<AuthenticationModel, AuthenticationViewModel>().ReverseMap();

            //CreateMap<Utilisateur, CreateAppUserCommand>().ReverseMap();

            //CreateMap<Utilisateur, UserTokenViewModel>().ReverseMap();
            //CreateMap<Utilisateur, GetAppUsersViewModel>().ReverseMap();

            CreateMap<Category, CreateCategoryCommand>().ReverseMap();
            CreateMap<Category, CategoryViewModel>().ReverseMap();
            CreateMap<Category, CategoriesViewModel>().ReverseMap();
            CreateMap<Category, UpdateCategoryCommand>().ReverseMap();
        }
    }
}
