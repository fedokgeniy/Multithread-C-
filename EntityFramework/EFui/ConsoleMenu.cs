using ManufacturerPhoneApp.Models;
using ManufacturerPhoneApp.Repositories;
using ManufacturerPhoneApp.Services;

namespace ManufacturerPhoneApp.UI
{
    /// <summary>
    /// Provides console-based user interface for the application.
    /// </summary>
    public partial class ConsoleMenu
    {
        private readonly IManufacturerRepository _manufacturerRepository;
        private readonly IPhoneRepository _phoneRepository;
        private readonly IBusinessService _businessService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConsoleMenu"/> class.
        /// </summary>
        /// <param name="manufacturerRepository">The manufacturer repository.</param>
        /// <param name="phoneRepository">The phone repository.</param>
        /// <param name="businessService">The business service.</param>
        public ConsoleMenu(
            IManufacturerRepository manufacturerRepository,
            IPhoneRepository phoneRepository,
            IBusinessService businessService)
        {
            _manufacturerRepository = manufacturerRepository ?? throw new ArgumentNullException(nameof(manufacturerRepository));
            _phoneRepository = phoneRepository ?? throw new ArgumentNullException(nameof(phoneRepository));
            _businessService = businessService ?? throw new ArgumentNullException(nameof(businessService));
        }

        /// <summary>
        /// Displays the main menu and handles user input.
        /// </summary>
        public async Task ShowMainMenuAsync()
        {
            bool running = true;

            while (running)
            {
                Console.Clear();
                Console.WriteLine("=== Manufacturer-Phone Management System ===");
                Console.WriteLine();
                Console.WriteLine("1. Manufacturer Operations");
                Console.WriteLine("2. Phone Operations");
                Console.WriteLine("3. Business Operations");
                Console.WriteLine("4. Exit");
                Console.WriteLine();
                Console.Write("Select an option (1-4): ");

                var choice = Console.ReadLine();

                try
                {
                    switch (choice)
                    {
                        case "1":
                            await ShowManufacturerMenuAsync();
                            break;
                        case "2":
                            await ShowPhoneMenuAsync();
                            break;
                        case "3":
                            await ShowBusinessMenuAsync();
                            break;
                        case "4":
                            running = false;
                            Console.WriteLine("Goodbye!");
                            break;
                        default:
                            Console.WriteLine("Invalid option. Please try again.");
                            Console.WriteLine("Press any key to continue...");
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
        /// Displays the manufacturer operations menu.
        /// </summary>
        private async Task ShowManufacturerMenuAsync()
        {
            Console.Clear();
            Console.WriteLine("=== Manufacturer Operations ===");
            Console.WriteLine();
            Console.WriteLine("1. View All Manufacturers");
            Console.WriteLine("2. View Manufacturer by ID");
            Console.WriteLine("3. Add New Manufacturer");
            Console.WriteLine("4. Update Manufacturer");
            Console.WriteLine("5. Delete Manufacturer");
            Console.WriteLine("6. Search Manufacturers by Name");
            Console.WriteLine("7. Back to Main Menu");
            Console.WriteLine();
            Console.Write("Select an option (1-7): ");

            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    await ViewAllManufacturersAsync();
                    break;
                case "2":
                    await ViewManufacturerByIdAsync();
                    break;
                case "3":
                    await AddManufacturerAsync();
                    break;
                case "4":
                    await UpdateManufacturerAsync();
                    break;
                case "5":
                    await DeleteManufacturerAsync();
                    break;
                case "6":
                    await SearchManufacturersByNameAsync();
                    break;
                case "7":
                    return;
                default:
                    Console.WriteLine("Invalid option. Please try again.");
                    break;
            }

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        /// <summary>
        /// Displays the phone operations menu.
        /// </summary>
        private async Task ShowPhoneMenuAsync()
        {
            Console.Clear();
            Console.WriteLine("=== Phone Operations ===");
            Console.WriteLine();
            Console.WriteLine("1. View All Phones");
            Console.WriteLine("2. View Phone by ID");
            Console.WriteLine("3. Add New Phone");
            Console.WriteLine("4. Update Phone");
            Console.WriteLine("5. Delete Phone");
            Console.WriteLine("6. Search Phones by Model");
            Console.WriteLine("7. Search Phone by Serial Number");
            Console.WriteLine("8. Back to Main Menu");
            Console.WriteLine();
            Console.Write("Select an option (1-8): ");

            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    await ViewAllPhonesAsync();
                    break;
                case "2":
                    await ViewPhoneByIdAsync();
                    break;
                case "3":
                    await AddPhoneAsync();
                    break;
                case "4":
                    await UpdatePhoneAsync();
                    break;
                case "5":
                    await DeletePhoneAsync();
                    break;
                case "6":
                    await SearchPhonesByModelAsync();
                    break;
                case "7":
                    await SearchPhoneBySerialNumberAsync();
                    break;
                case "8":
                    return;
                default:
                    Console.WriteLine("Invalid option. Please try again.");
                    break;
            }

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        /// <summary>
        /// Displays the business operations menu.
        /// </summary>
        private async Task ShowBusinessMenuAsync()
        {
            Console.Clear();
            Console.WriteLine("=== Business Operations ===");
            Console.WriteLine();
            Console.WriteLine("1. Add Product for New Manufacturer (Transaction)");
            Console.WriteLine("2. Get All Products for Manufacturer by ID");
            Console.WriteLine("3. Get All Products for Manufacturer by Name");
            Console.WriteLine("4. Back to Main Menu");
            Console.WriteLine();
            Console.Write("Select an option (1-4): ");

            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    await AddProductForNewManufacturerAsync();
                    break;
                case "2":
                    await GetProductsForManufacturerByIdAsync();
                    break;
                case "3":
                    await GetProductsForManufacturerByNameAsync();
                    break;
                case "4":
                    return;
                default:
                    Console.WriteLine("Invalid option. Please try again.");
                    break;
            }

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
    }
}
