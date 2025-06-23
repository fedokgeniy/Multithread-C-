using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PhoneInheritanceDemo.Models;
using PhoneInheritanceDemo.Services;
using PhoneInheritanceDemo.Strategies;

namespace PhoneInheritanceDemo.Menu
{
    /// <summary>
    /// Manages the interactive console menu system for the phone inheritance demo.
    /// Provides navigation between different inheritance strategies and operations.
    /// </summary>
    public class MenuManager
    {
        private IPhoneService? _currentService;
        private readonly Dictionary<string, Func<IPhoneService>> _strategyCreators;

        /// <summary>
        /// Initializes a new instance of the MenuManager class.
        /// </summary>
        public MenuManager()
        {
            _strategyCreators = new Dictionary<string, Func<IPhoneService>>
            {
                ["TPH"] = StrategyFactory.CreateTphService,
                ["TPT"] = StrategyFactory.CreateTptService,
                ["TPC"] = StrategyFactory.CreateTpcService
            };
        }

        /// <summary>
        /// Runs the main menu loop.
        /// </summary>
        public async Task RunAsync()
        {
            bool exit = false;

            while (!exit)
            {
                try
                {
                    DisplayMainMenu();
                    var choice = Console.ReadLine();

                    switch (choice)
                    {
                        case "1":
                            await SelectStrategyAsync();
                            break;
                        case "2":
                            await StrategyMenuAsync();
                            break;
                        case "3":
                            exit = true;
                            break;
                        default:
                            Console.WriteLine("Invalid choice. Please try again.");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                }
            }

            _currentService?.Dispose();
            Console.WriteLine("Thank you for using Phone Inheritance Demo!");
        }

        /// <summary>
        /// Displays the main menu options.
        /// </summary>
        private void DisplayMainMenu()
        {
            Console.Clear();
            Console.WriteLine("╔══════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║              Phone Inheritance Demo - Main Menu              ║");
            Console.WriteLine("╠══════════════════════════════════════════════════════════════╣");
            Console.WriteLine("║  Current Strategy: " + (_currentService?.StrategyName ?? "None").PadRight(42) + "║");
            Console.WriteLine("╠══════════════════════════════════════════════════════════════╣");
            Console.WriteLine("║  1. Select Inheritance Strategy                              ║");
            Console.WriteLine("║  2. Work with Current Strategy                               ║");
            Console.WriteLine("║  3. Exit                                                     ║");
            Console.WriteLine("╚══════════════════════════════════════════════════════════════╝");
            Console.Write("Please select an option (1-3): ");
        }

        /// <summary>
        /// Handles strategy selection.
        /// </summary>
        private async Task SelectStrategyAsync()
        {
            Console.Clear();
            Console.WriteLine("╔══════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║                    Select Strategy                           ║");
            Console.WriteLine("╠══════════════════════════════════════════════════════════════╣");
            Console.WriteLine("║  1. TPH (Table-Per-Hierarchy)                                ║");
            Console.WriteLine("║  2. TPT (Table-Per-Type)                                     ║");
            Console.WriteLine("║  3. TPC (Table-Per-Concrete-Type)                            ║");
            Console.WriteLine("║  4. Back to Main Menu                                        ║");
            Console.WriteLine("╚══════════════════════════════════════════════════════════════╝");
            Console.Write("Please select a strategy (1-4): ");

            var choice = Console.ReadLine();
            var strategyKey = choice switch
            {
                "1" => "TPH",
                "2" => "TPT",
                "3" => "TPC",
                "4" => null,
                _ => null
            };

            if (strategyKey != null && _strategyCreators.ContainsKey(strategyKey))
            {
                _currentService?.Dispose();
                _currentService = _strategyCreators[strategyKey]();

                await _currentService.EnsureDatabaseCreatedAsync();

                Console.WriteLine($"\nSelected strategy: {_currentService.StrategyName}");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
            }
            else if (choice != "4")
            {
                Console.WriteLine("Invalid choice. Press any key to continue...");
                Console.ReadKey();
            }
        }

        /// <summary>
        /// Displays and handles the strategy-specific menu.
        /// </summary>
        private async Task StrategyMenuAsync()
        {
            if (_currentService == null)
            {
                Console.WriteLine("Please select a strategy first. Press any key to continue...");
                Console.ReadKey();
                return;
            }

            bool backToMain = false;

            while (!backToMain)
            {
                Console.Clear();
                Console.WriteLine("╔══════════════════════════════════════════════════════════════╗");
                Console.WriteLine($"║ Strategy: {_currentService.StrategyName.PadRight(51)}║");
                Console.WriteLine("╠══════════════════════════════════════════════════════════════╣");
                Console.WriteLine("║  1. View Strategy Information                                ║");
                Console.WriteLine("║  2. Seed Sample Data                                         ║");
                Console.WriteLine("║  3. View All Manufacturers                                   ║");
                Console.WriteLine("║  4. View All Phones                                          ║");
                Console.WriteLine("║  5. View Smartphones Only                                    ║");
                Console.WriteLine("║  6. View Feature Phones Only                                 ║");
                Console.WriteLine("║  7. View Gaming Phones Only                                  ║");
                Console.WriteLine("║  8. Add New Manufacturer                                     ║");
                Console.WriteLine("║  9. Add New Phone                                            ║");
                Console.WriteLine("║ 10. Find Phone by ID                                         ║");
                Console.WriteLine("║ 11. Back to Main Menu                                        ║");
                Console.WriteLine("╚══════════════════════════════════════════════════════════════╝");
                Console.Write("Please select an option (1-11): ");

                var choice = Console.ReadLine();

                try
                {
                    switch (choice)
                    {
                        case "1":
                            await ShowStrategyInfoAsync();
                            break;
                        case "2":
                            await SeedDataAsync();
                            break;
                        case "3":
                            await ShowManufacturersAsync();
                            break;
                        case "4":
                            await ShowAllPhonesAsync();
                            break;
                        case "5":
                            await ShowPhonesByTypeAsync<Smartphone>("Smartphones");
                            break;
                        case "6":
                            await ShowPhonesByTypeAsync<FeaturePhone>("Feature Phones");
                            break;
                        case "7":
                            await ShowPhonesByTypeAsync<GamingPhone>("Gaming Phones");
                            break;
                        case "8":
                            await AddManufacturerAsync();
                            break;
                        case "9":
                            await AddPhoneAsync();
                            break;
                        case "10":
                            await FindPhoneByIdAsync();
                            break;
                        case "11":
                            backToMain = true;
                            break;
                        default:
                            Console.WriteLine("Invalid choice. Press any key to continue...");
                            Console.ReadKey();
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                }
            }
        }

        /// <summary>
        /// Shows information about the current strategy.
        /// </summary>
        private async Task ShowStrategyInfoAsync()
        {
            Console.Clear();
            Console.WriteLine($"Strategy Information: {_currentService!.StrategyName}");
            Console.WriteLine(new string('=', 60));

            var description = _currentService.StrategyName switch
            {
                "Table-Per-Hierarchy (TPH)" => @"
TPH stores all types in a single table with a discriminator column.
- Pros: Best performance for queries across all types, simple schema
- Cons: Sparse tables with many nullable columns
- Use when: You frequently query across all types",

                "Table-Per-Type (TPT)" => @"
TPT uses separate tables for each type with foreign key relationships.
- Pros: Normalized schema, no nullable columns
- Cons: Requires joins for queries, more complex schema
- Use when: You have distinct types with different properties",

                "Table-Per-Concrete-Type (TPC)" => @"
TPC creates complete tables for each concrete type.
- Pros: No joins required, optimal for type-specific queries
- Cons: Data duplication, difficult cross-type queries
- Use when: Types are very different and rarely queried together",

                _ => "Unknown strategy"
            };

            Console.WriteLine(description);
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        /// <summary>
        /// Seeds the database with sample data.
        /// </summary>
        private async Task SeedDataAsync()
        {
            Console.WriteLine("Seeding sample data...");
            await _currentService!.SeedDataAsync();
            Console.WriteLine("Sample data has been added to the database.");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        /// <summary>
        /// Shows all manufacturers.
        /// </summary>
        private async Task ShowManufacturersAsync()
        {
            Console.Clear();
            Console.WriteLine("All Manufacturers:");
            Console.WriteLine(new string('=', 50));

            var manufacturers = await _currentService!.GetManufacturersAsync();

            if (manufacturers.Any())
            {
                foreach (var manufacturer in manufacturers)
                {
                    Console.WriteLine($"ID: {manufacturer.ManufacturerId} | {manufacturer}");
                }
            }
            else
            {
                Console.WriteLine("No manufacturers found. Consider seeding sample data first.");
            }

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }

        /// <summary>
        /// Shows all phones.
        /// </summary>
        private async Task ShowAllPhonesAsync()
        {
            Console.Clear();
            Console.WriteLine("All Phones:");
            Console.WriteLine(new string('=', 80));

            var phones = await _currentService!.GetPhonesAsync();

            if (phones.Any())
            {
                foreach (var phone in phones)
                {
                    Console.WriteLine($"ID: {phone.PhoneId} | {phone.GetPhoneType()} | {phone.GetPhoneInfo()}");
                    Console.WriteLine();
                }
            }
            else
            {
                Console.WriteLine("No phones found. Consider seeding sample data first.");
            }

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        /// <summary>
        /// Shows phones of a specific type.
        /// </summary>
        private async Task ShowPhonesByTypeAsync<T>(string typeName) where T : Phone
        {
            Console.Clear();
            Console.WriteLine($"All {typeName}:");
            Console.WriteLine(new string('=', 80));

            var phones = await _currentService!.GetPhonesByTypeAsync<T>();

            if (phones.Any())
            {
                foreach (var phone in phones)
                {
                    Console.WriteLine($"ID: {phone.PhoneId} | {phone.GetPhoneInfo()}");
                    Console.WriteLine();
                }
            }
            else
            {
                Console.WriteLine($"No {typeName.ToLower()} found. Consider seeding sample data first.");
            }

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        /// <summary>
        /// Adds a new manufacturer interactively.
        /// </summary>
        private async Task AddManufacturerAsync()
        {
            Console.Clear();
            Console.WriteLine("Add New Manufacturer:");
            Console.WriteLine(new string('=', 30));

            Console.Write("Enter manufacturer name: ");
            var name = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(name)) return;

            Console.Write("Enter country: ");
            var country = Console.ReadLine() ?? "";

            Console.Write("Enter founded year: ");
            if (!int.TryParse(Console.ReadLine(), out int foundedYear))
            {
                Console.WriteLine("Invalid year. Operation cancelled.");
                Console.ReadKey();
                return;
            }

            var manufacturer = new Manufacturer
            {
                Name = name,
                Country = country,
                FoundedYear = foundedYear
            };

            try
            {
                await _currentService!.AddManufacturerAsync(manufacturer);
                Console.WriteLine($"Manufacturer '{name}' added successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding manufacturer: {ex.Message}");
            }

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        /// <summary>
        /// Adds a new phone interactively.
        /// </summary>
        private async Task AddPhoneAsync()
        {
            Console.Clear();
            Console.WriteLine("Add New Phone:");
            Console.WriteLine(new string('=', 20));

            var manufacturers = await _currentService!.GetManufacturersAsync();
            if (!manufacturers.Any())
            {
                Console.WriteLine("No manufacturers available. Please add a manufacturer first.");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("Available manufacturers:");
            foreach (var m in manufacturers)
            {
                Console.WriteLine($"{m.ManufacturerId}. {m.Name}");
            }

            Console.Write("Select manufacturer ID: ");
            if (!int.TryParse(Console.ReadLine(), out int manufacturerId) || 
                !manufacturers.Any(m => m.ManufacturerId == manufacturerId))
            {
                Console.WriteLine("Invalid manufacturer ID.");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("\nSelect phone type:");
            Console.WriteLine("1. Smartphone");
            Console.WriteLine("2. Feature Phone");
            Console.WriteLine("3. Gaming Phone");
            Console.Write("Choice: ");

            var phoneTypeChoice = Console.ReadLine();
            Phone? phone = null;

            Console.Write("Enter model: ");
            var model = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(model)) return;

            Console.Write("Enter serial number: ");
            var serialNumber = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(serialNumber)) return;

            Console.Write("Enter battery capacity (mAh): ");
            if (!int.TryParse(Console.ReadLine(), out int batteryCapacity)) return;

            Console.Write("Enter screen size (inches): ");
            if (!decimal.TryParse(Console.ReadLine(), out decimal screenSize)) return;

            Console.Write("Enter price: ");
            if (!decimal.TryParse(Console.ReadLine(), out decimal price)) return;

            try
            {
                phone = phoneTypeChoice switch
                {
                    "1" => await CreateSmartphoneAsync(model, serialNumber, batteryCapacity, screenSize, price, manufacturerId),
                    "2" => await CreateFeaturePhoneAsync(model, serialNumber, batteryCapacity, screenSize, price, manufacturerId),
                    "3" => await CreateGamingPhoneAsync(model, serialNumber, batteryCapacity, screenSize, price, manufacturerId),
                    _ => null
                };

                if (phone != null)
                {
                    await _currentService!.AddPhoneAsync(phone);
                    Console.WriteLine($"Phone '{model}' added successfully!");
                }
                else
                {
                    Console.WriteLine("Invalid phone type selected.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding phone: {ex.Message}");
            }

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        /// <summary>
        /// Creates a smartphone with additional properties.
        /// </summary>
        private async Task<Smartphone> CreateSmartphoneAsync(string model, string serialNumber, 
            int batteryCapacity, decimal screenSize, decimal price, int manufacturerId)
        {
            Console.Write("Enter operating system: ");
            var os = Console.ReadLine() ?? "";

            Console.Write("Enter RAM size (GB): ");
            int.TryParse(Console.ReadLine(), out int ramSize);

            Console.Write("Enter storage size (GB): ");
            int.TryParse(Console.ReadLine(), out int storageSize);

            Console.Write("Enter camera resolution (MP): ");
            int.TryParse(Console.ReadLine(), out int cameraResolution);

            Console.Write("Has 5G? (y/n): ");
            var hasFiveG = Console.ReadLine()?.ToLower() == "y";

            return new Smartphone
            {
                Model = model,
                SerialNumber = serialNumber,
                BatteryCapacity = batteryCapacity,
                ScreenSize = screenSize,
                Price = price,
                ManufacturerId = manufacturerId,
                OperatingSystem = os,
                RamSize = ramSize,
                StorageSize = storageSize,
                CameraResolution = cameraResolution,
                HasFiveG = hasFiveG
            };
        }

        /// <summary>
        /// Creates a feature phone with additional properties.
        /// </summary>
        private async Task<FeaturePhone> CreateFeaturePhoneAsync(string model, string serialNumber, 
            int batteryCapacity, decimal screenSize, decimal price, int manufacturerId)
        {
            Console.Write("Has physical keyboard? (y/n): ");
            var hasKeyboard = Console.ReadLine()?.ToLower() == "y";

            Console.Write("Enter SMS storage capacity: ");
            int.TryParse(Console.ReadLine(), out int smsStorage);

            Console.Write("Has basic games? (y/n): ");
            var hasGames = Console.ReadLine()?.ToLower() == "y";

            return new FeaturePhone
            {
                Model = model,
                SerialNumber = serialNumber,
                BatteryCapacity = batteryCapacity,
                ScreenSize = screenSize,
                Price = price,
                ManufacturerId = manufacturerId,
                HasPhysicalKeyboard = hasKeyboard,
                SmsStorageCapacity = smsStorage,
                HasBasicGames = hasGames
            };
        }

        /// <summary>
        /// Creates a gaming phone with additional properties.
        /// </summary>
        private async Task<GamingPhone> CreateGamingPhoneAsync(string model, string serialNumber, 
            int batteryCapacity, decimal screenSize, decimal price, int manufacturerId)
        {
            Console.Write("Enter GPU name: ");
            var gpuName = Console.ReadLine() ?? "";

            Console.Write("Enter refresh rate (Hz): ");
            int.TryParse(Console.ReadLine(), out int refreshRate);

            Console.Write("Has gaming triggers? (y/n): ");
            var hasTriggers = Console.ReadLine()?.ToLower() == "y";

            Console.Write("Enter cooling system: ");
            var coolingSystem = Console.ReadLine() ?? "";

            return new GamingPhone
            {
                Model = model,
                SerialNumber = serialNumber,
                BatteryCapacity = batteryCapacity,
                ScreenSize = screenSize,
                Price = price,
                ManufacturerId = manufacturerId,
                GpuName = gpuName,
                RefreshRate = refreshRate,
                HasGamingTriggers = hasTriggers,
                CoolingSystem = coolingSystem
            };
        }

        /// <summary>
        /// Finds and displays a phone by its ID.
        /// </summary>
        private async Task FindPhoneByIdAsync()
        {
            Console.Clear();
            Console.WriteLine("Find Phone by ID:");
            Console.WriteLine(new string('=', 20));

            Console.Write("Enter phone ID: ");
            if (!int.TryParse(Console.ReadLine(), out int phoneId))
            {
                Console.WriteLine("Invalid ID format.");
                Console.ReadKey();
                return;
            }

            var phone = await _currentService!.GetPhoneByIdAsync(phoneId);

            if (phone != null)
            {
                Console.WriteLine($"\nFound phone:");
                Console.WriteLine($"ID: {phone.PhoneId}");
                Console.WriteLine($"Type: {phone.GetPhoneType()}");
                Console.WriteLine($"Details: {phone.GetPhoneInfo()}");
            }
            else
            {
                Console.WriteLine($"No phone found with ID: {phoneId}");
            }

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }
    }
}