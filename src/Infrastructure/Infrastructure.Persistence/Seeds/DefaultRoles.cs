using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using Application.Enums;
using Domain.Entities;
using System.Security.Claims;
using System.Collections.Generic;
using Application.Wrappers;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Persistence.Seeds
{
    public static class DefaultRoles
    {
        public static async Task<WebApplication> SeedDefaultRolesAsync(this WebApplication webApp)
        {
            using (var scope = webApp.Services.CreateScope())
            {
                using (var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>())
                {
                    var superAdmin = await roleManager.FindByNameAsync(Roles.SuperAdmin.ToString());

                    if (superAdmin == null)
                    {
                        superAdmin = new IdentityRole(Roles.SuperAdmin.ToString());
                        await roleManager.CreateAsync(superAdmin);

                        for (int i = 0; i < ClaimsStore.AllClaims.Count; i++)
                        {
                            await roleManager.AddClaimAsync(superAdmin, ClaimsStore.AllClaims[i]);
                        }
                    }


                    var admin = await roleManager.FindByNameAsync(Roles.Admin.ToString());

                    if (admin == null)
                    {
                        admin = new IdentityRole(Roles.Admin.ToString());
                        await roleManager.CreateAsync(admin);

                        for (int i = 0; i < ClaimsStore.AllClaims.Count; i++)
                        {
                            await roleManager.AddClaimAsync(admin, ClaimsStore.AllClaims[i]);
                        }
                    }


                    var vendor = await roleManager.FindByNameAsync(Roles.Vendor.ToString());

                    if (vendor == null)
                    {
                        vendor = new IdentityRole(Roles.Vendor.ToString());
                        await roleManager.CreateAsync(vendor);

                        for (int i = 0; i < ClaimsStore.VendorClaims.Count; i++)
                        {
                            await roleManager.AddClaimAsync(vendor, ClaimsStore.VendorClaims[i]);
                        }
                    }

                    var customer = await roleManager.FindByNameAsync(Roles.Customer.ToString());

                    if (customer == null)
                    {
                        customer = new IdentityRole(Roles.Customer.ToString());
                        await roleManager.CreateAsync(customer);

                        for (int i = 0; i < ClaimsStore.CustomerClaims.Count; i++)
                        {
                            await roleManager.AddClaimAsync(customer, ClaimsStore.CustomerClaims[i]);
                        }
                    }
                }
            }

            return webApp;
        }

        
        /*
        public static async Task SeedAsync(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            var superAdmin = await roleManager.FindByNameAsync(Roles.SuperAdmin.ToString());

            if (superAdmin == null)
            {
                superAdmin = new IdentityRole(Roles.SuperAdmin.ToString());
                await roleManager.CreateAsync(superAdmin);

                for (int i = 0; i < ClaimsStore.AllClaims.Count; i++)
                {
                    await roleManager.AddClaimAsync(superAdmin, ClaimsStore.AllClaims[i]);
                }
            }


            var admin = await roleManager.FindByNameAsync(Roles.Admin.ToString());

            if (admin == null)
            {
                admin = new IdentityRole(Roles.Admin.ToString());
                await roleManager.CreateAsync(admin);

                for (int i = 0; i < ClaimsStore.AllClaims.Count; i++)
                {
                    await roleManager.AddClaimAsync(admin, ClaimsStore.AllClaims[i]);
                }
            }


            var vendor = await roleManager.FindByNameAsync(Roles.Vendor.ToString());

            if (vendor == null)
            {
                vendor = new IdentityRole(Roles.Vendor.ToString());
                await roleManager.CreateAsync(vendor);

                for (int i = 0; i < ClaimsStore.VendorClaims.Count; i++)
                {
                    await roleManager.AddClaimAsync(vendor, ClaimsStore.VendorClaims[i]);
                }
            }

            var customer = await roleManager.FindByNameAsync(Roles.Customer.ToString());

            if (customer == null)
            {
                customer = new IdentityRole(Roles.Customer.ToString());
                await roleManager.CreateAsync(customer);

                for (int i = 0; i < ClaimsStore.CustomerClaims.Count; i++)
                {
                    await roleManager.AddClaimAsync(customer, ClaimsStore.CustomerClaims[i]);
                }
            }
        }
        */
    }

    public static class ClaimsStore
    {
        public static List<ClaimWrapper> AllClaims = new List<ClaimWrapper>
        {
            new ClaimWrapper("appuser.read.policy", "appuser.read", "Read AppUsers"),
            new ClaimWrapper("appuser.write.policy", "appuser.write", "Write AppUsers"),
            new ClaimWrapper("appuser.manage.policy", "appuser.manage", "Manage AppUsers"),
            
            new ClaimWrapper("category.read.policy",  "category.read",  "Read Categories"),
            new ClaimWrapper("category.write.policy", "category.write", "Write Categories"),
            new ClaimWrapper("category.manage.policy","category.manage", "Manage Categories"),
            
            new ClaimWrapper("inventory.read.policy",  "inventory.read",  "Read Inventory"),
            new ClaimWrapper("inventory.write.policy", "inventory.write", "Write Inventory"),
            new ClaimWrapper("inventory.manage.policy","inventory.manage", "Manage Inventory"),
            
            new ClaimWrapper("item.read.policy",  "item.read",  "Read Items"),
            new ClaimWrapper("item.write.policy", "item.write", "Write Items"),
            new ClaimWrapper("item.manage.policy","item.manage", "Manage Items"),
            
            new ClaimWrapper("payment.read.policy",  "payment.read",  "Read Payments"),
            new ClaimWrapper("payment.write.policy", "payment.write", "Write Payments"),
            new ClaimWrapper("payment.manage.policy","payment.manage", "Manage Payments"),
            
            new ClaimWrapper("shop.read.policy",  "shop.read",  "Read Shops"),
            new ClaimWrapper("shop.write.policy", "shop.write", "Write Shops"),
            new ClaimWrapper("shop.manage.policy","shop.manage", "Manage Shops"),
            
            new ClaimWrapper("shoppingCart.read.policy",  "shoppingCart.read",  "Read ShoppingCarts"),
            new ClaimWrapper("shoppingCart.write.policy", "shoppingCart.write", "Write ShoppingCarts"),
            new ClaimWrapper("shoppingCart.manage.policy","shoppingCart.manage", "Manage ShoppingCarts"),
            
            new ClaimWrapper("shoppingCartItem.read.policy", "shoppingCartItem.read", "Read ShoppingCartItems"),
            new ClaimWrapper("shoppingCartItem.write.policy", "shoppingCartItem.write", "Write ShoppingCartItems"),
            new ClaimWrapper("shoppingCartItem.manage.policy", "shoppingCartItem.manage", "Manage ShoppingCartItems"),
        };

        public static List<Claim> VendorClaims = new List<Claim>
        {
            new Claim("appuser.read", "Read AppUsers"),
            new Claim("appuser.write", "Write AppUsers"),
            new Claim("appuser.manage", "Manage AppUsers"),
            
            new Claim("category.read",  "Read Categories"),
            
            new Claim("inventory.read",  "Read Inventory"),
            new Claim("inventory.write", "Write Inventory"),
            
            new Claim("item.read",  "Read Items"),
            new Claim("item.write", "Write Items"),
            
            new Claim("payment.read",  "Read Payments"),
            new Claim("payment.write", "Write Payments"),
            
            new Claim("shop.read",  "Read Shops"),
            new Claim("shop.write", "Write Shops"),
            
            new Claim("shoppingCart.read",  "Read ShoppingCarts"),
            new Claim("shoppingCart.write", "Write ShoppingCarts"),
            
            new Claim("shoppingCartItem.read",  "Read ShoppingCartItems"),
            new Claim("shoppingCartItem.write", "Write ShoppingCartItems"),
        };
        
        public static List<Claim> CustomerClaims = new List<Claim>
        {
            new Claim("appuser.read", "Read AppUsers"),
            new Claim("appuser.write", "Write AppUsers"),
            
            new Claim("category.read",  "Read Categories"),
            
            new Claim("inventory.read",  "Read Inventory"),
            new Claim("inventory.write", "Write Inventory"),
            
            new Claim("item.read",  "Read Items"),
            
            new Claim("payment.read",  "Read Payments"),
            new Claim("payment.write", "Write Payments"),
            
            new Claim("shop.read",  "Read Shops"),
            
            new Claim("shoppingCart.read",  "Read ShoppingCarts"),
            new Claim("shoppingCart.write", "Write ShoppingCarts"),
            
            new Claim("shoppingCartItem.read",  "Read ShoppingCartItems"),
            new Claim("shoppingCartItem.write", "Write ShoppingCartItems"),
        };
    }
}
