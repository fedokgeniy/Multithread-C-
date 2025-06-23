using ManufacturerPhoneApp.Models;
using ManufacturerPhoneApp.Repositories;
using ManufacturerPhoneApp.Services;

namespace ManufacturerPhoneApp.UI
{
    /// <summary>
    /// Console menu methods - partial class continuation.
    /// </summary>
    public partial class ConsoleMenu
    {
        #region Manufacturer Operations

        /// <summary>
        /// Displays all manufacturers.
        /// </summary>
        private async Task ViewAllManufacturersAsync()
        {
            Console.WriteLine("\n=== All Manufacturers ===");
            var manufacturers = await _manufacturerRepository.GetAllAsync();

            if (!manufacturers.Any())
            {
                Console.WriteLine("No manufacturers found.");
                return;
            }

            foreach (var manufacturer in manufacturers)
            {
                manufacturer.Print();
            }
        }

        /// <summary>
        /// Displays a manufacturer by ID.
        /// </summary>
        private async Task ViewManufacturerByIdAsync()
        {
            Console.Write("Enter Manufacturer ID: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                var manufacturer = await _manufacturerRepository.GetByIdWithPhonesAsync(id);
                if (manufacturer != null)
                {
                    Console.WriteLine("\n=== Manufacturer Details ===");
                    manufacturer.Print();

                    if (manufacturer.Phones.Any())
                    {
                        Console.WriteLine("\n--- Associated Phones ---");
                        foreach (var phone in manufacturer.Phones)
                        {
                            phone.Print();
                        }
                    }
                    else
                    {
                        Console.WriteLine("No phones associated with this manufacturer.");
                    }
                }
                else
                {
                    Console.WriteLine("Manufacturer not found.");
                }
            }
            else
            {
                Console.WriteLine("Invalid ID format.");
            }
        }

        /// <summary>
        /// Adds a new manufacturer.
        /// </summary>
        private async Task AddManufacturerAsync()
        {
            Console.WriteLine("\n=== Add New Manufacturer ===");

            Console.Write("Enter Name: ");
            var name = Console.ReadLine();

            Console.Write("Enter Address: ");
            var address = Console.ReadLine();

            Console.Write("Is Child Company (y/n): ");
            var isChildInput = Console.ReadLine();
            bool isChildCompany = isChildInput?.ToLower() == "y" || isChildInput?.ToLower() == "yes";

            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(address))
            {
                Console.WriteLine("Name and Address are required.");
                return;
            }

            var manufacturer = new Manufacturer
            {
                Name = name,
                Address = address,
                IsAChildCompany = isChildCompany
            };

            try
            {
                await _manufacturerRepository.AddAsync(manufacturer);
                Console.WriteLine($"Manufacturer '{name}' added successfully with ID: {manufacturer.Id}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding manufacturer: {ex.Message}");
            }
        }

        /// <summary>
        /// Updates an existing manufacturer.
        /// </summary>
        private async Task UpdateManufacturerAsync()
        {
            Console.Write("Enter Manufacturer ID to update: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                var manufacturer = await _manufacturerRepository.GetByIdAsync(id);
                if (manufacturer == null)
                {
                    Console.WriteLine("Manufacturer not found.");
                    return;
                }

                Console.WriteLine($"Current Name: {manufacturer.Name}");
                Console.Write("Enter new Name (press Enter to keep current): ");
                var name = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(name))
                    manufacturer.Name = name;

                Console.WriteLine($"Current Address: {manufacturer.Address}");
                Console.Write("Enter new Address (press Enter to keep current): ");
                var address = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(address))
                    manufacturer.Address = address;

