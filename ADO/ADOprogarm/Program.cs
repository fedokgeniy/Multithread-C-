using System;
using System.Threading.Tasks;
using ManufacturerPhoneApp.Infrastructure;
using ManufacturerPhoneApp.Repositories;
using ManufacturerPhoneApp.Services;
using ManufacturerPhoneApp.UI;

namespace ManufacturerPhoneApp
{
    /// <summary>
    /// Main program class containing the application entry point.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// <param name="args">Command line arguments.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public static async Task Main(string[] args)
        {
            Console.WriteLine("Starting Manufacturer-Phone Management System...");
            Console.WriteLine();

            try
            {
                // Initialize application
                await InitializeApplicationAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fatal error: {ex.Message}");
                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
            }
        }

        /// <summary>
        /// Initializes the application and its dependencies.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        private static async Task InitializeApplicationAsync()
        {
            // Database configuration
            const string databaseName = "ManufacturerPhoneDB";
            const string connectionString = $"Server=localhost;Database={databaseName};Trusted_Connection=true;TrustServerCertificate=true;";

            // Initialize database connection manager
            var connectionManager = new DatabaseConnectionManager(connectionString, databaseName);

            // Test database connection
            Console.WriteLine("Testing database connection...");
            var connectionSuccess = await connectionManager.TestConnectionAsync();
            if (!connectionSuccess)
            {
                Console.WriteLine("Failed to connect to database. Please ensure SQL Server is running.");
                Console.WriteLine("Connection string: " + connectionString);
                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
                return;
            }

            // Initialize database schema
            Console.WriteLine("Initializing database schema...");
            await connectionManager.InitializeDatabaseAsync();

            // Initialize repositories
            var manufacturerRepository = new ManufacturerRepository(connectionManager);
            var phoneRepository = new PhoneRepository(connectionManager);

            // Initialize services
            var manufacturerService = new ManufacturerService(manufacturerRepository);
            var phoneService = new PhoneService(phoneRepository, manufacturerRepository);
            var databaseSeeder = new DatabaseSeeder(manufacturerRepository, phoneRepository);

            // Initialize console interface
            var consoleInterface = new ConsoleInterface(manufacturerService, phoneService, databaseSeeder);

            Console.WriteLine("Application initialized successfully!");
            Console.WriteLine();

            // Run the application
            await consoleInterface.RunAsync();
        }
    }
}
