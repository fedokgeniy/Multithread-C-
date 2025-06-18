using System.Collections.Generic;
using System.Threading.Tasks;
using ManufacturerPhoneApp.Models;

namespace ManufacturerPhoneApp.Interfaces
{
    /// <summary>
    /// Interface for phone repository operations.
    /// </summary>
    public interface IPhoneRepository
    {
        /// <summary>
        /// Gets all phones asynchronously.
        /// </summary>
        /// <returns>A list of all phones.</returns>
        Task<List<Phone>> GetAllAsync();

        /// <summary>
        /// Gets a phone by ID asynchronously.
        /// </summary>
        /// <param name="id">The phone ID.</param>
        /// <returns>The phone if found, otherwise null.</returns>
        Task<Phone?> GetByIdAsync(int id);

        /// <summary>
        /// Gets all phones for a specific manufacturer asynchronously.
        /// </summary>
        /// <param name="manufacturerId">The manufacturer ID.</param>
        /// <returns>A list of phones for the specified manufacturer.</returns>
        Task<List<Phone>> GetByManufacturerIdAsync(int manufacturerId);

        /// <summary>
        /// Adds a new phone asynchronously.
        /// </summary>
        /// <param name="phone">The phone to add.</param>
        /// <returns>The ID of the newly added phone.</returns>
        Task<int> AddAsync(Phone phone);

        /// <summary>
        /// Updates an existing phone asynchronously.
        /// </summary>
        /// <param name="phone">The phone to update.</param>
        /// <returns>True if updated successfully, otherwise false.</returns>
        Task<bool> UpdateAsync(Phone phone);

        /// <summary>
        /// Deletes a phone by ID asynchronously.
        /// </summary>
        /// <param name="id">The phone ID to delete.</param>
        /// <returns>True if deleted successfully, otherwise false.</returns>
        Task<bool> DeleteAsync(int id);
    }
}