                Console.WriteLine($"Current Is Child Company: {manufacturer.IsAChildCompany}");
                Console.Write("Is Child Company (y/n, press Enter to keep current): ");
                var isChildInput = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(isChildInput))
                {
                    manufacturer.IsAChildCompany = isChildInput.ToLower() == "y" || isChildInput.ToLower() == "yes";
                }

                try
                {
                    await _manufacturerRepository.UpdateAsync(manufacturer);
                    Console.WriteLine("Manufacturer updated successfully.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error updating manufacturer: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("Invalid ID format.");
            }
        }

        /// <summary>
        /// Deletes a manufacturer.
        /// </summary>
        private async Task DeleteManufacturerAsync()
        {
            Console.Write("Enter Manufacturer ID to delete: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                var manufacturer = await _manufacturerRepository.GetByIdWithPhonesAsync(id);
                if (manufacturer == null)
                {
                    Console.WriteLine("Manufacturer not found.");
                    return;
                }

                Console.WriteLine("Manufacturer to delete:");
                manufacturer.Print();

                if (manufacturer.Phones.Any())
                {
                    Console.WriteLine($"Warning: This manufacturer has {manufacturer.Phones.Count} associated phones that will also be deleted.");
                }

                Console.Write("Are you sure you want to delete this manufacturer? (y/n): ");
                var confirmation = Console.ReadLine();

                if (confirmation?.ToLower() == "y" || confirmation?.ToLower() == "yes")
                {
                    try
                    {
                        await _manufacturerRepository.DeleteAsync(id);
                        Console.WriteLine("Manufacturer deleted successfully.");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error deleting manufacturer: {ex.Message}");
                    }
                }
                else
                {
                    Console.WriteLine("Delete operation cancelled.");
                }
            }
            else
            {
                Console.WriteLine("Invalid ID format.");
            }
        }

        /// <summary>
        /// Searches manufacturers by name.
        /// </summary>
        private async Task SearchManufacturersByNameAsync()
        {
            Console.Write("Enter manufacturer name to search: ");
            var name = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(name))
            {
                Console.WriteLine("Please enter a search term.");
                return;
            }

            var manufacturers = await _manufacturerRepository.GetByNameAsync(name);

            if (!manufacturers.Any())
            {
                Console.WriteLine($"No manufacturers found matching '{name}'.");
                return;
            }

            Console.WriteLine($"\n=== Manufacturers matching '{name}' ===");
            foreach (var manufacturer in manufacturers)
            {
                manufacturer.Print();
            }
        }

        #endregion

        #region Phone Operations

        /// <summary>
        /// Displays all phones.
        /// </summary>
        private async Task ViewAllPhonesAsync()
        {
            Console.WriteLine("\n=== All Phones ===");
            var phones = await _phoneRepository.GetAllWithManufacturersAsync();

            if (!phones.Any())
            {
                Console.WriteLine("No phones found.");
                return;
            }

            foreach (var phone in phones)
            {
                Console.WriteLine($"Phone: ID={phone.Id}, Model={phone.Model}, SN={phone.SerialNumber}, Type={phone.PhoneType}, Manufacturer={phone.Manufacturer?.Name}");
            }
        }

        /// <summary>
        /// Displays a phone by ID.
        /// </summary>
        private async Task ViewPhoneByIdAsync()
        {
            Console.Write("Enter Phone ID: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                var phone = await _phoneRepository.GetByIdAsync(id);
                if (phone != null)
                {
                    Console.WriteLine("\n=== Phone Details ===");
                    Console.WriteLine($"ID: {phone.Id}");
                    Console.WriteLine($"Model: {phone.Model}");
                    Console.WriteLine($"Serial Number: {phone.SerialNumber}");
                    Console.WriteLine($"Phone Type: {phone.PhoneType}");
                    Console.WriteLine($"Manufacturer: {phone.Manufacturer?.Name ?? "Unknown"}");
                }
                else
                {
                    Console.WriteLine("Phone not found.");
                }
            }
            else
            {
                Console.WriteLine("Invalid ID format.");
            }
        }

        /// <summary>
        /// Adds a new phone.
        /// </summary>
        private async Task AddPhoneAsync()
        {
            Console.WriteLine("\n=== Add New Phone ===");

            // First, show available manufacturers
            var manufacturers = await _manufacturerRepository.GetAllAsync();
            if (!manufacturers.Any())
            {
                Console.WriteLine("No manufacturers available. Please add a manufacturer first.");
                return;
            }

            Console.WriteLine("Available Manufacturers:");
            foreach (var m in manufacturers)
            {
                Console.WriteLine($"ID: {m.Id}, Name: {m.Name}");
            }

            Console.Write("Enter Manufacturer ID: ");
            if (!int.TryParse(Console.ReadLine(), out int manufacturerId))
            {
                Console.WriteLine("Invalid Manufacturer ID format.");
                return;
            }

            if (!await _manufacturerRepository.ExistsAsync(manufacturerId))
            {
                Console.WriteLine("Manufacturer not found.");
                return;
            }

            Console.Write("Enter Model: ");
            var model = Console.ReadLine();

            Console.Write("Enter Serial Number: ");
            var serialNumber = Console.ReadLine();

            Console.Write("Enter Phone Type: ");
            var phoneType = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(model) || string.IsNullOrWhiteSpace(serialNumber) || string.IsNullOrWhiteSpace(phoneType))
            {
                Console.WriteLine("All fields are required.");
                return;
            }

            // Check if serial number already exists
            var existingPhone = await _phoneRepository.GetBySerialNumberAsync(serialNumber);
            if (existingPhone != null)
            {
                Console.WriteLine("A phone with this serial number already exists.");
                return;
            }

            var phone = new Phone
            {
                Model = model,
                SerialNumber = serialNumber,
                PhoneType = phoneType,
                ManufacturerId = manufacturerId
            };

            try
            {
                await _phoneRepository.AddAsync(phone);
                Console.WriteLine($"Phone '{model}' added successfully with ID: {phone.Id}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding phone: {ex.Message}");
            }
        }

        /// <summary>
        /// Updates an existing phone.
        /// </summary>
        private async Task UpdatePhoneAsync()
        {
            Console.Write("Enter Phone ID to update: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                var phone = await _phoneRepository.GetByIdAsync(id);
                if (phone == null)
                {
                    Console.WriteLine("Phone not found.");
                    return;
                }

                Console.WriteLine($"Current Model: {phone.Model}");
                Console.Write("Enter new Model (press Enter to keep current): ");
                var model = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(model))
                    phone.Model = model;

                Console.WriteLine($"Current Serial Number: {phone.SerialNumber}");
                Console.Write("Enter new Serial Number (press Enter to keep current): ");
                var serialNumber = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(serialNumber))
                {
                    // Check if new serial number already exists
                    var existingPhone = await _phoneRepository.GetBySerialNumberAsync(serialNumber);
                    if (existingPhone != null && existingPhone.Id != phone.Id)
                    {
                        Console.WriteLine("A phone with this serial number already exists.");
                        return;
                    }
                    phone.SerialNumber = serialNumber;
                }

                Console.WriteLine($"Current Phone Type: {phone.PhoneType}");
                Console.Write("Enter new Phone Type (press Enter to keep current): ");
                var phoneType = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(phoneType))
                    phone.PhoneType = phoneType;

                try
                {
                    await _phoneRepository.UpdateAsync(phone);
                    Console.WriteLine("Phone updated successfully.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error updating phone: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("Invalid ID format.");
            }
        }

        /// <summary>
        /// Deletes a phone.
        /// </summary>
        private async Task DeletePhoneAsync()
        {
            Console.Write("Enter Phone ID to delete: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                var phone = await _phoneRepository.GetByIdAsync(id);
                if (phone == null)
                {
                    Console.WriteLine("Phone not found.");
                    return;
                }

                Console.WriteLine("Phone to delete:");
                Console.WriteLine($"ID: {phone.Id}, Model: {phone.Model}, SN: {phone.SerialNumber}, Manufacturer: {phone.Manufacturer?.Name}");

                Console.Write("Are you sure you want to delete this phone? (y/n): ");
                var confirmation = Console.ReadLine();

                if (confirmation?.ToLower() == "y" || confirmation?.ToLower() == "yes")
                {
                    try
                    {
                        await _phoneRepository.DeleteAsync(id);
                        Console.WriteLine("Phone deleted successfully.");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error deleting phone: {ex.Message}");
                    }
                }
                else
                {
                    Console.WriteLine("Delete operation cancelled.");
                }
            }
            else
            {
                Console.WriteLine("Invalid ID format.");
            }
        }

        /// <summary>
        /// Searches phones by model.
        /// </summary>
        private async Task SearchPhonesByModelAsync()
        {
            Console.Write("Enter phone model to search: ");
            var model = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(model))
            {
                Console.WriteLine("Please enter a search term.");
                return;
            }

            var phones = await _phoneRepository.GetByModelAsync(model);

            if (!phones.Any())
            {
                Console.WriteLine($"No phones found matching '{model}'.");
                return;
            }

            Console.WriteLine($"\n=== Phones matching '{model}' ===");
            foreach (var phone in phones)
            {
                Console.WriteLine($"ID: {phone.Id}, Model: {phone.Model}, SN: {phone.SerialNumber}, Manufacturer: {phone.Manufacturer?.Name}");
            }
        }

        /// <summary>
        /// Searches phone by serial number.
        /// </summary>
        private async Task SearchPhoneBySerialNumberAsync()
        {
            Console.Write("Enter phone serial number: ");
            var serialNumber = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(serialNumber))
            {
                Console.WriteLine("Please enter a serial number.");
                return;
            }

            var phone = await _phoneRepository.GetBySerialNumberAsync(serialNumber);

            if (phone == null)
            {
                Console.WriteLine($"No phone found with serial number '{serialNumber}'.");
                return;
            }

            Console.WriteLine("\n=== Phone Found ===");
            Console.WriteLine($"ID: {phone.Id}, Model: {phone.Model}, SN: {phone.SerialNumber}, Type: {phone.PhoneType}, Manufacturer: {phone.Manufacturer?.Name}");
        }

        #endregion

        #region Business Operations

        /// <summary>
        /// Adds a product for a new manufacturer using transaction.
        /// </summary>
        private async Task AddProductForNewManufacturerAsync()
        {
            Console.WriteLine("\n=== Add Product for New Manufacturer (with Transaction) ===");

            // Get manufacturer details
            Console.WriteLine("Enter Manufacturer Details:");
            Console.Write("Name: ");
            var manufacturerName = Console.ReadLine();

            Console.Write("Address: ");
            var manufacturerAddress = Console.ReadLine();

            Console.Write("Is Child Company (y/n): ");
            var isChildInput = Console.ReadLine();
            bool isChildCompany = isChildInput?.ToLower() == "y" || isChildInput?.ToLower() == "yes";

            // Get phone details
            Console.WriteLine("\nEnter Phone Details:");
            Console.Write("Model: ");
            var phoneModel = Console.ReadLine();

            Console.Write("Serial Number: ");
            var phoneSerialNumber = Console.ReadLine();

            Console.Write("Phone Type: ");
            var phoneType = Console.ReadLine();

            // Validate input
            if (string.IsNullOrWhiteSpace(manufacturerName) || string.IsNullOrWhiteSpace(manufacturerAddress) ||
                string.IsNullOrWhiteSpace(phoneModel) || string.IsNullOrWhiteSpace(phoneSerialNumber) || 
                string.IsNullOrWhiteSpace(phoneType))
            {
                Console.WriteLine("All fields are required.");
                return;
            }

            // Check if serial number already exists
            var existingPhone = await _phoneRepository.GetBySerialNumberAsync(phoneSerialNumber);
            if (existingPhone != null)
            {
                Console.WriteLine("A phone with this serial number already exists.");
                return;
            }

            var manufacturer = new Manufacturer
            {
                Name = manufacturerName,
                Address = manufacturerAddress,
                IsAChildCompany = isChildCompany
            };

            var phone = new Phone
            {
                Model = phoneModel,
                SerialNumber = phoneSerialNumber,
                PhoneType = phoneType
            };

            try
            {
                await _businessService.AddProductForNewManufacturerAsync(manufacturer, phone);
                Console.WriteLine("Transaction completed successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Transaction failed: {ex.Message}");
            }
        }

        /// <summary>
        /// Gets all products for a manufacturer by ID.
        /// </summary>
        private async Task GetProductsForManufacturerByIdAsync()
        {
            Console.Write("Enter Manufacturer ID: ");
            if (int.TryParse(Console.ReadLine(), out int manufacturerId))
            {
                var manufacturer = await _manufacturerRepository.GetByIdAsync(manufacturerId);
                if (manufacturer == null)
                {
                    Console.WriteLine("Manufacturer not found.");
                    return;
                }

                var phones = await _businessService.GetProductsForManufacturerAsync(manufacturerId);

                Console.WriteLine($"\n=== Products for {manufacturer.Name} ===");
                if (!phones.Any())
                {
                    Console.WriteLine("No products found for this manufacturer.");
                    return;
                }

                foreach (var phone in phones)
                {
                    Console.WriteLine($"ID: {phone.Id}, Model: {phone.Model}, SN: {phone.SerialNumber}, Type: {phone.PhoneType}");
                }
            }
            else
            {
                Console.WriteLine("Invalid ID format.");
            }
        }

        /// <summary>
        /// Gets all products for a manufacturer by name.
        /// </summary>
        private async Task GetProductsForManufacturerByNameAsync()
        {
            Console.Write("Enter Manufacturer Name: ");
            var manufacturerName = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(manufacturerName))
            {
                Console.WriteLine("Please enter a manufacturer name.");
                return;
            }

            var phones = await _businessService.GetProductsForManufacturerAsync(manufacturerName);

            Console.WriteLine($"\n=== Products for manufacturers matching '{manufacturerName}' ===");
            if (!phones.Any())
            {
                Console.WriteLine("No products found for this manufacturer.");
                return;
            }

            foreach (var phone in phones)
            {
                Console.WriteLine($"ID: {phone.Id}, Model: {phone.Model}, SN: {phone.SerialNumber}, Type: {phone.PhoneType}, Manufacturer: {phone.Manufacturer?.Name}");
            }
        }

        #endregion
    }
}
