using System;
using System.Threading.Tasks;
using ManufacturerPhoneApp.Services;

namespace ManufacturerPhoneApp.UI
{
    /// <summary>
    /// Provides console-based user interface for the application.
    /// </summary>
    public class ConsoleInterface
    {
        private readonly ManufacturerService _manufacturerService;
        private readonly PhoneService _phoneService;
        private readonly DatabaseSeeder _databaseSeeder;

        /// <summary>
        /// Initializes a new instance of the ConsoleInterface class.
        /// </summary>
        /// <param name="manufacturerService">The manufacturer service.</param>
        /// <param name="phoneService">The phone service.</param>
        /// <param name="databaseSeeder">The database seeder.</param>
        public ConsoleInterface(ManufacturerService manufacturerService, PhoneService phoneService, DatabaseSeeder databaseSeeder)
        {
            _manufacturerService = manufacturerService ?? throw new ArgumentNullException(nameof(manufacturerService));
            _phoneService = phoneService ?? throw new ArgumentNullException(nameof(phoneService));
            _databaseSeeder = databaseSeeder ?? throw new ArgumentNullException(nameof(databaseSeeder));
        }

        /// <summary>
        /// Runs the main application loop.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task RunAsync()
        {
            Console.WriteLine("=== Manufacturer-Phone Management System ===");
            Console.WriteLine();

            while (true)
            {
                DisplayMainMenu();
                var choice = Console.ReadLine()?.Trim();

                try
                {
                    switch (choice)
                    {
                        case "1":
                            await SeedDatabaseAsync();
                            break;
                        case "2":
                            await ShowAllManufacturersAsync();
                            break;
                        case "3":
                            await ShowAllPhonesAsync();
                            break;
                        case "4":
                            await AddNewManufacturerAsync();
                            break;
                        case "5":
                            await AddNewPhoneAsync();
                            break;
                        case "6":
                            await ShowPhonesByManufacturerAsync();
                            break;
                        case "7":
                            await UpdateManufacturerAsync();
                            break;
                        case "8":
                            await UpdatePhoneAsync();
                            break;
                        case "9":
                            await DeleteManufacturerAsync();
                            break;
                        case "10":
                            await DeletePhoneAsync();
                            break;
                        case "0":
                            Console.WriteLine("Goodbye!");
                            return;
                        default:
                            Console.WriteLine("Invalid choice. Please try again.");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }

                Console.WriteLine();
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                Console.Clear();
            }
        }

        /// <summary>
        /// Displays the main menu.
        /// </summary>
        private static void DisplayMainMenu()
        {
            Console.WriteLine("Please choose an option:");
            Console.WriteLine("1. Seed database with sample data");
            Console.WriteLine("2. Show all manufacturers");
            Console.WriteLine("3. Show all phones");
            Console.WriteLine("4. Add new manufacturer");
            Console.WriteLine("5. Add new phone");
            Console.WriteLine("6. Show phones by manufacturer");
            Console.WriteLine("7. Update manufacturer");
            Console.WriteLine("8. Update phone");
            Console.WriteLine("9. Delete manufacturer");
            Console.WriteLine("10. Delete phone");
            Console.WriteLine("0. Exit");
            Console.Write("Enter your choice: ");
        }

        /// <summary>
        /// Seeds the database with sample data.
        /// </summary>
        private async Task SeedDatabaseAsync()
        {
            Console.WriteLine("Seeding database with sample data...");
            await _databaseSeeder.SeedDatabaseAsync();
        }

        /// <summary>
        /// Shows all manufacturers.
        /// </summary>
        private async Task ShowAllManufacturersAsync()
        {
            Console.WriteLine("All Manufacturers:");
            Console.WriteLine(new string('-', 80));

            var manufacturers = await _manufacturerService.GetAllManufacturersAsync();
            if (manufacturers.Count == 0)
            {
                Console.WriteLine("No manufacturers found.");
                return;
            }

            foreach (var manufacturer in manufacturers)
            {
                manufacturer.Print();
            }

            Console.WriteLine($"\nTotal: {manufacturers.Count} manufacturers");
        }

        /// <summary>
        /// Shows all phones.
        /// </summary>
        private async Task ShowAllPhonesAsync()
        {
            Console.WriteLine("All Phones:");
            Console.WriteLine(new string('-', 80));

            var phones = await _phoneService.GetAllPhonesAsync();
            if (phones.Count == 0)
            {
                Console.WriteLine("No phones found.");
                return;
            }

            foreach (var phone in phones)
            {
                phone.Print();
                if (phone.Manufacturer != null)
                {
                    Console.WriteLine($"  Manufacturer: {phone.Manufacturer.Name}");
                }
            }

            Console.WriteLine($"\nTotal: {phones.Count} phones");
        }

        /// <summary>
        /// Adds a new manufacturer.
        /// </summary>
        private async Task AddNewManufacturerAsync()
        {
            Console.WriteLine("Add New Manufacturer:");
            Console.WriteLine(new string('-', 40));

            Console.Write("Enter manufacturer name: ");
            var name = Console.ReadLine();

            Console.Write("Enter manufacturer address: ");
            var address = Console.ReadLine();

            Console.Write("Is this a child company? (y/n): ");
            var isChildInput = Console.ReadLine()?.ToLower().Trim();
            var isChild = isChildInput == "y" || isChildInput == "yes";

            var id = await _manufacturerService.CreateManufacturerAsync(name!, address!, isChild);
            Console.WriteLine($"Manufacturer added successfully with ID: {id}");
        }

        /// <summary>
        /// Adds a new phone.
        /// </summary>
        private async Task AddNewPhoneAsync()
        {
            Console.WriteLine("Add New Phone:");
            Console.WriteLine(new string('-', 40));

            // Show available manufacturers
            var manufacturers = await _manufacturerService.GetAllManufacturersAsync();
            if (manufacturers.Count == 0)
            {
                Console.WriteLine("No manufacturers available. Please add a manufacturer first.");
                return;
            }

            Console.WriteLine("Available manufacturers:");
            foreach (var manufacturer in manufacturers)
            {
                Console.WriteLine($"  {manufacturer.Id}: {manufacturer.Name}");
            }

            Console.Write("Enter phone model: ");
            var model = Console.ReadLine();

            Console.Write("Enter serial number: ");
            var serialNumber = Console.ReadLine();

            Console.Write("Enter phone type: ");
            var phoneType = Console.ReadLine();

            Console.Write("Enter manufacturer ID: ");
            if (!int.TryParse(Console.ReadLine(), out var manufacturerId))
            {
                Console.WriteLine("Invalid manufacturer ID.");
                return;
            }

            var id = await _phoneService.CreatePhoneAsync(model!, serialNumber!, phoneType!, manufacturerId);
            Console.WriteLine($"Phone added successfully with ID: {id}");
        }

        /// <summary>
        /// Shows phones by manufacturer.
        /// </summary>
        private async Task ShowPhonesByManufacturerAsync()
        {
            Console.WriteLine("Show Phones by Manufacturer:");
            Console.WriteLine(new string('-', 40));

            // Show available manufacturers
            var manufacturers = await _manufacturerService.GetAllManufacturersAsync();
            if (manufacturers.Count == 0)
            {
                Console.WriteLine("No manufacturers available.");
                return;
            }

            Console.WriteLine("Available manufacturers:");
            foreach (var manufacturer in manufacturers)
            {
                Console.WriteLine($"  {manufacturer.Id}: {manufacturer.Name}");
            }

            Console.Write("Enter manufacturer ID: ");
            if (!int.TryParse(Console.ReadLine(), out var manufacturerId))
            {
                Console.WriteLine("Invalid manufacturer ID.");
                return;
            }

            var manufacturer_info = await _manufacturerService.GetManufacturerByIdAsync(manufacturerId);
            if (manufacturer_info == null)
            {
                Console.WriteLine("Manufacturer not found.");
                return;
            }

            var phones = await _phoneService.GetPhonesByManufacturerIdAsync(manufacturerId);

            Console.WriteLine($"\nPhones for {manufacturer_info.Name}:");
            Console.WriteLine(new string('-', 80));

            if (phones.Count == 0)
            {
                Console.WriteLine("No phones found for this manufacturer.");
                return;
            }

            foreach (var phone in phones)
            {
                phone.Print();
            }

            Console.WriteLine($"\nTotal: {phones.Count} phones");
        }

        /// <summary>
        /// Updates a manufacturer.
        /// </summary>
        private async Task UpdateManufacturerAsync()
        {
            Console.WriteLine("Update Manufacturer:");
            Console.WriteLine(new string('-', 40));

            Console.Write("Enter manufacturer ID to update: ");
            if (!int.TryParse(Console.ReadLine(), out var id))
            {
                Console.WriteLine("Invalid manufacturer ID.");
                return;
            }

            var manufacturer = await _manufacturerService.GetManufacturerByIdAsync(id);
            if (manufacturer == null)
            {
                Console.WriteLine("Manufacturer not found.");
                return;
            }

            Console.WriteLine($"Current data: {manufacturer}");
            Console.WriteLine();

            Console.Write($"Enter new name (current: {manufacturer.Name}): ");
            var name = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(name))
                name = manufacturer.Name;

            Console.Write($"Enter new address (current: {manufacturer.Address}): ");
            var address = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(address))
                address = manufacturer.Address;

