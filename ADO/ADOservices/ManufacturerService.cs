using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ManufacturerPhoneApp.Interfaces;
using ManufacturerPhoneApp.Models;

namespace ManufacturerPhoneApp.Services
{
    /// <summary>
    /// Service for manufacturer business logic operations.
    /// </summary>
    public class ManufacturerService
    {
        private readonly IManufacturerRepository _manufacturerRepository;

        /// <summary>
        /// Initializes a new instance of the ManufacturerService class.
        /// </summary>
        /// <param name="manufacturerRepository">The manufacturer repository.</param>
        public ManufacturerService(IManufacturerRepository manufacturerRepository)
        {
            _manufacturerRepository = manufacturerRepository ?? throw new ArgumentNullException(nameof(manufacturerRepository));
        }

        /// <summary>
        /// Gets all manufacturers asynchronously.
        /// </summary>
        /// <returns>A list of all manufacturers.</returns>
        public async Task<List<Manufacturer>> GetAllManufacturersAsync()
        {
            return await _manufacturerRepository.GetAllAsync();
        }

        /// <summary>
        /// Gets a manufacturer by ID asynchronously.
        /// </summary>
        /// <param name="id">The manufacturer ID.</param>
        /// <returns>The manufacturer if found, otherwise null.</returns>
        public async Task<Manufacturer?> GetManufacturerByIdAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Manufacturer ID must be greater than zero.", nameof(id));
            }

            return await _manufacturerRepository.GetByIdAsync(id);
        }

        /// <summary>
        /// Creates a new manufacturer asynchronously.
        /// </summary>
        /// <param name="name">The manufacturer name.</param>
        /// <param name="address">The manufacturer address.</param>
        /// <param name="isAChildCompany">Whether the manufacturer is a child company.</param>
        /// <returns>The ID of the newly created manufacturer.</returns>
        public async Task<int> CreateManufacturerAsync(string name, string address, bool isAChildCompany)
        {
            ValidateManufacturerInput(name, address);

            var manufacturer = new Manufacturer
            {
                Name = name.Trim(),
                Address = address.Trim(),
                IsAChildCompany = isAChildCompany
            };

            return await _manufacturerRepository.AddAsync(manufacturer);
        }

        /// <summary>
        /// Updates an existing manufacturer asynchronously.
        /// </summary>
        /// <param name="id">The manufacturer ID.</param>
        /// <param name="name">The manufacturer name.</param>
        /// <param name="address">The manufacturer address.</param>
        /// <param name="isAChildCompany">Whether the manufacturer is a child company.</param>
        /// <returns>True if updated successfully, otherwise false.</returns>
        public async Task<bool> UpdateManufacturerAsync(int id, string name, string address, bool isAChildCompany)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Manufacturer ID must be greater than zero.", nameof(id));
            }

            ValidateManufacturerInput(name, address);

            var manufacturer = new Manufacturer
            {
                Id = id,
                Name = name.Trim(),
                Address = address.Trim(),
                IsAChildCompany = isAChildCompany
            };

            return await _manufacturerRepository.UpdateAsync(manufacturer);
        }

        /// <summary>
        /// Deletes a manufacturer by ID asynchronously.
        /// </summary>
        /// <param name="id">The manufacturer ID to delete.</param>
        /// <returns>True if deleted successfully, otherwise false.</returns>
        public async Task<bool> DeleteManufacturerAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Manufacturer ID must be greater than zero.", nameof(id));
            }

            return await _manufacturerRepository.DeleteAsync(id);
        }

        /// <summary>
        /// Validates manufacturer input data.
        /// </summary>
        /// <param name="name">The manufacturer name.</param>
        /// <param name="address">The manufacturer address.</param>
        private static void ValidateManufacturerInput(string name, string address)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Manufacturer name cannot be empty.", nameof(name));
            }

            if (string.IsNullOrWhiteSpace(address))
            {
                throw new ArgumentException("Manufacturer address cannot be empty.", nameof(address));
            }

            if (name.Trim().Length > 255)
            {
                throw new ArgumentException("Manufacturer name cannot exceed 255 characters.", nameof(name));
            }

            if (address.Trim().Length > 500)
            {
                throw new ArgumentException("Manufacturer address cannot exceed 500 characters.", nameof(address));
            }
        }
    }
}
