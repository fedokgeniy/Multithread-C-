using ManufacturerPhoneApp.Models;

namespace ManufacturerPhoneApp.Repositories
{
    /// <summary>
    /// Repository interface for phone-specific operations.
    /// </summary>
    public interface IPhoneRepository : IRepository<Phone>
    {
        /// <summary>
        /// Gets all phones with their associated manufacturers.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains the list of phones with manufacturers.</returns>
        Task<IEnumerable<Phone>> GetAllWithManufacturersAsync();

        /// <summary>
        /// Gets all phones for a specific manufacturer.
        /// </summary>
        /// <param name="manufacturerId">The manufacturer ID.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the phones for the manufacturer.</returns>
        Task<IEnumerable<Phone>> GetByManufacturerIdAsync(int manufacturerId);

        /// <summary>
        /// Gets a phone by serial number.
        /// </summary>
        /// <param name="serialNumber">The phone serial number.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the phone.</returns>
        Task<Phone?> GetBySerialNumberAsync(string serialNumber);

        /// <summary>
        /// Gets phones by model (partial match).
        /// </summary>
        /// <param name="model">The phone model to search for.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the matching phones.</returns>
        Task<IEnumerable<Phone>> GetByModelAsync(string model);
    }
}
