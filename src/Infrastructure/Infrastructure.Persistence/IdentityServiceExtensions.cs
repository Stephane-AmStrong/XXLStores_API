using Application.Interfaces;
using Domain.Entities;
using Domain.Settings;
using Infrastructure.Persistence.Contexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Text;

using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Infrastructure.Persistence.Repository;
using System.Text.Json;
using Newtonsoft.Json.Linq;

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

                    //o.Events = new JwtBearerEvents()
                    //{
                    //    OnAuthenticationFailed = context =>
                    //    {
                    //        context.NoResult();
                    //        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    //        context.Response.ContentType = "application/json";
                    //        return context.Response.WriteAsync(JsonSerializer.Serialize(context.Exception.ToString()));
                    //    },
                    //    OnChallenge = context =>
                    //    {
                    //        context.HandleResponse();
                    //        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    //        context.Response.ContentType = "application/json";
                    //        var result = JsonSerializer.Serialize("You are not Authorized");
                    //        return context.Response.WriteAsync(result);
                    //    },
                    //    OnForbidden = context =>
                    //    {
                    //        context.Response.StatusCode = StatusCodes.Status403Forbidden;
                    //        context.Response.ContentType = "application/json";
                    //        var result = JsonSerializer.Serialize("You are not authorized to access this resource");
                    //        return context.Response.WriteAsync(result);
                    //    },
                    //};


                    //

                    o.Events = new JwtBearerEvents
                    {
                        OnAuthenticationFailed = context =>
                        {
                            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                            string message = "";
                            message += FlattenException(StatusCodes.Status401Unauthorized, context.Exception);
                            return Task.CompletedTask;
                        },

                        OnChallenge = context =>
                        {
                            context.HandleResponse();
                            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                            context.Response.ContentType = "application/json";

                            var errorDetails = new JObject
                            {
                                //["error"] = context.Error,
                                ["StatusCode"] = StatusCodes.Status401Unauthorized,
                                ["Message"] = context.ErrorDescription
                            };

                            return context.Response.WriteAsync(errorDetails.ToString());
                        },

                        OnForbidden = context =>
                        {
                            context.Response.StatusCode = StatusCodes.Status403Forbidden;
                            context.Response.ContentType = "application/json";

                            var errorDetails = new JObject
                            {
                                ["StatusCode"] = StatusCodes.Status401Unauthorized,
                                ["Message"] = "You are not Authorized"
                            };

                            return context.Response.WriteAsync(JsonSerializer.Serialize(errorDetails));
                        }
                    };

                });
        }

        public static string FlattenException(int statusCode, Exception exception)
        {
            var stringBuilder = new StringBuilder();

            while (exception != null)
            {
                stringBuilder.AppendLine(exception.Message);
                //stringBuilder.AppendLine(exception.StackTrace);

                exception = exception.InnerException;
            }

            var errorDetails = new JObject
            {
                ["StatusCode"] = statusCode,
                ["Message"] = stringBuilder.ToString()
            };

            return JsonSerializer.Serialize(errorDetails);
        }
    }
}
