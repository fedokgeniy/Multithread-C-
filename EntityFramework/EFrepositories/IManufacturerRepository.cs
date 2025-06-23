using ManufacturerPhoneApp.Models;

namespace ManufacturerPhoneApp.Repositories
{
    /// <summary>
    /// Repository interface for manufacturer-specific operations.
    /// </summary>
    public interface IManufacturerRepository : IRepository<Manufacturer>
    {
        /// <summary>
        /// Gets all manufacturers with their associated phones.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains the list of manufacturers with phones.</returns>
        Task<IEnumerable<Manufacturer>> GetAllWithPhonesAsync();

        /// <summary>
        /// Gets a manufacturer by ID with associated phones.
        /// </summary>
        /// <param name="id">The manufacturer ID.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the manufacturer with phones.</returns>
        Task<Manufacturer?> GetByIdWithPhonesAsync(int id);

        /// <summary>
        /// Gets manufacturers by name (partial match).
        /// </summary>
        /// <param name="name">The manufacturer name to search for.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the matching manufacturers.</returns>
        Task<IEnumerable<Manufacturer>> GetByNameAsync(string name);
    }
}
