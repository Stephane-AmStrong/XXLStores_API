using Application.Interfaces;
using Domain.Entities;
using Domain.Settings;
using Infrastructure.Persistence.Contexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Infrastructure.Persistence.Repository;
using Infrastructure.Persistence.Repositories;

namespace Infrastructure.Persistence
{
    public static class PersistenceServiceExtensions
    {
        public static void AddPersistenceInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer
                (
                   configuration.GetConnectionString("DefaultConnection"),
                   b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)
                )
            );


            #region Repositories
            services.AddScoped<ISortHelper<AppUser>, SortHelper<AppUser>>();
            services.AddScoped<ISortHelper<Category>, SortHelper<Category>>();
            services.AddScoped<ISortHelper<InventoryLevel>, SortHelper<InventoryLevel>>();
            services.AddScoped<ISortHelper<Item>, SortHelper<Item>>();
            services.AddScoped<ISortHelper<Payment>, SortHelper<Payment>>();
            services.AddScoped<ISortHelper<Shop>, SortHelper<Shop>>();
            services.AddScoped<ISortHelper<ShoppingCart>, SortHelper<ShoppingCart>>();
            services.AddScoped<ISortHelper<ShoppingCartItem>, SortHelper<ShoppingCartItem>>();

            services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();
            #endregion
        }
    }
}
