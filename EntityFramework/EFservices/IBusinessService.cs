using ManufacturerPhoneApp.Models;

namespace ManufacturerPhoneApp.Services
{
    /// <summary>
    /// Interface for business operations.
    /// </summary>
    public interface IBusinessService
    {
        /// <summary>
        /// Adds a new product for a new manufacturer using transaction.
        /// </summary>
        /// <param name="manufacturer">The manufacturer to add.</param>
        /// <param name="phone">The phone to add.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task AddProductForNewManufacturerAsync(Manufacturer manufacturer, Phone phone);

        /// <summary>
        /// Gets all products for a specific manufacturer.
        /// </summary>
        /// <param name="manufacturerId">The manufacturer ID.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the phones.</returns>
        Task<IEnumerable<Phone>> GetProductsForManufacturerAsync(int manufacturerId);

        /// <summary>
        /// Gets all products for a specific manufacturer by name.
        /// </summary>
        /// <param name="manufacturerName">The manufacturer name.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the phones.</returns>
        Task<IEnumerable<Phone>> GetProductsForManufacturerAsync(string manufacturerName);
    }
}
