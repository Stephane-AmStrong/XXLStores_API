using Application.Interfaces;
using Domain.Settings;
using Infrastructure.Persistence.Seeds;
using Infrastructure.Persistence.Service;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Infrastructure.Persistence
{
    public static class ClaimPolicyExtensions
    {
        public static void ConfigureClaimPolicy(this IServiceCollection services)
        {
            services.AddAuthorization(option =>
            {
                for (int i = 0; i < ClaimsStore.AllClaims.Count; i++)
                {
                    option.AddPolicy(ClaimsStore.AllClaims[i].PolicyName, policy =>
                    {
                        policy.RequireClaim(ClaimsStore.AllClaims[i].Type);
                        policy.AddAuthenticationSchemes("Bearer");
                    });
                }
            });
        }
    }
}
