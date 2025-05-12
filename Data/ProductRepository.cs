using AgriEnergyConnect.Models;
using Microsoft.EntityFrameworkCore;

namespace AgriEnergyConnect.Data
{
    public class ProductRepository(AppDbContext context) : Repository<Product>(context), IProductRepository
    {
        public async Task<IEnumerable<Product>> GetFilteredProductsAsync(
            int? farmerId,
            string? category,
            DateTime? startDate,
            DateTime? endDate)
        {
            // Start with base query including Farmer relationship
            var query = _dbSet
                .Include(p => p.Farmer)
                .AsQueryable();

            // Apply farmer filter if specified
            if (farmerId.HasValue)
            {
                query = query.Where(p => p.FarmerID == farmerId.Value);
            }

            // Apply category filter if specified (case-insensitive partial match)
            if (!string.IsNullOrWhiteSpace(category))
            {
                query = query.Where(p =>
                    EF.Functions.Like(p.Category, $"%{category}%"));
            }

            // Apply date range filters if specified
            if (startDate.HasValue)
            {
                query = query.Where(p => p.ProductionDate >= startDate.Value.Date);
            }

            if (endDate.HasValue)
            {
                query = query.Where(p => p.ProductionDate <= endDate.Value.Date);
            }

            // Execute query and return results
            return await query
                .OrderByDescending(p => p.ProductionDate) // Newest first
                .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProductsByCategoryAsync(string category)
        {
            return await _dbSet
                .Where(p => p.Category == category)
                .Include(p => p.Farmer) // Include farmer details
                .ToListAsync();
        }
    }

    public interface IProductRepository : IRepository<Product>
    {
        Task<IEnumerable<Product>> GetProductsByCategoryAsync(string category);
        Task<IEnumerable<Product>> GetFilteredProductsAsync(
            int? farmerId,
            string? category,
            DateTime? startDate,
            DateTime? endDate);
    }
}