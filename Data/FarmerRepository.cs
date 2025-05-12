using AgriEnergyConnect.Models;
using Microsoft.EntityFrameworkCore;

namespace AgriEnergyConnect.Data
{
    // Data/FarmerRepository.cs

    /// <summary>
    /// Repository for accessing and managing Farmer entities in the database.
    /// Inherits generic CRUD operations from Repository<Farmer>.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the FarmerRepository with the specified context.
    /// </remarks>
    /// <param name="context">The application's database context.</param>
    public class FarmerRepository(AppDbContext context) : Repository<Farmer>(context), IFarmerRepository
    {

        /// <summary>
        /// Retrieves all farmers along with their associated products.
        /// </summary>
        /// <returns>A list of farmers with their products included.</returns>
        public async Task<IEnumerable<Farmer>> GetFarmersWithProductsAsync()
        {
            return await _context.Set<Farmer>()
                .Include(f => f.Products)
                .ToListAsync();
        }
    }

    /// <summary>
    /// Interface for FarmerRepository, providing methods for accessing Farmer data.
    /// </summary>
    public interface IFarmerRepository : IRepository<Farmer>
    {
        /// <summary>
        /// Retrieves all farmers with their products.
        /// </summary>
        Task<IEnumerable<Farmer>> GetFarmersWithProductsAsync();
    }
}