            Console.Write($"Is child company? (current: {manufacturer.IsAChildCompany}) (y/n): ");
            var isChildInput = Console.ReadLine()?.ToLower().Trim();
            var isChild = string.IsNullOrEmpty(isChildInput) ? manufacturer.IsAChildCompany : 
                         (isChildInput == "y" || isChildInput == "yes");

            var success = await _manufacturerService.UpdateManufacturerAsync(id, name, address, isChild);
            Console.WriteLine(success ? "Manufacturer updated successfully." : "Failed to update manufacturer.");
        }

        /// <summary>
        /// Updates a phone.
        /// </summary>
        private async Task UpdatePhoneAsync()
        {
            Console.WriteLine("Update Phone:");
            Console.WriteLine(new string('-', 40));

            Console.Write("Enter phone ID to update: ");
            if (!int.TryParse(Console.ReadLine(), out var id))
            {
                Console.WriteLine("Invalid phone ID.");
                return;
            }

            var phone = await _phoneService.GetPhoneByIdAsync(id);
            if (phone == null)
            {
                Console.WriteLine("Phone not found.");
                return;
            }

            Console.WriteLine($"Current data: {phone}");
            Console.WriteLine();

            Console.Write($"Enter new model (current: {phone.Model}): ");
            var model = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(model))
                model = phone.Model;

