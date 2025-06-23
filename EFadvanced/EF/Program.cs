using PhoneInheritanceDemo.Menu;
using PhoneInheritanceDemo.Services;
using PhoneInheritanceDemo.Strategies;

namespace PhoneInheritanceDemo;

/// <summary>
/// Main program class demonstrating Entity Framework inheritance strategies.
/// </summary>
class Program
{
    /// <summary>
    /// Main entry point of the application.
    /// </summary>
    /// <param name="args">Command line arguments.</param>
    static async Task Main(string[] args)
    {
        try
        {
            Console.Title = "Phone Inheritance Demo - Entity Framework Strategies";

            // Display welcome message
            ShowWelcomeMessage();

            // Let user choose inheritance strategy
            var phoneService = await ChooseInheritanceStrategyAsync();

            if (phoneService == null)
            {
                Console.WriteLine("No strategy selected. Exiting application.");
                return;
            }

            // Create and run menu manager
            var menuManager = new MenuManager(phoneService);
            await menuManager.ShowMainMenuAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }

    /// <summary>
    /// Displays the welcome message and application information.
    /// </summary>
    private static void ShowWelcomeMessage()
    {
        Console.Clear();
        Console.WriteLine("╔".PadRight(81, '═') + "╗");
        Console.WriteLine("║" + " Phone Inheritance Demo Application".PadRight(80) + "║");
        Console.WriteLine("║" + " Entity Framework Core Inheritance Strategies".PadRight(80) + "║");
        Console.WriteLine("╠".PadRight(81, '═') + "╣");
        Console.WriteLine("║" + " This application demonstrates three inheritance mapping strategies:".PadRight(80) + "║");
        Console.WriteLine("║" + "".PadRight(80) + "║");
        Console.WriteLine("║" + " • TPH (Table-Per-Hierarchy): Single table with discriminator".PadRight(80) + "║");
        Console.WriteLine("║" + " • TPT (Table-Per-Type): Separate tables with foreign keys".PadRight(80) + "║");
        Console.WriteLine("║" + " • TPC (Table-Per-Concrete-Type): Complete tables per type".PadRight(80) + "║");
        Console.WriteLine("║" + "".PadRight(80) + "║");
        Console.WriteLine("║" + " Domain: Phone hierarchy (FeaturePhone, Smartphone, GamingPhone)".PadRight(80) + "║");
        Console.WriteLine("║" + " Technology: Entity Framework Core 8.0 with InMemory database".PadRight(80) + "║");
        Console.WriteLine("╚".PadRight(81, '═') + "╝");
        Console.WriteLine();
    }

    /// <summary>
    /// Allows the user to choose an inheritance strategy.
    /// </summary>
    /// <returns>A phone service using the selected strategy.</returns>
    private static async Task<IPhoneService?> ChooseInheritanceStrategyAsync()
    {
        while (true)
        {
            Console.WriteLine("Please select an Entity Framework inheritance strategy:");
            Console.WriteLine();

            var strategies = StrategyFactory.GetAvailableStrategies();
            for (int i = 0; i < strategies.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {strategies[i]}");
            }

            Console.WriteLine("4. Exit");
            Console.WriteLine();
            Console.Write("Enter your choice (1-4): ");

            if (int.TryParse(Console.ReadLine(), out int choice))
            {
                if (choice == 4)
                {
                    return null; // Exit
                }

                if (choice >= 1 && choice <= 3)
                {
                    try
                    {
                        var phoneService = StrategyFactory.CreateServiceByChoice(choice);

                        Console.WriteLine();
                        Console.WriteLine($"Selected: {phoneService.GetStrategyName()}");
                        Console.WriteLine("Initializing database...");

                        // The database will be created automatically when first accessed
                        await Task.Delay(500); // Small delay for user experience

                        Console.WriteLine("Database initialized successfully!");
                        Console.WriteLine();
                        Console.WriteLine("Press any key to continue to the main menu...");
                        Console.ReadKey();

                        return phoneService;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error initializing strategy: {ex.Message}");
                        Console.WriteLine("Please try again.");
                        Console.WriteLine();
                        continue;
                    }
                }
            }

            Console.WriteLine("Invalid selection. Please enter a number between 1 and 4.");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
            Console.Clear();
            ShowWelcomeMessage();
        }
    }
}