using AgriEnergyConnect.Models;
using Microsoft.AspNetCore.Identity;
using AgriEnergyConnect.Data;

namespace AgriEnergyConnect
{
    public static class SeedData
    {
        public static void Initialize(AppDbContext context)
        {
            if (!context.Farmers.Any())
            {
                Farmer[] farmers =
                [
                    new() { Name = "John Agri", Email = "john@agri.com", Location = "Pretoria", PasswordHash = "dummyhash1", UserEmail = "john@agri.com" },
                    new() { Name = "Eco Farmer", Email = "eco@farmer.com", Location = "Cape Town", PasswordHash = "dummyhash2", UserEmail = "eco@farmer.com" }
                ];
                // Ensure all required properties are set for Farmer
                foreach (Farmer farmer in farmers)
                {
                    if (string.IsNullOrWhiteSpace(farmer.PasswordHash))
                    {
                        farmer.PasswordHash = string.Empty;
                    }

                    if (string.IsNullOrWhiteSpace(farmer.UserEmail))
                    {
                        farmer.UserEmail = farmer.Email;
                    }
                }
                context.Farmers.AddRange(farmers);
                _ = context.SaveChanges();

                // After saving farmers, their IDs are set
                Product[] products =
                [
                    new() { Name = "Solar Panel X1", Category = "Solar", FarmerID = farmers[0].Id, Farmer = farmers[0] },
                    new() { Name = "Biogas Kit", Category = "Biogas", FarmerID = farmers[1].Id, Farmer = farmers[1] }
                ];
                context.Products.AddRange(products);
                _ = context.SaveChanges();
            }
        }

        internal static void Initialize(AppDbContext context, UserManager<IdentityUser> userManager)
        {
            // This method is intentionally left blank.
        }

        internal static void Initialize(AppDbContext context, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            throw new NotImplementedException();
        }
    }
}