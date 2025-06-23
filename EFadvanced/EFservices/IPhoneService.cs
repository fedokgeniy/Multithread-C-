using PhoneInheritanceDemo.Models;

namespace PhoneInheritanceDemo.Services;

/// <summary>
/// Interface for phone management services.
/// </summary>
public interface IPhoneService
{
    /// <summary>
    /// Adds a new manufacturer to the database.
    /// </summary>
    /// <param name="manufacturer">The manufacturer to add.</param>
    /// <returns>The added manufacturer with generated ID.</returns>
    Task<Manufacturer> AddManufacturerAsync(Manufacturer manufacturer);

    /// <summary>
    /// Adds a new phone to the database.
    /// </summary>
    /// <param name="phone">The phone to add.</param>
    /// <returns>The added phone with generated ID.</returns>
    Task<Phone> AddPhoneAsync(Phone phone);

    /// <summary>
    /// Gets all manufacturers from the database.
    /// </summary>
    /// <returns>List of all manufacturers.</returns>
    Task<List<Manufacturer>> GetAllManufacturersAsync();

    /// <summary>
    /// Gets all phones from the database.
    /// </summary>
    /// <returns>List of all phones.</returns>
    Task<List<Phone>> GetAllPhonesAsync();

    /// <summary>
    /// Gets phones by type.
    /// </summary>
    /// <typeparam name="T">The phone type.</typeparam>
    /// <returns>List of phones of the specified type.</returns>
    Task<List<T>> GetPhonesByTypeAsync<T>() where T : Phone;

    /// <summary>
    /// Gets a phone by its ID.
    /// </summary>
    /// <param name="id">The phone ID.</param>
    /// <returns>The phone if found, null otherwise.</returns>
    Task<Phone?> GetPhoneByIdAsync(int id);

    /// <summary>
    /// Updates an existing phone.
    /// </summary>
    /// <param name="phone">The phone to update.</param>
    /// <returns>The updated phone.</returns>
    Task<Phone> UpdatePhoneAsync(Phone phone);

    /// <summary>
    /// Deletes a phone by its ID.
    /// </summary>
    /// <param name="id">The phone ID to delete.</param>
    /// <returns>True if deleted successfully, false otherwise.</returns>
    Task<bool> DeletePhoneAsync(int id);

    /// <summary>
    /// Seeds the database with sample data.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task SeedDataAsync();

    /// <summary>
    /// Gets the strategy name.
    /// </summary>
    /// <returns>The inheritance strategy name.</returns>
    string GetStrategyName();
}