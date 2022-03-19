using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using Application.Enums;
using Domain.Entities;
using System.Security.Claims;

namespace Infrastructure.Persistence.Seeds
{
    public static class DefaultRoles
    {
        public static async Task SeedAsync(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            var superAdmin = await roleManager.FindByNameAsync(Roles.SuperAdmin.ToString());

            if (superAdmin == null)
            {
                superAdmin = new IdentityRole(Roles.SuperAdmin.ToString());
                await roleManager.CreateAsync(superAdmin);

                await roleManager.AddClaimAsync(superAdmin, new Claim("about.read", "Read About"));
                await roleManager.AddClaimAsync(superAdmin, new Claim("about.write", "Write About"));
                await roleManager.AddClaimAsync(superAdmin, new Claim("about.manage", "Manage About"));
            }

            //Seed Roles

            var admin = await roleManager.FindByNameAsync(Roles.Admin.ToString());
            
            if (admin == null)
            {
                admin = new IdentityRole(Roles.Admin.ToString());
                await roleManager.CreateAsync(admin);

                await roleManager.AddClaimAsync(admin, new Claim("about.read", "Read About"));
                await roleManager.AddClaimAsync(admin, new Claim("about.write", "Write About"));
                await roleManager.AddClaimAsync(admin, new Claim("about.manage", "Manage About"));
            }


            var moderator = await roleManager.FindByNameAsync(Roles.Moderator.ToString());

            if (moderator == null)
            {
                moderator = new IdentityRole(Roles.Moderator.ToString());
                await roleManager.CreateAsync(moderator);

                await roleManager.AddClaimAsync(moderator, new Claim("about.read", "Read About"));
                await roleManager.AddClaimAsync(moderator, new Claim("about.write", "Write About"));
                await roleManager.AddClaimAsync(moderator, new Claim("about.manage", "Manage About"));
            }

            var basic = await roleManager.FindByNameAsync(Roles.Basic.ToString());

            if (basic == null)
            {
                basic = new IdentityRole(Roles.Basic.ToString());
                await roleManager.CreateAsync(basic);

                await roleManager.AddClaimAsync(basic, new Claim("about.read", "Read About"));
                await roleManager.AddClaimAsync(basic, new Claim("about.write", "Write About"));
                await roleManager.AddClaimAsync(basic, new Claim("about.manage", "Manage About"));
            }
        }
    }
}
