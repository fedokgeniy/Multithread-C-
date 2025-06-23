using PhoneInheritanceDemo.Services;
using PhoneInheritanceDemo.Models;

namespace PhoneInheritanceDemo.Menu;

/// <summary>
/// Manages the console menu system for the phone inheritance demo application.
/// </summary>
public class MenuManager
{
    private readonly IPhoneService _phoneService;

    /// <summary>
    /// Initializes a new instance of the MenuManager class.
    /// </summary>
    /// <param name="phoneService">The phone service to use.</param>
    public MenuManager(IPhoneService phoneService)
    {
        _phoneService = phoneService;
    }

    /// <summary>
    /// Displays the main menu and handles user input.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task ShowMainMenuAsync()
    {
        var menuItems = new List<MenuItem>
        {
            new MenuItem("View Strategy Information", ViewStrategyInfoAsync),
            new MenuItem("Seed Sample Data", SeedDataAsync),
            new MenuItem("View All Manufacturers", ViewAllManufacturersAsync),
            new MenuItem("View All Phones", ViewAllPhonesAsync),
            new MenuItem("View Feature Phones", ViewFeaturePhonesAsync),
            new MenuItem("View Smartphones", ViewSmartphonesAsync),
            new MenuItem("View Gaming Phones", ViewGamingPhonesAsync),
            new MenuItem("Add New Manufacturer", AddManufacturerAsync),
            new MenuItem("Add New Phone", AddPhoneAsync),
            new MenuItem("Search Phone by ID", SearchPhoneByIdAsync),
            new MenuItem("Exit", isExit: true)
        };

        await ShowMenuAsync("Phone Inheritance Demo - " + _phoneService.GetStrategyName(), menuItems);
    }

    /// <summary>
    /// Displays a menu with the specified title and items.
    /// </summary>
    /// <param name="title">The menu title.</param>
    /// <param name="menuItems">The menu items to display.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    private async Task ShowMenuAsync(string title, List<MenuItem> menuItems)
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("═".PadRight(80, '═'));
            Console.WriteLine($" {title}");
            Console.WriteLine("═".PadRight(80, '═'));
            Console.WriteLine();

            for (int i = 0; i < menuItems.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {menuItems[i].Text}");
            }

            Console.WriteLine();
            Console.Write("Please select an option (1-{0}): ", menuItems.Count);