            Console.Write($"Enter new serial number (current: {phone.SerialNumber}): ");
            var serialNumber = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(serialNumber))
                serialNumber = phone.SerialNumber;

            Console.Write($"Enter new phone type (current: {phone.PhoneType}): ");
            var phoneType = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(phoneType))
                phoneType = phone.PhoneType;

            Console.Write($"Enter new manufacturer ID (current: {phone.ManufacturerId}): ");
            var manufacturerIdInput = Console.ReadLine();
            var manufacturerId = string.IsNullOrWhiteSpace(manufacturerIdInput) ? 
                               phone.ManufacturerId : 
                               int.Parse(manufacturerIdInput);

            var success = await _phoneService.UpdatePhoneAsync(id, model, serialNumber, phoneType, manufacturerId);
            Console.WriteLine(success ? "Phone updated successfully." : "Failed to update phone.");
        }

        /// <summary>
        /// Deletes a manufacturer.
        /// </summary>
        private async Task DeleteManufacturerAsync()
        {
            Console.WriteLine("Delete Manufacturer:");
            Console.WriteLine(new string('-', 40));

            Console.Write("Enter manufacturer ID to delete: ");
            if (!int.TryParse(Console.ReadLine(), out var id))
            {
                Console.WriteLine("Invalid manufacturer ID.");
                return;
            }

            var manufacturer = await _manufacturerService.GetManufacturerByIdAsync(id);
            if (manufacturer == null)
            {
                Console.WriteLine("Manufacturer not found.");
                return;
            }

            Console.WriteLine($"Are you sure you want to delete: {manufacturer}");
            Console.Write("This will also delete all associated phones. Continue? (y/n): ");
            var confirm = Console.ReadLine()?.ToLower().Trim();

            if (confirm == "y" || confirm == "yes")
            {
                var success = await _manufacturerService.DeleteManufacturerAsync(id);
                Console.WriteLine(success ? "Manufacturer deleted successfully." : "Failed to delete manufacturer.");
            }
            else
            {
                Console.WriteLine("Deletion cancelled.");
            }
        }

        /// <summary>
        /// Deletes a phone.
        /// </summary>
        private async Task DeletePhoneAsync()
        {
            Console.WriteLine("Delete Phone:");
            Console.WriteLine(new string('-', 40));

            Console.Write("Enter phone ID to delete: ");
            if (!int.TryParse(Console.ReadLine(), out var id))
            {
                Console.WriteLine("Invalid phone ID.");
                return;
            }

            var phone = await _phoneService.GetPhoneByIdAsync(id);
            if (phone == null)
            {
                Console.WriteLine("Phone not found.");
                return;
            }

            Console.WriteLine($"Are you sure you want to delete: {phone}");
            Console.Write("Continue? (y/n): ");
            var confirm = Console.ReadLine()?.ToLower().Trim();

            if (confirm == "y" || confirm == "yes")
            {
                var success = await _phoneService.DeletePhoneAsync(id);
                Console.WriteLine(success ? "Phone deleted successfully." : "Failed to delete phone.");
            }
            else
            {
                Console.WriteLine("Deletion cancelled.");
            }
        }
    }
}
