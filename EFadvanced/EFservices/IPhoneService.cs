using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PhoneInheritanceDemo.Models;

namespace PhoneInheritanceDemo.Services
{
    /// <summary>
    /// Defines the contract for phone service operations.
    /// Provides methods for managing phones and manufacturers using different inheritance strategies.
    /// </summary>
    public interface IPhoneService : IDisposable
    {
        /// <summary>
        /// Gets the name of the inheritance strategy being used.
        /// </summary>
        string StrategyName { get; }

        /// <summary>
        /// Ensures that the database for the context exists.
        /// If it exists, no action is taken. If it does not exist, 
        /// the database and all its schema are created.
        /// </summary>
        /// <returns>A task representing the asynchronous operation</returns>
        Task EnsureDatabaseCreatedAsync();

        /// <summary>
        /// Gets all manufacturers from the database.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation containing all manufacturers</returns>
        Task<IEnumerable<Manufacturer>> GetManufacturersAsync();

        /// <summary>
        /// Gets all phones from the database.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation containing all phones</returns>
        Task<IEnumerable<Phone>> GetPhonesAsync();

        /// <summary>
        /// Gets phones of a specific type.
        /// </summary>
        /// <typeparam name="T">The type of phone to retrieve</typeparam>
        /// <returns>A task that represents the asynchronous operation containing phones of the specified type</returns>
        Task<IEnumerable<T>> GetPhonesByTypeAsync<T>() where T : Phone;

        /// <summary>
        /// Gets a phone by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the phone</param>
        /// <returns>A task that represents the asynchronous operation containing the phone or null if not found</returns>
        Task<Phone?> GetPhoneByIdAsync(int id);

        /// <summary>
        /// Adds a new manufacturer to the database.
        /// </summary>
        /// <param name="manufacturer">The manufacturer to add</param>
        /// <returns>A task that represents the asynchronous operation containing the added manufacturer</returns>
        Task<Manufacturer> AddManufacturerAsync(Manufacturer manufacturer);

        /// <summary>
        /// Adds a new phone to the database.
        /// </summary>
        /// <param name="phone">The phone to add</param>
        /// <returns>A task that represents the asynchronous operation containing the added phone</returns>
        Task<Phone> AddPhoneAsync(Phone phone);

        /// <summary>
        /// Seeds the database with sample data if it's empty.
        /// </summary>
        /// <returns>A task representing the asynchronous operation</returns>
        Task SeedDataAsync();
    }
}