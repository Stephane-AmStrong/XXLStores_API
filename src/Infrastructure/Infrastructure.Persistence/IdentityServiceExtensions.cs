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

namespace Infrastructure.Persistence
{
    public static class IdentityServiceExtensions
    {
        public static void AddIdentityInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<IdentityContext>(options =>
                options.UseSqlServer(
                        configuration.GetConnectionString("IdentityConnection"),
                        b => b.MigrationsAssembly(typeof(IdentityContext).Assembly.FullName)
                    )
                );


            services.AddIdentity<AppUser, IdentityRole>().AddEntityFrameworkStores<IdentityContext>().AddDefaultTokenProviders();
            #region Services
            services.AddTransient<IAccountRepository, AccountRepository>();
            #endregion

            services.Configure<JWTSettings>(configuration.GetSection("JWTSettings"));
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(o =>
                {
                    o.RequireHttpsMetadata = false;
                    o.SaveToken = false;
                    o.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = bool.Parse(configuration["JWTSettings:ValidateIssuerSigningKey"]),
                        ValidateIssuer = bool.Parse(configuration["JWTSettings:ValidateIssuer"]),
                        ValidateAudience = bool.Parse(configuration["JWTSettings:ValidateAudience"]),
                        ValidateLifetime = bool.Parse(configuration["JWTSettings:ValidateLifetime"]),
                        ClockSkew = TimeSpan.Zero,
                        ValidIssuer = configuration["JWTSettings:Issuer"],
                        ValidAudience = configuration["JWTSettings:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWTSettings:Key"]))
                    };
                    o.Events = new JwtBearerEvents()
                    {
                        OnAuthenticationFailed = c =>
                        {
                            c.NoResult();
                            c.Response.StatusCode = 500;
                            c.Response.ContentType = "text/plain";
                            return c.Response.WriteAsync(c.Exception.ToString());
                        },
                        OnChallenge = context =>
                        {
                            context.HandleResponse();
                            context.Response.StatusCode = 401;
                            context.Response.ContentType = "application/json";
                            var result = JsonConvert.SerializeObject("You are not Authorized");
                            return context.Response.WriteAsync(result);
                        },
                        OnForbidden = context =>
                        {
                            context.Response.StatusCode = 403;
                            context.Response.ContentType = "application/json";
                            var result = JsonConvert.SerializeObject("You are not authorized to access this resource");
                            return context.Response.WriteAsync(result);
                        },
                    };
                });
        }
    }
}
