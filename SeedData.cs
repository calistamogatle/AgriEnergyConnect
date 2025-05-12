using AgriEnergyConnect.Models;
using Microsoft.AspNetCore.Identity;

namespace AgriEnergyConnect.Data
{
    public static class SeedData
    {
        public static void Initialize(AppDbContext context)
        {
            if (!context.Farmers.Any())
            {
                var farmers = new Farmer[]
                {
                    new Farmer { Name = "John Agri", Email = "john@agri.com", Location = "Pretoria" },
                    new Farmer { Name = "Eco Farmer", Email = "eco@farmer.com", Location = "Cape Town" }
                };

                context.Farmers.AddRange(farmers);
                _ = context.SaveChanges();

                var products = new Product[]
                {
                    new Product { Name = "Solar Panel X1", Category = "Solar", FarmerID = 1 },
                    new Product { Name = "Biogas Kit", Category = "Biogas", FarmerID = 2 }
                };

                object value = context.Products.AddRange(products);
                _ = context.SaveChanges();
            }
        }

        internal static async Task Initialize(AppDbContext context, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            throw new NotImplementedException();
        }
    }
}