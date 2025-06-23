using ManufacturerPhoneApp.Models;
using Microsoft.EntityFrameworkCore;

namespace ManufacturerPhoneApp.Data
{
    /// <summary>
    /// Provides methods to initialize the database with sample data.
    /// </summary>
    public static class DataInitializer
    {
        /// <summary>
        /// Seeds the database with initial data.
        /// </summary>
        /// <param name="context">The database context.</param>
        public static void Initialize(ManufacturerPhoneContext context)
        {
            // Ensure database is created
            context.Database.EnsureCreated();

            // Check if data already exists
            if (context.Manufacturers.Any())
            {
                return; // Database has been seeded
            }

            // Create manufacturers
            var manufacturers = CreateManufacturers();
            context.Manufacturers.AddRange(manufacturers);
            context.SaveChanges();

            // Create phones
            var phones = CreatePhones(manufacturers);
            context.Phones.AddRange(phones);
            context.SaveChanges();
        }

        /// <summary>
        /// Creates a list of sample manufacturers.
        /// </summary>
        /// <returns>A list of manufacturers.</returns>
        private static List<Manufacturer> CreateManufacturers()
        {
            var manufacturers = new List<Manufacturer>();
            var companyNames = new[]
            {
                "Apple Inc.", "Samsung Electronics", "Huawei Technologies", "Xiaomi Corporation",
                "OPPO Electronics", "Vivo Communication", "OnePlus Technology", "Google LLC",
                "Sony Corporation", "Nokia Corporation", "Motorola Mobility", "LG Electronics",
                "HTC Corporation", "BlackBerry Limited", "Nothing Technology", "Realme Mobile",
                "Honor Device", "Asus Computer", "Razer Inc.", "Fairphone BV",
                "Essential Products", "RED Digital Cinema", "Vertu Corporation", "Alcatel Mobile",
                "ZTE Corporation", "TCL Communication", "Lenovo Group", "Microsoft Corporation",
                "Amazon.com Inc.", "Meta Platforms"
            };

            var addresses = new[]
            {
                "1 Apple Park Way, Cupertino, CA, USA", "129 Samsung-ro, Suwon-si, South Korea",
                "Bantian, Longgang District, Shenzhen, China", "68 Qinghe Middle Street, Beijing, China",
                "18 Haibin Road, Dongguan, China", "283 BBK Road, Chang'an, Dongguan, China",
                "18 Haibin Road, Dongguan, China", "1600 Amphitheatre Parkway, Mountain View, CA",
                "1-7-1 Konan, Minato-ku, Tokyo, Japan", "Karaportti 3, Espoo, Finland",
                "222 W Merchandise Mart Plaza, Chicago, IL", "128 Yeoui-daero, Seoul, South Korea",
                "23 Xinghua Road, Taoyuan, Taiwan", "2200 University Avenue East, Waterloo, Canada",
                "Warehouse Way, London, UK", "No. 18 Haibin Road, Dongguan, China",
                "Bantian, Longgang District, Shenzhen, China", "15 Li-Te Road, Taipei, Taiwan",
                "1 Razer Way, Irvine, CA, USA", "Jollemanhof 17, Amsterdam, Netherlands",
                "1 Hacker Way, Menlo Park, CA, USA", "100 Universal City Plaza, CA, USA",
                "124 Horseferry Road, London, UK", "4 Rue Louis Pasteur, Boulogne-Billancourt, France",
                "55 Keji Road, Nanshan District, Shenzhen, China", "5 Rue Rene Panhard, Nanterre, France",
                "1009 Think Place, Morrisville, NC, USA", "One Microsoft Way, Redmond, WA, USA",
                "410 Terry Avenue North, Seattle, WA, USA", "1 Hacker Way, Menlo Park, CA, USA"
            };

            for (int i = 0; i < 30; i++)
            {
                manufacturers.Add(new Manufacturer
                {
                    Name = companyNames[i],
                    Address = addresses[i],
                    IsAChildCompany = i % 4 == 0 // Every 4th company is a child company
                });
            }

            return manufacturers;
        }

        /// <summary>
        /// Creates a list of sample phones.
        /// </summary>
        /// <param name="manufacturers">The list of manufacturers.</param>
        /// <returns>A list of phones.</returns>
        private static List<Phone> CreatePhones(List<Manufacturer> manufacturers)
        {
            var phones = new List<Phone>();
            var phoneModels = new[]
            {
                "iPhone 15 Pro", "Galaxy S24 Ultra", "P60 Pro", "Mi 14 Ultra", "Find X7 Ultra",
                "X100 Pro", "12 Pro", "Pixel 8 Pro", "Xperia 1 V", "G60 5G",
                "Edge 50 Ultra", "V70 ThinQ", "U24 Ultra", "KEY3", "Phone (2a)",
                "GT 6", "Magic 6 Pro", "ROG Phone 8", "Phone 4", "5G Eco",
                "PH-2", "Hydrogen Two", "Signature Touch", "3T 10", "Axon 50 Ultra",
                "50 5G", "Legion Y90", "Surface Duo 3", "Fire Phone 2", "Portal Phone"
            };

            var phoneTypes = new[]
            {
                "Smartphone", "Flagship", "Premium", "Gaming", "Business", "Rugged",
                "Foldable", "Compact", "Budget", "Mid-range"
            };

            var random = new Random(42); // Fixed seed for reproducible results

            for (int i = 0; i < 30; i++)
            {
                // Assign phones to manufacturers (each manufacturer gets at least one phone)
                var manufacturerIndex = i % manufacturers.Count;

                phones.Add(new Phone
                {
                    Model = phoneModels[i],
                    SerialNumber = $"SN{(i + 1):D6}{random.Next(1000, 9999)}",
                    PhoneType = phoneTypes[random.Next(phoneTypes.Length)],
                    ManufacturerId = manufacturers[manufacturerIndex].Id
                });
            }

            // Add additional phones to create more variety
            for (int i = 30; i < 60; i++)
            {
                var manufacturerIndex = random.Next(manufacturers.Count);
                var modelIndex = random.Next(phoneModels.Length);

                phones.Add(new Phone
                {
                    Model = $"{phoneModels[modelIndex]} {(i - 29)}",
                    SerialNumber = $"SN{(i + 1):D6}{random.Next(1000, 9999)}",
                    PhoneType = phoneTypes[random.Next(phoneTypes.Length)],
                    ManufacturerId = manufacturers[manufacturerIndex].Id
                });
            }

            return phones;
        }
    }
}
