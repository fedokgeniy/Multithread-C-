using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ManufacturerPhoneApp.Interfaces;
using ManufacturerPhoneApp.Models;

namespace ManufacturerPhoneApp.Services
{
    /// <summary>
    /// Service for phone business logic operations.
    /// </summary>
    public class PhoneService
    {
        private readonly IPhoneRepository _phoneRepository;
        private readonly IManufacturerRepository _manufacturerRepository;

        /// <summary>
        /// Initializes a new instance of the PhoneService class.
        /// </summary>
        /// <param name="phoneRepository">The phone repository.</param>
        /// <param name="manufacturerRepository">The manufacturer repository.</param>
        public PhoneService(IPhoneRepository phoneRepository, IManufacturerRepository manufacturerRepository)
        {
            _phoneRepository = phoneRepository ?? throw new ArgumentNullException(nameof(phoneRepository));
            _manufacturerRepository = manufacturerRepository ?? throw new ArgumentNullException(nameof(manufacturerRepository));
        }

        /// <summary>
        /// Gets all phones asynchronously.
        /// </summary>
        /// <returns>A list of all phones.</returns>
        public async Task<List<Phone>> GetAllPhonesAsync()
        {
            return await _phoneRepository.GetAllAsync();
        }

        /// <summary>
        /// Gets a phone by ID asynchronously.
        /// </summary>
        /// <param name="id">The phone ID.</param>
        /// <returns>The phone if found, otherwise null.</returns>
        public async Task<Phone?> GetPhoneByIdAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Phone ID must be greater than zero.", nameof(id));
            }

            return await _phoneRepository.GetByIdAsync(id);
        }

        /// <summary>
        /// Gets all phones for a specific manufacturer asynchronously.
        /// </summary>
        /// <param name="manufacturerId">The manufacturer ID.</param>
        /// <returns>A list of phones for the specified manufacturer.</returns>
        public async Task<List<Phone>> GetPhonesByManufacturerIdAsync(int manufacturerId)
        {
            if (manufacturerId <= 0)
            {
                throw new ArgumentException("Manufacturer ID must be greater than zero.", nameof(manufacturerId));
            }

            // Verify manufacturer exists
            var manufacturer = await _manufacturerRepository.GetByIdAsync(manufacturerId);
            if (manufacturer == null)
            {
                throw new ArgumentException($"Manufacturer with ID {manufacturerId} not found.", nameof(manufacturerId));
            }

            return await _phoneRepository.GetByManufacturerIdAsync(manufacturerId);
        }

        /// <summary>
        /// Creates a new phone asynchronously.
        /// </summary>
        /// <param name="model">The phone model.</param>
        /// <param name="serialNumber">The serial number.</param>
        /// <param name="phoneType">The phone type.</param>
        /// <param name="manufacturerId">The manufacturer ID.</param>
        /// <returns>The ID of the newly created phone.</returns>
        public async Task<int> CreatePhoneAsync(string model, string serialNumber, string phoneType, int manufacturerId)
        {
            await ValidatePhoneInputAsync(model, serialNumber, phoneType, manufacturerId);

            var phone = new Phone
            {
                Model = model.Trim(),
                SerialNumber = serialNumber.Trim(),
                PhoneType = phoneType.Trim(),
                ManufacturerId = manufacturerId
            };

            return await _phoneRepository.AddAsync(phone);
        }

        /// <summary>
        /// Updates an existing phone asynchronously.
        /// </summary>
        /// <param name="id">The phone ID.</param>
        /// <param name="model">The phone model.</param>
        /// <param name="serialNumber">The serial number.</param>
        /// <param name="phoneType">The phone type.</param>
        /// <param name="manufacturerId">The manufacturer ID.</param>
        /// <returns>True if updated successfully, otherwise false.</returns>
        public async Task<bool> UpdatePhoneAsync(int id, string model, string serialNumber, string phoneType, int manufacturerId)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Phone ID must be greater than zero.", nameof(id));
            }

            await ValidatePhoneInputAsync(model, serialNumber, phoneType, manufacturerId);

            var phone = new Phone
            {
                Id = id,
                Model = model.Trim(),
                SerialNumber = serialNumber.Trim(),
                PhoneType = phoneType.Trim(),
                ManufacturerId = manufacturerId
            };

            return await _phoneRepository.UpdateAsync(phone);
        }

        /// <summary>
        /// Deletes a phone by ID asynchronously.
        /// </summary>
        /// <param name="id">The phone ID to delete.</param>
        /// <returns>True if deleted successfully, otherwise false.</returns>
        public async Task<bool> DeletePhoneAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Phone ID must be greater than zero.", nameof(id));
            }

            return await _phoneRepository.DeleteAsync(id);
        }

        /// <summary>
        /// Validates phone input data.
        /// </summary>
        /// <param name="model">The phone model.</param>
        /// <param name="serialNumber">The serial number.</param>
        /// <param name="phoneType">The phone type.</param>
        /// <param name="manufacturerId">The manufacturer ID.</param>
        private async Task ValidatePhoneInputAsync(string model, string serialNumber, string phoneType, int manufacturerId)
        {
            if (string.IsNullOrWhiteSpace(model))
            {
                throw new ArgumentException("Phone model cannot be empty.", nameof(model));
            }

            if (string.IsNullOrWhiteSpace(serialNumber))
            {
                throw new ArgumentException("Serial number cannot be empty.", nameof(serialNumber));
            }

            if (string.IsNullOrWhiteSpace(phoneType))
            {
                throw new ArgumentException("Phone type cannot be empty.", nameof(phoneType));
            }

            if (manufacturerId <= 0)
            {
                throw new ArgumentException("Manufacturer ID must be greater than zero.", nameof(manufacturerId));
            }

            if (model.Trim().Length > 255)
            {
                throw new ArgumentException("Phone model cannot exceed 255 characters.", nameof(model));
            }

            if (serialNumber.Trim().Length > 255)
            {
                throw new ArgumentException("Serial number cannot exceed 255 characters.", nameof(serialNumber));
            }

            if (phoneType.Trim().Length > 100)
            {
                throw new ArgumentException("Phone type cannot exceed 100 characters.", nameof(phoneType));
            }

            // Verify manufacturer exists
            var manufacturer = await _manufacturerRepository.GetByIdAsync(manufacturerId);
            if (manufacturer == null)
            {
                throw new ArgumentException($"Manufacturer with ID {manufacturerId} not found.", nameof(manufacturerId));
            }
        }
    }
}
