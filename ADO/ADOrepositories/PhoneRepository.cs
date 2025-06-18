using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using ManufacturerPhoneApp.Extensions;
using ManufacturerPhoneApp.Infrastructure;
using ManufacturerPhoneApp.Interfaces;
using ManufacturerPhoneApp.Models;

namespace ManufacturerPhoneApp.Repositories
{
    /// <summary>
    /// Repository for phone data operations.
    /// </summary>
    public class PhoneRepository : IPhoneRepository
    {
        private readonly DatabaseConnectionManager _connectionManager;

        /// <summary>
        /// Initializes a new instance of the PhoneRepository class.
        /// </summary>
        /// <param name="connectionManager">The database connection manager.</param>
        public PhoneRepository(DatabaseConnectionManager connectionManager)
        {
            _connectionManager = connectionManager ?? throw new ArgumentNullException(nameof(connectionManager));
        }

        /// <summary>
        /// Gets all phones asynchronously.
        /// </summary>
        /// <returns>A list of all phones.</returns>
        public async Task<List<Phone>> GetAllAsync()
        {
            var phones = new List<Phone>();

            try
            {
                using var connection = _connectionManager.CreateConnection();
                await connection.OpenAsync();

                var query = @"
                    SELECT p.Id, p.Model, p.SerialNumber, p.PhoneType, p.ManufacturerId,
                           m.Name as ManufacturerName, m.Address as ManufacturerAddress, m.IsAChildCompany
                    FROM Phones p
                    LEFT JOIN Manufacturers m ON p.ManufacturerId = m.Id
                    ORDER BY p.Model";

                using var command = new SqlCommand(query, connection);
                using var reader = await command.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    phones.Add(MapToPhone(reader));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting all phones: {ex.Message}");
                throw;
            }

            return phones;
        }

        /// <summary>
        /// Gets a phone by ID asynchronously.
        /// </summary>
        /// <param name="id">The phone ID.</param>
        /// <returns>The phone if found, otherwise null.</returns>
        public async Task<Phone?> GetByIdAsync(int id)
        {
            try
            {
                using var connection = _connectionManager.CreateConnection();
                await connection.OpenAsync();

                var query = @"
                    SELECT p.Id, p.Model, p.SerialNumber, p.PhoneType, p.ManufacturerId,
                           m.Name as ManufacturerName, m.Address as ManufacturerAddress, m.IsAChildCompany
                    FROM Phones p
                    LEFT JOIN Manufacturers m ON p.ManufacturerId = m.Id
                    WHERE p.Id = @Id";

                using var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Id", id);

                using var reader = await command.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    return MapToPhone(reader);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting phone by ID {id}: {ex.Message}");
                throw;
            }

            return null;
        }

        /// <summary>
        /// Gets all phones for a specific manufacturer asynchronously.
        /// </summary>
        /// <param name="manufacturerId">The manufacturer ID.</param>
        /// <returns>A list of phones for the specified manufacturer.</returns>
        public async Task<List<Phone>> GetByManufacturerIdAsync(int manufacturerId)
        {
            var phones = new List<Phone>();

            try
            {
                using var connection = _connectionManager.CreateConnection();
                await connection.OpenAsync();

                var query = @"
                    SELECT p.Id, p.Model, p.SerialNumber, p.PhoneType, p.ManufacturerId,
                           m.Name as ManufacturerName, m.Address as ManufacturerAddress, m.IsAChildCompany
                    FROM Phones p
                    LEFT JOIN Manufacturers m ON p.ManufacturerId = m.Id
                    WHERE p.ManufacturerId = @ManufacturerId
                    ORDER BY p.Model";

                using var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@ManufacturerId", manufacturerId);

                using var reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    phones.Add(MapToPhone(reader));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting phones for manufacturer {manufacturerId}: {ex.Message}");
                throw;
            }

            return phones;
        }

        /// <summary>
        /// Adds a new phone asynchronously.
        /// </summary>
        /// <param name="phone">The phone to add.</param>
        /// <returns>The ID of the newly added phone.</returns>
        public async Task<int> AddAsync(Phone phone)
        {
            if (phone == null)
                throw new ArgumentNullException(nameof(phone));

            try
            {
                using var connection = _connectionManager.CreateConnection();
                await connection.OpenAsync();

                var query = @"
                    INSERT INTO Phones (Model, SerialNumber, PhoneType, ManufacturerId) 
                    OUTPUT INSERTED.Id 
                    VALUES (@Model, @SerialNumber, @PhoneType, @ManufacturerId)";

                using var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Model", phone.Model);
                command.Parameters.AddWithValue("@SerialNumber", phone.SerialNumber);
                command.Parameters.AddWithValue("@PhoneType", phone.PhoneType);
                command.Parameters.AddWithValue("@ManufacturerId", phone.ManufacturerId);

                var result = await command.ExecuteScalarAsync();
                return (int)result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding phone: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Updates an existing phone asynchronously.
        /// </summary>
        /// <param name="phone">The phone to update.</param>
        /// <returns>True if updated successfully, otherwise false.</returns>
        public async Task<bool> UpdateAsync(Phone phone)
        {
            if (phone == null)
                throw new ArgumentNullException(nameof(phone));

            try
            {
                using var connection = _connectionManager.CreateConnection();
                await connection.OpenAsync();

                var query = @"
                    UPDATE Phones 
                    SET Model = @Model, SerialNumber = @SerialNumber, PhoneType = @PhoneType, ManufacturerId = @ManufacturerId 
                    WHERE Id = @Id";

                using var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Id", phone.Id);
                command.Parameters.AddWithValue("@Model", phone.Model);
                command.Parameters.AddWithValue("@SerialNumber", phone.SerialNumber);
                command.Parameters.AddWithValue("@PhoneType", phone.PhoneType);
                command.Parameters.AddWithValue("@ManufacturerId", phone.ManufacturerId);

                var rowsAffected = await command.ExecuteNonQueryAsync();
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating phone: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Deletes a phone by ID asynchronously.
        /// </summary>
        /// <param name="id">The phone ID to delete.</param>
        /// <returns>True if deleted successfully, otherwise false.</returns>
        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                using var connection = _connectionManager.CreateConnection();
                await connection.OpenAsync();

                var query = "DELETE FROM Phones WHERE Id = @Id";
                using var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Id", id);

                var rowsAffected = await command.ExecuteNonQueryAsync();
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting phone with ID {id}: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Maps a SqlDataReader row to a Phone object.
        /// </summary>
        /// <param name="reader">The SqlDataReader instance.</param>
        /// <returns>A Phone object.</returns>
        private static Phone MapToPhone(SqlDataReader reader)
        {
            var phone = new Phone
            {
                Id = reader.GetInt32("Id"),
                Model = reader.GetStringOrEmpty("Model"),
                SerialNumber = reader.GetStringOrEmpty("SerialNumber"),
                PhoneType = reader.GetStringOrEmpty("PhoneType"),
                ManufacturerId = reader.GetInt32("ManufacturerId")
            };

            // Check if manufacturer data is available in the result set
            try
            {
                var manufacturerName = reader.GetStringOrEmpty("ManufacturerName");
                if (!string.IsNullOrEmpty(manufacturerName))
                {
                    phone.Manufacturer = new Manufacturer
                    {
                        Id = phone.ManufacturerId,
                        Name = manufacturerName,
                        Address = reader.GetStringOrEmpty("ManufacturerAddress"),
                        IsAChildCompany = reader.GetBoolean("IsAChildCompany")
                    };
                }
            }
            catch
            {
                // Manufacturer data not available in this query
            }

            return phone;
        }
    }
}
