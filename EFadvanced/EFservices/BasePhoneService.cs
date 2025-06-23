using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PhoneInheritanceDemo.Data;
using PhoneInheritanceDemo.Models;

namespace PhoneInheritanceDemo.Services
{
    /// <summary>
    /// Base implementation of phone service providing common functionality
    /// for all inheritance strategy implementations.
    /// </summary>
    /// <typeparam name="TContext">The type of database context</typeparam>
    public abstract class BasePhoneService<TContext> : IPhoneService
        where TContext : BasePhoneContext
    {
        protected readonly TContext _context;

        /// <summary>
        /// Initializes a new instance of the BasePhoneService class.
        /// </summary>
        /// <param name="context">The database context to use</param>
        protected BasePhoneService(TContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <summary>
        /// Ensures that the database for the context exists.
        /// If it exists, no action is taken. If it does not exist, 
        /// the database and all its schema are created.
        /// </summary>
        public virtual async Task EnsureDatabaseCreatedAsync()
        {
            await _context.Database.EnsureCreatedAsync();
        }

        /// <summary>
        /// Gets the name of the inheritance strategy being used.
        /// </summary>
        public abstract string StrategyName { get; }

        /// <summary>
        /// Gets all manufacturers from the database.
        /// </summary>
        /// <returns>A collection of all manufacturers</returns>
        public virtual async Task<IEnumerable<Manufacturer>> GetManufacturersAsync()
        {
            return await _context.Manufacturers.ToListAsync();
        }

        /// <summary>
        /// Gets all phones from the database.
        /// </summary>
        /// <returns>A collection of all phones</returns>
        public virtual async Task<IEnumerable<Phone>> GetPhonesAsync()
        {
            return await _context.Phones
                .Include(p => p.Manufacturer)
                .ToListAsync();
        }

        /// <summary>
        /// Gets phones of a specific type.
        /// </summary>
        /// <typeparam name="T">The type of phone to retrieve</typeparam>
        /// <returns>A collection of phones of the specified type</returns>
        public virtual async Task<IEnumerable<T>> GetPhonesByTypeAsync<T>() where T : Phone
        {
            return await _context.Phones
                .OfType<T>()
                .Include(p => p.Manufacturer)
                .ToListAsync();
        }

        /// <summary>
        /// Gets a phone by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the phone</param>
        /// <returns>The phone with the specified ID, or null if not found</returns>
        public virtual async Task<Phone?> GetPhoneByIdAsync(int id)
        {
            return await _context.Phones
                .Include(p => p.Manufacturer)
                .FirstOrDefaultAsync(p => p.PhoneId == id);
        }

        /// <summary>
        /// Adds a new manufacturer to the database.
        /// </summary>
        /// <param name="manufacturer">The manufacturer to add</param>
        /// <returns>The added manufacturer with generated ID</returns>
        public virtual async Task<Manufacturer> AddManufacturerAsync(Manufacturer manufacturer)
        {
            if (manufacturer == null)
                throw new ArgumentNullException(nameof(manufacturer));

            _context.Manufacturers.Add(manufacturer);
            await _context.SaveChangesAsync();
            return manufacturer;
        }

        /// <summary>
        /// Adds a new phone to the database.
        /// </summary>
        /// <param name="phone">The phone to add</param>
        /// <returns>The added phone with generated ID</returns>
        public virtual async Task<Phone> AddPhoneAsync(Phone phone)
        {
            if (phone == null)
                throw new ArgumentNullException(nameof(phone));

            _context.Phones.Add(phone);
            await _context.SaveChangesAsync();
            return phone;
        }

        /// <summary>
        /// Seeds the database with sample data if it's empty.
        /// </summary>
        public virtual async Task SeedDataAsync()
        {
            if (await _context.Manufacturers.AnyAsync())
                return;

            var manufacturers = new[]
            {
                new Manufacturer { Name = "Apple", Country = "USA", FoundedYear = 1976 },
                new Manufacturer { Name = "Samsung", Country = "South Korea", FoundedYear = 1938 },
                new Manufacturer { Name = "Nokia", Country = "Finland", FoundedYear = 1865 },
                new Manufacturer { Name = "ASUS ROG", Country = "Taiwan", FoundedYear = 1989 }
            };

            _context.Manufacturers.AddRange(manufacturers);
            await _context.SaveChangesAsync();

            var phones = new Phone[]
            {
                new Smartphone
                {
                    Model = "iPhone 15 Pro",
                    SerialNumber = "APL-15PRO-001",
                    BatteryCapacity = 3274,
                    ScreenSize = 6.1m,
                    Price = 1199.99m,
                    ManufacturerId = manufacturers[0].ManufacturerId,
                    OperatingSystem = "iOS 17",
                    RamSize = 8,
                    StorageSize = 256,
                    CameraResolution = 48,
                    HasFiveG = true
                },
                new Smartphone
                {
                    Model = "Galaxy S24 Ultra",
                    SerialNumber = "SAM-S24U-001",
                    BatteryCapacity = 5000,
                    ScreenSize = 6.8m,
                    Price = 1299.99m,
                    ManufacturerId = manufacturers[1].ManufacturerId,
                    OperatingSystem = "Android 14",
                    RamSize = 12,
                    StorageSize = 512,
                    CameraResolution = 200,
                    HasFiveG = true
                },
                new FeaturePhone
                {
                    Model = "Nokia 3310",
                    SerialNumber = "NOK-3310-001",
                    BatteryCapacity = 1200,
                    ScreenSize = 2.4m,
                    Price = 59.99m,
                    ManufacturerId = manufacturers[2].ManufacturerId,
                    HasPhysicalKeyboard = true,
                    SmsStorageCapacity = 300,
                    HasBasicGames = true
                },
                new GamingPhone
                {
                    Model = "ROG Phone 8",
                    SerialNumber = "ASUS-ROG8-001",
                    BatteryCapacity = 6000,
                    ScreenSize = 6.78m,
                    Price = 1099.99m,
                    ManufacturerId = manufacturers[3].ManufacturerId,
                    GpuName = "Adreno 750",
                    RefreshRate = 165,
                    HasGamingTriggers = true,
                    CoolingSystem = "Vapor Chamber"
                }
            };

            _context.Phones.AddRange(phones);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Disposes the database context.
        /// </summary>
        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}