using Microsoft.EntityFrameworkCore;
using ManufacturerPhoneApp.Data;
using ManufacturerPhoneApp.Models;

namespace ManufacturerPhoneApp.Repositories
{
    /// <summary>
    /// Repository class for phone-specific operations.
    /// </summary>
    public class PhoneRepository : Repository<Phone>, IPhoneRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PhoneRepository"/> class.
        /// </summary>
        /// <param name="context">The database context.</param>
        public PhoneRepository(ManufacturerPhoneContext context) : base(context)
        {
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Phone>> GetAllWithManufacturersAsync()
        {
            return await _dbSet
                .Include(p => p.Manufacturer)
                .OrderBy(p => p.Model)
                .ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Phone>> GetByManufacturerIdAsync(int manufacturerId)
        {
            return await _dbSet
                .Include(p => p.Manufacturer)
                .Where(p => p.ManufacturerId == manufacturerId)
                .OrderBy(p => p.Model)
                .ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<Phone?> GetBySerialNumberAsync(string serialNumber)
        {
            if (string.IsNullOrWhiteSpace(serialNumber))
                return null;

            return await _dbSet
                .Include(p => p.Manufacturer)
                .FirstOrDefaultAsync(p => p.SerialNumber == serialNumber);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Phone>> GetByModelAsync(string model)
        {
            if (string.IsNullOrWhiteSpace(model))
                return Enumerable.Empty<Phone>();

            return await _dbSet
                .Include(p => p.Manufacturer)
                .Where(p => p.Model.Contains(model))
                .OrderBy(p => p.Model)
                .ToListAsync();
        }

        /// <inheritdoc/>
        public override async Task<IEnumerable<Phone>> GetAllAsync()
        {
            return await _dbSet
                .Include(p => p.Manufacturer)
                .OrderBy(p => p.Model)
                .ToListAsync();
        }

        /// <inheritdoc/>
        public override async Task<Phone?> GetByIdAsync(int id)
        {
            return await _dbSet
                .Include(p => p.Manufacturer)
                .FirstOrDefaultAsync(p => p.Id == id);
        }
    }
}
