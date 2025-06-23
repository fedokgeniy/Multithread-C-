using Microsoft.EntityFrameworkCore;
using ManufacturerPhoneApp.Data;
using ManufacturerPhoneApp.Models;

namespace ManufacturerPhoneApp.Repositories
{
    /// <summary>
    /// Repository class for manufacturer-specific operations.
    /// </summary>
    public class ManufacturerRepository : Repository<Manufacturer>, IManufacturerRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ManufacturerRepository"/> class.
        /// </summary>
        /// <param name="context">The database context.</param>
        public ManufacturerRepository(ManufacturerPhoneContext context) : base(context)
        {
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Manufacturer>> GetAllWithPhonesAsync()
        {
            return await _dbSet
                .Include(m => m.Phones)
                .ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<Manufacturer?> GetByIdWithPhonesAsync(int id)
        {
            return await _dbSet
                .Include(m => m.Phones)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Manufacturer>> GetByNameAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return Enumerable.Empty<Manufacturer>();

            return await _dbSet
                .Where(m => m.Name.Contains(name))
                .ToListAsync();
        }

        /// <inheritdoc/>
        public override async Task<IEnumerable<Manufacturer>> GetAllAsync()
        {
            return await _dbSet
                .OrderBy(m => m.Name)
                .ToListAsync();
        }
    }
}
