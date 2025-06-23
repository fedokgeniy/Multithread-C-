using System;
using System.Threading.Tasks;
using PhoneInheritanceDemo.Strategies;
using PhoneInheritanceDemo.Menu;

namespace PhoneInheritanceDemo
{
    /// <summary>
    /// Entry point for the Phone Inheritance Demo console application.
    /// Demonstrates Entity Framework Core inheritance strategies (TPH, TPT, TPC) with SQLite database.
    /// </summary>
    internal class Program
    {
        /// <summary>
        /// Main entry point for the application.
        /// Initializes the console menu system for interacting with different inheritance strategies.
        /// </summary>
        /// <param name="args">Command line arguments (not used)</param>
        static async Task Main(string[] args)
        {
            try
            {
                Console.WriteLine("=== Phone Inheritance Demo - Entity Framework Core ===");
                Console.WriteLine("Demonstrating TPH, TPT, and TPC inheritance strategies with SQLite");
                Console.WriteLine();

                // Initialize databases for all strategies
                await InitializeDatabasesAsync();

                // Start the interactive menu
                var menuManager = new MenuManager();
                await menuManager.RunAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
            }
        }

        /// <summary>
        /// Initializes all databases for the three inheritance strategies.
        /// This ensures that the databases and tables are created before first use.
        /// </summary>
        private static async Task InitializeDatabasesAsync()
        {
            Console.WriteLine("Initializing databases...");

            try
            {
                // Initialize TPH database
                var tphService = StrategyFactory.CreateTphService();
                await tphService.EnsureDatabaseCreatedAsync();
                Console.WriteLine("✓ TPH database initialized");

                // Initialize TPT database
                var tptService = StrategyFactory.CreateTptService();
                await tptService.EnsureDatabaseCreatedAsync();
                Console.WriteLine("✓ TPT database initialized");

                // Initialize TPC database
                var tpcService = StrategyFactory.CreateTpcService();
                await tpcService.EnsureDatabaseCreatedAsync();
                Console.WriteLine("✓ TPC database initialized");

                Console.WriteLine("All databases initialized successfully!");
                Console.WriteLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error initializing databases: {ex.Message}");
                throw;
            }
        }
    }
}