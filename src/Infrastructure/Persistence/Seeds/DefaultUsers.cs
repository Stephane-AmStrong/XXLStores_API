using Application.Enums;
using Application.Models;
using Domain.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Persistence.Seeds
{
    public static class DefaultUsers
    {
        public static async Task<WebApplication> SeedVendorAsync(this WebApplication webApp)
        {
            using (var scope = webApp.Services.CreateScope())
            {
                using (var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>())
                {
                    var defaultUser = new AppUser
                    {
                        UserName = "vendor",
                        Email = "vendor@gmail.com",
                        FirstName = "John the Seller",
                        LastName = "Businessman",
                        EmailConfirmed = true,
                        PhoneNumberConfirmed = true
                    };

                    if (userManager.Users.All(u => u.Id != defaultUser.Id))
                    {
                        var user = await userManager.FindByEmailAsync(defaultUser.Email);
                        if (user == null)
                        {
                            await userManager.CreateAsync(defaultUser, "123Pa$$word!");
                            await userManager.AddToRoleAsync(defaultUser, Roles.Vendor.ToString());
                        }

                    }
                }
            }

            return webApp;
        }

        public static async Task<WebApplication> SeedCustomerAsync(this WebApplication webApp)
        {
            using (var scope = webApp.Services.CreateScope())
            {
                using (var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>())
                {
                    //Seed Default User
                    var defaultUser = new AppUser
                    {
                        UserName = "customer",
                        Email = "customer@gmail.com",
                        FirstName = "Ideal Customer",
                        LastName = "Ideal",
                        EmailConfirmed = true,
                        PhoneNumberConfirmed = true
                    };

                    if (userManager.Users.All(u => u.Id != defaultUser.Id))
                    {
                        var user = await userManager.FindByEmailAsync(defaultUser.Email);
                        if (user == null)
                        {
                            await userManager.CreateAsync(defaultUser, "123Pa$$word!");
                            await userManager.AddToRoleAsync(defaultUser, Roles.Customer.ToString());
                        }

                    }
                }
            }
            return webApp;
        }
    }
}
