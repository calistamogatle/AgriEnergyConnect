using System.Text.Json.Serialization;
using AgriEnergyConnect.Models;
using AgriEnergyConnect.Models.Account;
using Microsoft.EntityFrameworkCore;

namespace AgriEnergyConnect.Data
{
    // Only one definition of AppDbContext should exist in this namespace
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            // Recommended for development - validates model on startup
            if (Database.IsRelational())
            {
                _ = Database.EnsureCreated();
            }
        }

        // Existing DbSets
        [JsonIgnore]
        public DbSet<Farmer> Farmers { get; set; }
        [JsonIgnore]
        public DbSet<Product> Products { get; set; }
        [JsonIgnore]
        public DbSet<Employee> Employees { get; set; }
        [JsonIgnore]
        public DbSet<ProductFilterViewModel> ProductFilterViewModels { get; set; }
        [JsonIgnore]
        public DbSet<AuthUser> Users { get; set; } = null!; // Fix CS8618

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Farmer configuration
            _ = modelBuilder.Entity<Farmer>(entity =>
            {
                _ = entity.HasKey(f => f.FarmerID);
                _ = entity.HasIndex(f => f.Email).IsUnique();
                _ = entity.Property(f => f.Name).HasMaxLength(100).IsRequired();
                _ = entity.Property(f => f.Email).HasMaxLength(100).IsRequired();
                _ = entity.Property(f => f.PasswordHash).IsRequired();
            });

            // Product configuration
            _ = modelBuilder.Entity<Product>(entity =>
            {
                _ = entity.HasKey(p => p.ProductID);
                _ = entity.Property(p => p.Name).HasMaxLength(100).IsRequired();
                _ = entity.Property(p => p.Category).HasMaxLength(50).IsRequired();
                _ = entity.Property(p => p.ProductionDate)
                          .HasDefaultValueSql("CURRENT_TIMESTAMP");
            });

            // Employee configuration
            _ = modelBuilder.Entity<Employee>(entity =>
            {
                _ = entity.HasKey(e => e.Id);
                _ = entity.HasIndex(e => e.Email).IsUnique();
                _ = entity.Property(e => e.FirstName).HasMaxLength(50).IsRequired();
                _ = entity.Property(e => e.LastName).HasMaxLength(50).IsRequired();
                _ = entity.Property(e => e.Email).HasMaxLength(100).IsRequired();
                _ = entity.Property(e => e.Department).HasMaxLength(50);
                _ = entity.Property(e => e.Position).HasMaxLength(50);
                _ = entity.Property(e => e.PasswordHash).IsRequired();
            });

            // Relationship configuration
            _ = modelBuilder.Entity<Product>()
                .HasOne(p => p.Farmer)
                .WithMany(f => f.Products)
                .HasForeignKey(p => p.FarmerID)
                .OnDelete(DeleteBehavior.Cascade);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
#if DEBUG
            optionsBuilder.LogTo(Console.WriteLine, LogLevel.Information)
                         .EnableSensitiveDataLogging()
                         .EnableDetailedErrors();
#endif
        }
    }
}