            if (int.TryParse(Console.ReadLine(), out int choice) && 
                choice >= 1 && choice <= menuItems.Count)
            {
                var selectedItem = menuItems[choice - 1];

                if (selectedItem.IsExit)
                {
                    Console.WriteLine("Thank you for using the Phone Inheritance Demo!");
                    break;
                }

                if (selectedItem.Action != null)
                {
                    try
                    {
                        await selectedItem.Action();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                    }

                    Console.WriteLine();
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                }
            }
            else
            {
                Console.WriteLine("Invalid selection. Please try again.");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
            }
        }
    }

    /// <summary>
    /// Displays strategy information.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    private async Task ViewStrategyInfoAsync()
    {
        Console.WriteLine();
        Console.WriteLine("Current Strategy: " + _phoneService.GetStrategyName());
        Console.WriteLine();

        string strategyName = _phoneService.GetStrategyName();

        if (strategyName.Contains("TPH"))
        {
            Console.WriteLine("Table-Per-Hierarchy (TPH) Strategy:");
            Console.WriteLine("- All phone types are stored in a single table");
            Console.WriteLine("- Uses a discriminator column to identify phone types");
            Console.WriteLine("- Simplest schema with potential for unused columns");
            Console.WriteLine("- Best performance for queries across all types");
        }
        else if (strategyName.Contains("TPT"))
        {
            Console.WriteLine("Table-Per-Type (TPT) Strategy:");
            Console.WriteLine("- Each type has its own table");
            Console.WriteLine("- Base properties in the base table, specific properties in derived tables");
            Console.WriteLine("- Normalized schema with foreign key relationships");
            Console.WriteLine("- May require joins for complex queries");
        }
        else if (strategyName.Contains("TPC"))
        {
            Console.WriteLine("Table-Per-Concrete-Type (TPC) Strategy:");
            Console.WriteLine("- Each concrete type has its own complete table");
            Console.WriteLine("- All properties (base and specific) in each table");
            Console.WriteLine("- Denormalized schema, but no joins required");
            Console.WriteLine("- Good performance for type-specific queries");
        }

        await Task.CompletedTask;
    }

    /// <summary>
    /// Seeds the database with sample data.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    private async Task SeedDataAsync()
    {
        Console.WriteLine();
        Console.WriteLine("Seeding database with sample data...");

        await _phoneService.SeedDataAsync();

        Console.WriteLine("Sample data has been added successfully!");
    }

    /// <summary>
    /// Displays all manufacturers.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    private async Task ViewAllManufacturersAsync()
    {
        Console.WriteLine();
        Console.WriteLine("All Manufacturers:");
        Console.WriteLine("─".PadRight(80, '─'));

        var manufacturers = await _phoneService.GetAllManufacturersAsync();

        if (manufacturers.Count == 0)
        {
            Console.WriteLine("No manufacturers found. Please seed data first.");
            return;
        }

        foreach (var manufacturer in manufacturers)
        {
            manufacturer.Print();
        }
    }

    /// <summary>
    /// Displays all phones.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    private async Task ViewAllPhonesAsync()
    {
        Console.WriteLine();
        Console.WriteLine("All Phones:");
        Console.WriteLine("─".PadRight(80, '─'));

        var phones = await _phoneService.GetAllPhonesAsync();

        if (phones.Count == 0)
        {
            Console.WriteLine("No phones found. Please seed data first.");
            return;
        }

        foreach (var phone in phones)
        {
            phone.Print();
            Console.WriteLine();
        }
    }

    /// <summary>
    /// Displays feature phones only.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    private async Task ViewFeaturePhonesAsync()
    {
        Console.WriteLine();
        Console.WriteLine("Feature Phones:");
        Console.WriteLine("─".PadRight(80, '─'));

        var phones = await _phoneService.GetPhonesByTypeAsync<FeaturePhone>();

        if (phones.Count == 0)
        {
            Console.WriteLine("No feature phones found.");
            return;
        }

        foreach (var phone in phones)
        {
            phone.Print();
            Console.WriteLine();
        }
    }

    /// <summary>
    /// Displays smartphones only.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    private async Task ViewSmartphonesAsync()
    {
        Console.WriteLine();
        Console.WriteLine("Smartphones:");
        Console.WriteLine("─".PadRight(80, '─'));

        var phones = await _phoneService.GetPhonesByTypeAsync<Smartphone>();

        if (phones.Count == 0)
        {
            Console.WriteLine("No smartphones found.");
            return;
        }

        foreach (var phone in phones)
        {
            phone.Print();
            Console.WriteLine();
        }
    }

    /// <summary>
    /// Displays gaming phones only.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    private async Task ViewGamingPhonesAsync()
    {
        Console.WriteLine();
        Console.WriteLine("Gaming Phones:");
        Console.WriteLine("─".PadRight(80, '─'));

        var phones = await _phoneService.GetPhonesByTypeAsync<GamingPhone>();

        if (phones.Count == 0)
        {
            Console.WriteLine("No gaming phones found.");
            return;
        }

        foreach (var phone in phones)
        {
            phone.Print();
            Console.WriteLine();
        }
    }

    /// <summary>
    /// Adds a new manufacturer interactively.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    private async Task AddManufacturerAsync()
    {
        Console.WriteLine();
        Console.WriteLine("Add New Manufacturer:");
        Console.WriteLine("─".PadRight(50, '─'));

        Console.Write("Enter manufacturer name: ");
        string name = Console.ReadLine() ?? string.Empty;

        Console.Write("Enter manufacturer address: ");
        string address = Console.ReadLine() ?? string.Empty;

        Console.Write("Is this a child company? (y/n): ");
        bool isChildCompany = Console.ReadLine()?.ToLower() == "y";

        var manufacturer = new Manufacturer
        {
            Name = name,
            Address = address,
            IsAChildCompany = isChildCompany
        };

        var addedManufacturer = await _phoneService.AddManufacturerAsync(manufacturer);
        Console.WriteLine($"Manufacturer added successfully with ID: {addedManufacturer.Id}");
    }

    /// <summary>
    /// Adds a new phone interactively.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    private async Task AddPhoneAsync()
    {
        Console.WriteLine();
        Console.WriteLine("Add New Phone:");
        Console.WriteLine("─".PadRight(50, '─'));

        // Display available manufacturers
        var manufacturers = await _phoneService.GetAllManufacturersAsync();
        if (manufacturers.Count == 0)
        {
            Console.WriteLine("No manufacturers found. Please add manufacturers first.");
            return;
        }

        Console.WriteLine("Available Manufacturers:");
        for (int i = 0; i < manufacturers.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {manufacturers[i].Name}");
        }

        Console.Write("Select manufacturer (1-{0}): ", manufacturers.Count);
        if (!int.TryParse(Console.ReadLine(), out int mfgChoice) || 
            mfgChoice < 1 || mfgChoice > manufacturers.Count)
        {
            Console.WriteLine("Invalid manufacturer selection.");
            return;
        }

        int manufacturerId = manufacturers[mfgChoice - 1].Id;

        // Select phone type
        Console.WriteLine();
        Console.WriteLine("Select Phone Type:");
        Console.WriteLine("1. Feature Phone");
        Console.WriteLine("2. Smartphone");
        Console.WriteLine("3. Gaming Phone");
        Console.Write("Enter choice (1-3): ");

        if (!int.TryParse(Console.ReadLine(), out int phoneTypeChoice) || 
            phoneTypeChoice < 1 || phoneTypeChoice > 3)
        {
            Console.WriteLine("Invalid phone type selection.");
            return;
        }

        // Get common properties
        Console.Write("Enter model: ");
        string model = Console.ReadLine() ?? string.Empty;

        Console.Write("Enter serial number: ");
        string serialNumber = Console.ReadLine() ?? string.Empty;

        Console.Write("Enter battery capacity (mAh): ");
        if (!int.TryParse(Console.ReadLine(), out int batteryCapacity))
        {
            Console.WriteLine("Invalid battery capacity.");
            return;
        }

        Console.Write("Enter screen size (inches): ");
        if (!decimal.TryParse(Console.ReadLine(), out decimal screenSize))
        {
            Console.WriteLine("Invalid screen size.");
            return;
        }

        Phone phone;

        switch (phoneTypeChoice)
        {
            case 1: // Feature Phone
                Console.Write("Has physical keypad? (y/n): ");
                bool hasKeypad = Console.ReadLine()?.ToLower() == "y";

                Console.Write("SMS storage capacity: ");
                if (!int.TryParse(Console.ReadLine(), out int smsCapacity))
                {
                    Console.WriteLine("Invalid SMS capacity.");
                    return;
                }

                Console.Write("Supports basic games? (y/n): ");
                bool supportsGames = Console.ReadLine()?.ToLower() == "y";

                phone = new FeaturePhone
                {
                    Model = model,
                    SerialNumber = serialNumber,
                    ManufacturerId = manufacturerId,
                    BatteryCapacity = batteryCapacity,
                    ScreenSize = screenSize,
                    HasPhysicalKeypad = hasKeypad,
                    SmsStorageCapacity = smsCapacity,
                    SupportsBasicGames = supportsGames
                };
                break;

            case 2: // Smartphone
                Console.Write("Operating System: ");
                string os = Console.ReadLine() ?? string.Empty;

                Console.Write("RAM capacity (GB): ");
                if (!int.TryParse(Console.ReadLine(), out int ram))
                {
                    Console.WriteLine("Invalid RAM capacity.");
                    return;
                }

                Console.Write("Storage capacity (GB): ");
                if (!int.TryParse(Console.ReadLine(), out int storage))
                {
                    Console.WriteLine("Invalid storage capacity.");
                    return;
                }

                Console.Write("Camera resolution (MP): ");
                if (!decimal.TryParse(Console.ReadLine(), out decimal camera))
                {
                    Console.WriteLine("Invalid camera resolution.");
                    return;
                }

                Console.Write("Supports 5G? (y/n): ");
                bool supports5G = Console.ReadLine()?.ToLower() == "y";

                phone = new Smartphone
                {
                    Model = model,
                    SerialNumber = serialNumber,
                    ManufacturerId = manufacturerId,
                    BatteryCapacity = batteryCapacity,
                    ScreenSize = screenSize,
                    OperatingSystem = os,
                    RamCapacity = ram,
                    StorageCapacity = storage,
                    CameraResolution = camera,
                    Supports5G = supports5G
                };
                break;

            case 3: // Gaming Phone
                Console.Write("Operating System: ");
                string gamingOs = Console.ReadLine() ?? string.Empty;

                Console.Write("RAM capacity (GB): ");
                if (!int.TryParse(Console.ReadLine(), out int gamingRam))
                {
                    Console.WriteLine("Invalid RAM capacity.");
                    return;
                }

                Console.Write("Storage capacity (GB): ");
                if (!int.TryParse(Console.ReadLine(), out int gamingStorage))
                {
                    Console.WriteLine("Invalid storage capacity.");
                    return;
                }

                Console.Write("GPU model: ");
                string gpu = Console.ReadLine() ?? string.Empty;

                Console.Write("Refresh rate (Hz): ");
                if (!int.TryParse(Console.ReadLine(), out int refreshRate))
                {
                    Console.WriteLine("Invalid refresh rate.");
                    return;
                }

                Console.Write("Has gaming triggers? (y/n): ");
                bool hasTriggers = Console.ReadLine()?.ToLower() == "y";

                Console.Write("Cooling system: ");
                string cooling = Console.ReadLine() ?? string.Empty;

                phone = new GamingPhone
                {
                    Model = model,
                    SerialNumber = serialNumber,
                    ManufacturerId = manufacturerId,
                    BatteryCapacity = batteryCapacity,
                    ScreenSize = screenSize,
                    OperatingSystem = gamingOs,
                    RamCapacity = gamingRam,
                    StorageCapacity = gamingStorage,
                    GpuModel = gpu,
                    RefreshRate = refreshRate,
                    HasGamingTriggers = hasTriggers,
                    CoolingSystem = cooling
                };
                break;

            default:
                Console.WriteLine("Invalid selection.");
                return;
        }

        var addedPhone = await _phoneService.AddPhoneAsync(phone);
        Console.WriteLine($"Phone added successfully with ID: {addedPhone.Id}");
    }

    /// <summary>
    /// Searches for a phone by ID.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    private async Task SearchPhoneByIdAsync()
    {
        Console.WriteLine();
        Console.Write("Enter phone ID to search: ");

        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("Invalid ID format.");
            return;
        }

        var phone = await _phoneService.GetPhoneByIdAsync(id);

        if (phone == null)
        {
            Console.WriteLine($"No phone found with ID: {id}");
            return;
        }

        Console.WriteLine();
        Console.WriteLine("Phone Found:");
        Console.WriteLine("─".PadRight(50, '─'));
        phone.Print();
    }
}