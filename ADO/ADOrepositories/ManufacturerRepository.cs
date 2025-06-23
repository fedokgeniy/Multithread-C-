using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Threading.Tasks;
using ManufacturerPhoneApp.Interfaces;
using ManufacturerPhoneApp.Models;
using ManufacturerPhoneApp.Infrastructure;
using ManufacturerPhoneApp.Extensions;

namespace ManufacturerPhoneApp.Repositories
{
    /// <summary>
    /// Repository for manufacturer data operations.
    /// </summary>
    public class ManufacturerRepository : IManufacturerRepository
    {
        private readonly DatabaseConnectionManager _connectionManager;

        /// <summary>
        /// Initializes a new instance of the ManufacturerRepository class.
        /// </summary>
        /// <param name="connectionManager">The database connection manager.</param>
        public ManufacturerRepository(DatabaseConnectionManager connectionManager)
        {
            _connectionManager = connectionManager ?? throw new ArgumentNullException(nameof(connectionManager));
        }

        /// <summary>
        /// Gets all manufacturers asynchronously.
        /// </summary>
        /// <returns>A list of all manufacturers.</returns>
        public async Task<List<Manufacturer>> GetAllAsync()
        {
            var manufacturers = new List<Manufacturer>();

            try
            {
                using var connection = _connectionManager.CreateConnection();
                await connection.OpenAsync();

                var query = "SELECT Id, Name, Address, IsAChildCompany FROM Manufacturers ORDER BY Name";
                using var command = new SqlCommand(query, connection);
                using var reader = await command.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    manufacturers.Add(MapToManufacturer(reader));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting all manufacturers: {ex.Message}");
                throw;
            }

            return manufacturers;
        }

        /// <summary>
        /// Gets a manufacturer by ID asynchronously.
        /// </summary>
        /// <param name="id">The manufacturer ID.</param>
        /// <returns>The manufacturer if found, otherwise null.</returns>
        public async Task<Manufacturer?> GetByIdAsync(int id)
        {
            try
            {
                using var connection = _connectionManager.CreateConnection();
                await connection.OpenAsync();

                var query = "SELECT Id, Name, Address, IsAChildCompany FROM Manufacturers WHERE Id = @Id";
                using var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Id", id);

                using var reader = await command.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    return MapToManufacturer(reader);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting manufacturer by ID {id}: {ex.Message}");
                throw;
            }

            return null;
        }

        /// <summary>
        /// Adds a new manufacturer asynchronously.
        /// </summary>
        /// <param name="manufacturer">The manufacturer to add.</param>
        /// <returns>The ID of the newly added manufacturer.</returns>
        public async Task<int> AddAsync(Manufacturer manufacturer)
        {
            if (manufacturer == null)
                throw new ArgumentNullException(nameof(manufacturer));

            try
            {
                using var connection = _connectionManager.CreateConnection();
                await connection.OpenAsync();

                var query = @"
                    INSERT INTO Manufacturers (Name, Address, IsAChildCompany) 
                    OUTPUT INSERTED.Id 
                    VALUES (@Name, @Address, @IsAChildCompany)";

                using var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Name", manufacturer.Name);
                command.Parameters.AddWithValue("@Address", manufacturer.Address);
                command.Parameters.AddWithValue("@IsAChildCompany", manufacturer.IsAChildCompany);

                var result = await command.ExecuteScalarAsync();
                return (int)result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding manufacturer: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Updates an existing manufacturer asynchronously.
        /// </summary>
        /// <param name="manufacturer">The manufacturer to update.</param>
        /// <returns>True if updated successfully, otherwise false.</returns>
        public async Task<bool> UpdateAsync(Manufacturer manufacturer)
        {
            if (manufacturer == null)
                throw new ArgumentNullException(nameof(manufacturer));

            try
            {
                using var connection = _connectionManager.CreateConnection();
                await connection.OpenAsync();

                var query = @"
                    UPDATE Manufacturers 
                    SET Name = @Name, Address = @Address, IsAChildCompany = @IsAChildCompany 
                    WHERE Id = @Id";

                using var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Id", manufacturer.Id);
                command.Parameters.AddWithValue("@Name", manufacturer.Name);
                command.Parameters.AddWithValue("@Address", manufacturer.Address);
                command.Parameters.AddWithValue("@IsAChildCompany", manufacturer.IsAChildCompany);

                var rowsAffected = await command.ExecuteNonQueryAsync();
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating manufacturer: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Deletes a manufacturer by ID asynchronously.
        /// </summary>
        /// <param name="id">The manufacturer ID to delete.</param>
        /// <returns>True if deleted successfully, otherwise false.</returns>
        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                using var connection = _connectionManager.CreateConnection();
                await connection.OpenAsync();

                var query = "DELETE FROM Manufacturers WHERE Id = @Id";
                using var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Id", id);

                var rowsAffected = await command.ExecuteNonQueryAsync();
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting manufacturer with ID {id}: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Maps a SqlDataReader row to a Manufacturer object.
        /// </summary>
        /// <param name="reader">The SqlDataReader instance.</param>
        /// <returns>A Manufacturer object.</returns>
        private static Manufacturer MapToManufacturer(SqlDataReader reader)
        {
            return new Manufacturer
            {
                Id = reader.GetInt32("Id"),
                Name = reader.GetStringOrEmpty("Name"),
                Address = reader.GetStringOrEmpty("Address"),
                IsAChildCompany = reader.GetBoolean("IsAChildCompany")
            };
        }
    }
}
