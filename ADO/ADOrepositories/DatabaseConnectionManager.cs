using System;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace ManufacturerPhoneApp.Repositories
{
    /// <summary>
    /// Manages database connections and initialization.
    /// </summary>
    public class DatabaseConnectionManager
    {
        private readonly string _connectionString;
        private readonly string _databaseName;

        /// <summary>
        /// Initializes a new instance of the DatabaseConnectionManager class.
        /// </summary>
        /// <param name="connectionString">The connection string for the database.</param>
        /// <param name="databaseName">The name of the database.</param>
        public DatabaseConnectionManager(string connectionString, string databaseName)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
            _databaseName = databaseName ?? throw new ArgumentNullException(nameof(databaseName));
        }

        /// <summary>
        /// Creates a new SQL connection.
        /// </summary>
        /// <returns>A new SqlConnection instance.</returns>
        public SqlConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }

        /// <summary>
        /// Initializes the database and creates tables if they don't exist.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task InitializeDatabaseAsync()
        {
            try
            {
                await CreateDatabaseIfNotExistsAsync();
                await CreateTablesIfNotExistAsync();
                Console.WriteLine("Database initialized successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error initializing database: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Creates the database if it doesn't exist.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        private async Task CreateDatabaseIfNotExistsAsync()
        {
            var masterConnectionString = _connectionString.Replace($"Database={_databaseName};", "Database=master;");

            using var connection = new SqlConnection(masterConnectionString);
            await connection.OpenAsync();

            var checkDbQuery = $"SELECT COUNT(*) FROM sys.databases WHERE name = '{_databaseName}'";
            using var checkCommand = new SqlCommand(checkDbQuery, connection);
            var dbExists = (int)await checkCommand.ExecuteScalarAsync() > 0;

            if (!dbExists)
            {
                var createDbQuery = $"CREATE DATABASE [{_databaseName}]";
                using var createCommand = new SqlCommand(createDbQuery, connection);
                await createCommand.ExecuteNonQueryAsync();
                Console.WriteLine($"Database '{_databaseName}' created successfully.");
            }
        }

        /// <summary>
        /// Creates the required tables if they don't exist.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        private async Task CreateTablesIfNotExistAsync()
        {
            using var connection = CreateConnection();
            await connection.OpenAsync();

            // Create Manufacturers table
            var createManufacturersTableQuery = @"
                IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Manufacturers' AND xtype='U')
                CREATE TABLE Manufacturers (
                    Id INT IDENTITY(1,1) PRIMARY KEY,
                    Name NVARCHAR(255) NOT NULL,
                    Address NVARCHAR(500) NOT NULL,
                    IsAChildCompany BIT NOT NULL DEFAULT 0,
                    CreatedAt DATETIME2 DEFAULT GETDATE()
                )";

            using var createManufacturersCommand = new SqlCommand(createManufacturersTableQuery, connection);
            await createManufacturersCommand.ExecuteNonQueryAsync();

            // Create Phones table
            var createPhonesTableQuery = @"
                IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Phones' AND xtype='U')
                CREATE TABLE Phones (
                    Id INT IDENTITY(1,1) PRIMARY KEY,
                    Model NVARCHAR(255) NOT NULL,
                    SerialNumber NVARCHAR(255) NOT NULL UNIQUE,
                    PhoneType NVARCHAR(100) NOT NULL,
                    ManufacturerId INT NOT NULL,
                    CreatedAt DATETIME2 DEFAULT GETDATE(),
                    FOREIGN KEY (ManufacturerId) REFERENCES Manufacturers(Id) ON DELETE CASCADE
                )";

            using var createPhonesCommand = new SqlCommand(createPhonesTableQuery, connection);
            await createPhonesCommand.ExecuteNonQueryAsync();

            Console.WriteLine("Tables created successfully.");
        }

        /// <summary>
        /// Tests the database connection.
        /// </summary>
        /// <returns>True if connection is successful, otherwise false.</returns>
        public async Task<bool> TestConnectionAsync()
        {
            try
            {
                using var connection = CreateConnection();
                await connection.OpenAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Connection test failed: {ex.Message}");
                return false;
            }
        }
    }
}
