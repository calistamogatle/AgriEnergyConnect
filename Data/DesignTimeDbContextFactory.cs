using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace AgriEnergyConnect.Data
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            DbContextOptionsBuilder<AppDbContext> optionsBuilder = new();
            _ = optionsBuilder.UseNpgsql("Host=localhost;Database=AgriEnergyConnect;Username=postgres;Password=Kari@21100");

            // Resolve ambiguity by explicitly specifying the constructor
            return Activator.CreateInstance(typeof(AppDbContext), optionsBuilder.Options) as AppDbContext;
        }
    }
}

