using System.Collections.Generic;
using System.Threading.Tasks;
using ManufacturerPhoneApp.Models;

namespace ManufacturerPhoneApp.Interfaces
{
    /// <summary>
    /// Interface for manufacturer repository operations.
    /// </summary>
    public interface IManufacturerRepository
    {
        /// <summary>
        /// Gets all manufacturers asynchronously.
        /// </summary>
        /// <returns>A list of all manufacturers.</returns>
        Task<List<Manufacturer>> GetAllAsync();

        /// <summary>
        /// Gets a manufacturer by ID asynchronously.
        /// </summary>
        /// <param name="id">The manufacturer ID.</param>
        /// <returns>The manufacturer if found, otherwise null.</returns>
        Task<Manufacturer?> GetByIdAsync(int id);

        /// <summary>
        /// Adds a new manufacturer asynchronously.
        /// </summary>
        /// <param name="manufacturer">The manufacturer to add.</param>
        /// <returns>The ID of the newly added manufacturer.</returns>
        Task<int> AddAsync(Manufacturer manufacturer);

        /// <summary>
        /// Updates an existing manufacturer asynchronously.
        /// </summary>
        /// <param name="manufacturer">The manufacturer to update.</param>
        /// <returns>True if updated successfully, otherwise false.</returns>
        Task<bool> UpdateAsync(Manufacturer manufacturer);

        /// <summary>
        /// Deletes a manufacturer by ID asynchronously.
        /// </summary>
        /// <param name="id">The manufacturer ID to delete.</param>
        /// <returns>True if deleted successfully, otherwise false.</returns>
        Task<bool> DeleteAsync(int id);
    }
}
