using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ManufacturerPhoneApp.Interfaces;
using ManufacturerPhoneApp.Models;

namespace ManufacturerPhoneApp.Services
{
    /// <summary>
    /// Seeds the database with initial data.
    /// </summary>
    public class DatabaseSeeder
    {
        private readonly IManufacturerRepository _manufacturerRepository;
        private readonly IPhoneRepository _phoneRepository;

        /// <summary>
        /// Initializes a new instance of the DatabaseSeeder class.
        /// </summary>
        /// <param name="manufacturerRepository">The manufacturer repository.</param>
        /// <param name="phoneRepository">The phone repository.</param>
        public DatabaseSeeder(IManufacturerRepository manufacturerRepository, IPhoneRepository phoneRepository)
        {
            _manufacturerRepository = manufacturerRepository ?? throw new ArgumentNullException(nameof(manufacturerRepository));
            _phoneRepository = phoneRepository ?? throw new ArgumentNullException(nameof(phoneRepository));
        }

        /// <summary>
        /// Seeds the database with 30 manufacturers and corresponding phones asynchronously.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task SeedDatabaseAsync()
        {
            try
            {
                Console.WriteLine("Starting database seeding...");

                // Check if data already exists
                var existingManufacturers = await _manufacturerRepository.GetAllAsync();
                if (existingManufacturers.Count > 0)
                {
                    Console.WriteLine("Database already contains data. Skipping seeding.");
                    return;
                }

                var manufacturers = GenerateManufacturers();
                var phoneTypes = new[] { "Smartphone", "Feature Phone", "Satellite Phone", "Cordless Phone", "VoIP Phone" };

                for (int i = 0; i < manufacturers.Count; i++)
                {
                    var manufacturer = manufacturers[i];

                    // Add manufacturer
                    var manufacturerId = await _manufacturerRepository.AddAsync(manufacturer);
                    Console.WriteLine($"Added manufacturer {i + 1}/30: {manufacturer.Name}");

                    // Add phones for this manufacturer
                    var phones = GeneratePhonesForManufacturer(manufacturerId, manufacturer.Name, phoneTypes);
                    foreach (var phone in phones)
                    {
                        await _phoneRepository.AddAsync(phone);
                    }

                    Console.WriteLine($"Added {phones.Count} phones for {manufacturer.Name}");

                    // Small delay to demonstrate async operation
                    await Task.Delay(50);
                }

                Console.WriteLine("Database seeding completed successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error seeding database: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Generates a list of 30 manufacturers.
        /// </summary>
        /// <returns>A list of manufacturers.</returns>
        private static List<Manufacturer> GenerateManufacturers()
        {
            var manufacturers = new List<Manufacturer>();
            var random = new Random();

            var manufacturerData = new[]
            {
                ("Apple Inc.", "Cupertino, CA, USA", false),
                ("Samsung Electronics", "Seoul, South Korea", false),
                ("Google LLC", "Mountain View, CA, USA", false),
                ("Huawei Technologies", "Shenzhen, China", false),
                ("Xiaomi Corporation", "Beijing, China", false),
                ("OnePlus Technology", "Shenzhen, China", true),
                ("OPPO Electronics", "Dongguan, China", false),
                ("Vivo Communication", "Dongguan, China", true),
                ("Nokia Corporation", "Espoo, Finland", false),
                ("Sony Mobile Communications", "Tokyo, Japan", true),
                ("LG Electronics", "Seoul, South Korea", false),
                ("Motorola Mobility", "Chicago, IL, USA", true),
                ("HTC Corporation", "Taoyuan, Taiwan", false),
                ("BlackBerry Limited", "Waterloo, ON, Canada", false),
                ("Realme Technology", "Shenzhen, China", true),
                ("Honor Device", "Shenzhen, China", true),
                ("Nothing Technology", "London, UK", false),
                ("Fairphone B.V.", "Amsterdam, Netherlands", false),
                ("Essential Products", "Palo Alto, CA, USA", false),
                ("Red Magic Gaming", "Shenzhen, China", true),
                ("Asus Mobile", "Taipei, Taiwan", true),
                ("Lenovo Mobile", "Beijing, China", true),
                ("ZTE Corporation", "Shenzhen, China", false),
                ("TCL Communication", "Shenzhen, China", false),
                ("Alcatel Mobile", "Paris, France", true),
                ("CAT Phones", "Reading, UK", true),
                ("Kyocera Mobile", "Kyoto, Japan", true),
                ("Panasonic Mobile", "Osaka, Japan", true),
                ("Sharp Mobile", "Sakai, Japan", true),
                ("Casio Mobile", "Tokyo, Japan", true)
            };

            for (int i = 0; i < manufacturerData.Length; i++)
            {
                var (name, address, isChild) = manufacturerData[i];
                manufacturers.Add(new Manufacturer
                {
                    Name = name,
                    Address = address,
                    IsAChildCompany = isChild
                });
            }

            return manufacturers;
        }

        /// <summary>
        /// Generates phones for a specific manufacturer.
        /// </summary>
        /// <param name="manufacturerId">The manufacturer ID.</param>
        /// <param name="manufacturerName">The manufacturer name.</param>
        /// <param name="phoneTypes">Available phone types.</param>
        /// <returns>A list of phones.</returns>
        private static List<Phone> GeneratePhonesForManufacturer(int manufacturerId, string manufacturerName, string[] phoneTypes)
        {
            var phones = new List<Phone>();
            var random = new Random();
            var phoneCount = random.Next(1, 4); // 1-3 phones per manufacturer

            var phoneModels = GetPhoneModelsForManufacturer(manufacturerName);

            for (int i = 0; i < phoneCount && i < phoneModels.Length; i++)
            {
                var serialNumber = GenerateSerialNumber(manufacturerName, i + 1);

                phones.Add(new Phone
                {
                    Model = phoneModels[i],
                    SerialNumber = serialNumber,
                    PhoneType = phoneTypes[random.Next(phoneTypes.Length)],
                    ManufacturerId = manufacturerId
                });
            }

            return phones;
        }

        /// <summary>
        /// Gets phone models for a specific manufacturer.
        /// </summary>
        /// <param name="manufacturerName">The manufacturer name.</param>
        /// <returns>An array of phone model names.</returns>
        private static string[] GetPhoneModelsForManufacturer(string manufacturerName)
        {
            return manufacturerName switch
            {
                "Apple Inc." => new[] { "iPhone 15 Pro", "iPhone 15", "iPhone 14 Pro Max" },
                "Samsung Electronics" => new[] { "Galaxy S24 Ultra", "Galaxy S24", "Galaxy Note 23" },
                "Google LLC" => new[] { "Pixel 8 Pro", "Pixel 8", "Pixel 7a" },
                "Huawei Technologies" => new[] { "P60 Pro", "Mate 60", "Nova 11" },
                "Xiaomi Corporation" => new[] { "Mi 14 Pro", "Redmi Note 13", "POCO X6" },
                "OnePlus Technology" => new[] { "OnePlus 12", "OnePlus 11T", "OnePlus Nord 3" },
                "OPPO Electronics" => new[] { "Find X7 Pro", "Reno 11", "A98" },
                "Vivo Communication" => new[] { "X100 Pro", "V30", "Y100" },
                "Nokia Corporation" => new[] { "G60 5G", "X30", "C32" },
                "Sony Mobile Communications" => new[] { "Xperia 1 V", "Xperia 5 V", "Xperia 10 V" },
                _ => new[] { $"{manufacturerName.Split(' ')[0]} Model X", $"{manufacturerName.Split(' ')[0]} Pro", $"{manufacturerName.Split(' ')[0]} Lite" }
            };
        }

        /// <summary>
        /// Generates a unique serial number.
        /// </summary>
        /// <param name="manufacturerName">The manufacturer name.</param>
        /// <param name="phoneIndex">The phone index.</param>
        /// <returns>A unique serial number.</returns>
        private static string GenerateSerialNumber(string manufacturerName, int phoneIndex)
        {
            var prefix = manufacturerName.Length >= 3 ? manufacturerName[..3].ToUpper() : manufacturerName.ToUpper();
            var timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            var random = new Random().Next(1000, 9999);

            return $"{prefix}{timestamp % 100000}{phoneIndex:D2}{random}";
        }
    }
}
