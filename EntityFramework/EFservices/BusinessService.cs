using Microsoft.EntityFrameworkCore;
using ManufacturerPhoneApp.Data;
using ManufacturerPhoneApp.Models;
using ManufacturerPhoneApp.Repositories;

namespace ManufacturerPhoneApp.Services
{
    /// <summary>
    /// Business service for complex operations involving multiple entities.
    /// </summary>
    public class BusinessService : IBusinessService
    {
        private readonly ManufacturerPhoneContext _context;
        private readonly IManufacturerRepository _manufacturerRepository;
        private readonly IPhoneRepository _phoneRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="BusinessService"/> class.
        /// </summary>
        /// <param name="context">The database context.</param>
        /// <param name="manufacturerRepository">The manufacturer repository.</param>
        /// <param name="phoneRepository">The phone repository.</param>
        public BusinessService(
            ManufacturerPhoneContext context,
            IManufacturerRepository manufacturerRepository,
            IPhoneRepository phoneRepository)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _manufacturerRepository = manufacturerRepository ?? throw new ArgumentNullException(nameof(manufacturerRepository));
            _phoneRepository = phoneRepository ?? throw new ArgumentNullException(nameof(phoneRepository));
        }

        /// <inheritdoc/>
        public async Task AddProductForNewManufacturerAsync(Manufacturer manufacturer, Phone phone)
        {
            if (manufacturer == null)
                throw new ArgumentNullException(nameof(manufacturer));

            if (phone == null)
                throw new ArgumentNullException(nameof(phone));

            // Start transaction
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // Add manufacturer first
                var addedManufacturer = await _manufacturerRepository.AddAsync(manufacturer);

                // Set the manufacturer ID for the phone
                phone.ManufacturerId = addedManufacturer.Id;

                // Add phone
                await _phoneRepository.AddAsync(phone);

                // Commit transaction
                await transaction.CommitAsync();

                Console.WriteLine($"Successfully added manufacturer '{manufacturer.Name}' and phone '{phone.Model}'");
            }
            catch (Exception ex)
            {
                // Rollback transaction on error
                await transaction.RollbackAsync();
                Console.WriteLine($"Transaction failed: {ex.Message}");
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Phone>> GetProductsForManufacturerAsync(int manufacturerId)
        {
            return await _phoneRepository.GetByManufacturerIdAsync(manufacturerId);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Phone>> GetProductsForManufacturerAsync(string manufacturerName)
        {
            if (string.IsNullOrWhiteSpace(manufacturerName))
                return Enumerable.Empty<Phone>();

            // Find manufacturer by name
            var manufacturers = await _manufacturerRepository.GetByNameAsync(manufacturerName);
            var manufacturer = manufacturers.FirstOrDefault();

            if (manufacturer == null)
                return Enumerable.Empty<Phone>();

            return await GetProductsForManufacturerAsync(manufacturer.Id);
        }
    }
}
