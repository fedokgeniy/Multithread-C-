using Microsoft.EntityFrameworkCore;
using PhoneInheritanceDemo.Data;
using PhoneInheritanceDemo.Models;

namespace PhoneInheritanceDemo.Services;

/// <summary>
/// Base implementation of phone service with common functionality.
/// </summary>
/// <typeparam name="TContext">The DbContext type.</typeparam>
public abstract class BasePhoneService<TContext> : IPhoneService where TContext : BasePhoneContext
{
    protected readonly TContext _context;

    /// <summary>
    /// Initializes a new instance of the BasePhoneService class.
    /// </summary>
    /// <param name="context">The database context.</param>
    protected BasePhoneService(TContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Adds a new manufacturer to the database.
    /// </summary>
    /// <param name="manufacturer">The manufacturer to add.</param>
    /// <returns>The added manufacturer with generated ID.</returns>
    public virtual async Task<Manufacturer> AddManufacturerAsync(Manufacturer manufacturer)
    {
        _context.Manufacturers.Add(manufacturer);
        await _context.SaveChangesAsync();
        return manufacturer;
    }

    /// <summary>
    /// Adds a new phone to the database.
    /// </summary>
    /// <param name="phone">The phone to add.</param>
    /// <returns>The added phone with generated ID.</returns>
    public virtual async Task<Phone> AddPhoneAsync(Phone phone)
    {
        _context.Phones.Add(phone);
        await _context.SaveChangesAsync();
        return phone;
    }

    /// <summary>
    /// Gets all manufacturers from the database.
    /// </summary>
    /// <returns>List of all manufacturers.</returns>
    public virtual async Task<List<Manufacturer>> GetAllManufacturersAsync()
    {
        return await _context.Manufacturers.ToListAsync();
    }

    /// <summary>
    /// Gets all phones from the database.
    /// </summary>
    /// <returns>List of all phones.</returns>
    public virtual async Task<List<Phone>> GetAllPhonesAsync()
    {
        return await _context.Phones.Include(p => p.Manufacturer).ToListAsync();
    }

    /// <summary>
    /// Gets phones by type.
    /// </summary>
    /// <typeparam name="T">The phone type.</typeparam>
    /// <returns>List of phones of the specified type.</returns>
    public virtual async Task<List<T>> GetPhonesByTypeAsync<T>() where T : Phone
    {
        return await _context.Set<T>().Include(p => p.Manufacturer).ToListAsync();
    }

    /// <summary>
    /// Gets a phone by its ID.
    /// </summary>
    /// <param name="id">The phone ID.</param>
    /// <returns>The phone if found, null otherwise.</returns>
    public virtual async Task<Phone?> GetPhoneByIdAsync(int id)
    {
        return await _context.Phones.Include(p => p.Manufacturer)
                                   .FirstOrDefaultAsync(p => p.Id == id);
    }

    /// <summary>
    /// Updates an existing phone.
    /// </summary>
    /// <param name="phone">The phone to update.</param>
    /// <returns>The updated phone.</returns>
    public virtual async Task<Phone> UpdatePhoneAsync(Phone phone)
    {
        _context.Phones.Update(phone);
        await _context.SaveChangesAsync();
        return phone;
    }

    /// <summary>
    /// Deletes a phone by its ID.
    /// </summary>
    /// <param name="id">The phone ID to delete.</param>
    /// <returns>True if deleted successfully, false otherwise.</returns>
    public virtual async Task<bool> DeletePhoneAsync(int id)
    {
        var phone = await _context.Phones.FindAsync(id);
        if (phone == null)
            return false;

        _context.Phones.Remove(phone);
        await _context.SaveChangesAsync();
        return true;
    }

    /// <summary>
    /// Seeds the database with sample data.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    public virtual async Task SeedDataAsync()
    {
        // Check if data already exists
        if (await _context.Manufacturers.AnyAsync())
            return;

        // Add manufacturers
        var manufacturers = new List<Manufacturer>
        {
            new Manufacturer
            {
                Name = "Apple Inc.",
                Address = "One Apple Park Way, Cupertino, CA 95014, USA",
                IsAChildCompany = false
            },
            new Manufacturer
            {
                Name = "Samsung Electronics",
                Address = "Samsung Digital City, Yeongtong-gu, Suwon-si, South Korea",
                IsAChildCompany = false
            },
            new Manufacturer
            {
                Name = "Nokia Corporation",
                Address = "Karaportti 3, 02610 Espoo, Finland",
                IsAChildCompany = false
            },
            new Manufacturer
            {
                Name = "ASUS ROG",
                Address = "15 Biopolis Way, Singapore 138669",
                IsAChildCompany = true
            }
        };

        _context.Manufacturers.AddRange(manufacturers);
        await _context.SaveChangesAsync();

        // Add phones
        var phones = new List<Phone>
        {
            new FeaturePhone
            {
                Model = "Nokia 3310",
                SerialNumber = "NK3310001",
                ManufacturerId = manufacturers[2].Id,
                BatteryCapacity = 1200,
                ScreenSize = 2.4m,
                HasPhysicalKeypad = true,
                SmsStorageCapacity = 500,
                SupportsBasicGames = true
            },
            new Smartphone
            {
                Model = "iPhone 15 Pro",
                SerialNumber = "IP15P001",
                ManufacturerId = manufacturers[0].Id,
                BatteryCapacity = 3274,
                ScreenSize = 6.1m,
                OperatingSystem = "iOS",
                RamCapacity = 8,
                StorageCapacity = 256,
                CameraResolution = 48.0m,
                Supports5G = true
            },
            new Smartphone
            {
                Model = "Galaxy S24 Ultra",
                SerialNumber = "GS24U001",
                ManufacturerId = manufacturers[1].Id,
                BatteryCapacity = 5000,
                ScreenSize = 6.8m,
                OperatingSystem = "Android",
                RamCapacity = 12,
                StorageCapacity = 512,
                CameraResolution = 200.0m,
                Supports5G = true
            },
            new GamingPhone
            {
                Model = "ROG Phone 8 Pro",
                SerialNumber = "ROG8P001",
                ManufacturerId = manufacturers[3].Id,
                BatteryCapacity = 6000,
                ScreenSize = 6.78m,
                OperatingSystem = "Android",
                RamCapacity = 24,
                StorageCapacity = 1024,
                GpuModel = "Adreno 750",
                RefreshRate = 165,
                HasGamingTriggers = true,
                CoolingSystem = "GameCool 8 Cooling System"
            }
        };

        _context.Phones.AddRange(phones);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Gets the strategy name.
    /// </summary>
    /// <returns>The inheritance strategy name.</returns>
    public abstract string GetStrategyName();
}