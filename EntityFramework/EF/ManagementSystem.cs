using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ManufacturerPhoneApp.Data;
using ManufacturerPhoneApp.Repositories;
using ManufacturerPhoneApp.Services;
using ManufacturerPhoneApp.UI;

namespace ManufacturerPhoneApp
{
    /// <summary>
    /// Main program entry point.
    /// </summary>
    internal class ManagementSystem
    {
        /// <summary>
        /// Application entry point.
        /// </summary>
        /// <param name="args">Command line arguments.</param>
        private static async Task Main(string[] args)
        {
            Console.WriteLine("Initializing Manufacturer-Phone Management System...");

            try
            {
                var host = CreateHostBuilder(args).Build();

                await InitializeDatabaseAsync(host.Services);

                var consoleMenu = host.Services.GetRequiredService<ConsoleMenu>();
                await consoleMenu.ShowMainMenuAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Application error: {ex.Message}");
                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
            }
        }

        /// <summary>
        /// Creates and configures the host builder.
        /// </summary>
        /// <param name="args">Command line arguments.</param>
        /// <returns>The configured host builder.</returns>
        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.AddConsole();
                    logging.SetMinimumLevel(LogLevel.Warning);
                })
                .ConfigureServices((context, services) =>
                {
                    services.AddDbContext<ManufacturerPhoneContext>(options =>
                        options.UseSqlite("Data Source=ManufacturerPhone.db")
                               .EnableSensitiveDataLogging(false)
                               .EnableDetailedErrors(false));

                    services.AddScoped<IManufacturerRepository, ManufacturerRepository>();
                    services.AddScoped<IPhoneRepository, PhoneRepository>();

                    services.AddScoped<IBusinessService, BusinessService>();

                    services.AddScoped<ConsoleMenu>();
                });

        /// <summary>
        /// Initializes the database with sample data.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        private static async Task InitializeDatabaseAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ManufacturerPhoneContext>();

            try
            {
                Console.WriteLine("Ensuring database is created...");

                await context.Database.EnsureCreatedAsync();

                Console.WriteLine("Initializing sample data...");

                DataInitializer.Initialize(context);

                var manufacturerCount = await context.Manufacturers.CountAsync();
                var phoneCount = await context.Phones.CountAsync();

                Console.WriteLine($"Database initialized successfully!");
                Console.WriteLine($"Manufacturers: {manufacturerCount}, Phones: {phoneCount}");
                Console.WriteLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Database initialization error: {ex.Message}");
                throw;
            }
        }
    }
}
