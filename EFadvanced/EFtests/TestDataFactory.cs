using System;
using System.Collections.Generic;
using PhoneInheritanceDemo.Models;

namespace PhoneInheritanceDemo.Tests.Utilities
{
    /// <summary>
    /// Factory class for creating test data objects.
    /// Provides consistent test data across all unit tests.
    /// </summary>
    public static class TestDataFactory
    {
        /// <summary>
        /// Creates a test manufacturer with default values.
        /// </summary>
        /// <param name="id">Optional manufacturer ID</param>
        /// <param name="name">Optional manufacturer name</param>
        /// <param name="country">Optional manufacturer country</param>
        /// <param name="foundedYear">Optional founded year</param>
        /// <returns>A test manufacturer instance</returns>
        public static Manufacturer CreateManufacturer(
            int id = 1, 
            string name = "Test Manufacturer", 
            string country = "Test Country", 
            int foundedYear = 2000)
        {
            return new Manufacturer
            {
                ManufacturerId = id,
                Name = name,
                Country = country,
                FoundedYear = foundedYear
            };
        }

        /// <summary>
        /// Creates a collection of test manufacturers.
        /// </summary>
        /// <returns>A collection of test manufacturers</returns>
        public static IEnumerable<Manufacturer> CreateManufacturers()
        {
            return new[]
            {
                CreateManufacturer(1, "Apple", "USA", 1976),
                CreateManufacturer(2, "Samsung", "South Korea", 1938),
                CreateManufacturer(3, "Nokia", "Finland", 1865),
                CreateManufacturer(4, "ASUS ROG", "Taiwan", 1989)
            };
        }

        /// <summary>
        /// Creates a test smartphone with default values.
        /// </summary>
        /// <param name="id">Optional phone ID</param>
        /// <param name="model">Optional model name</param>
        /// <param name="manufacturerId">Optional manufacturer ID</param>
        /// <param name="manufacturer">Optional manufacturer instance</param>
        /// <returns>A test smartphone instance</returns>
        public static Smartphone CreateSmartphone(
            int id = 1,
            string model = "Test Smartphone",
            int manufacturerId = 1,
            Manufacturer? manufacturer = null)
        {
            return new Smartphone
            {
                PhoneId = id,
                Model = model,
                SerialNumber = $"TEST-SP-{id:000}",
                BatteryCapacity = 4000,
                ScreenSize = 6.0m,
                Price = 799.99m,
                ManufacturerId = manufacturerId,
                Manufacturer = manufacturer,
                OperatingSystem = "Test OS",
                RamSize = 8,
                StorageSize = 128,
                CameraResolution = 48,
                HasFiveG = true
            };
        }

        /// <summary>
        /// Creates a test feature phone with default values.
        /// </summary>
        /// <param name="id">Optional phone ID</param>
        /// <param name="model">Optional model name</param>
        /// <param name="manufacturerId">Optional manufacturer ID</param>
        /// <param name="manufacturer">Optional manufacturer instance</param>
        /// <returns>A test feature phone instance</returns>
        public static FeaturePhone CreateFeaturePhone(
            int id = 2,
            string model = "Test Feature Phone",
            int manufacturerId = 1,
            Manufacturer? manufacturer = null)
        {
            return new FeaturePhone
            {
                PhoneId = id,
                Model = model,
                SerialNumber = $"TEST-FP-{id:000}",
                BatteryCapacity = 1200,
                ScreenSize = 2.4m,
                Price = 59.99m,
                ManufacturerId = manufacturerId,
                Manufacturer = manufacturer,
                HasPhysicalKeyboard = true,
                SmsStorageCapacity = 300,
                HasBasicGames = true
            };
        }

        /// <summary>
        /// Creates a test gaming phone with default values.
        /// </summary>
        /// <param name="id">Optional phone ID</param>
        /// <param name="model">Optional model name</param>
        /// <param name="manufacturerId">Optional manufacturer ID</param>
        /// <param name="manufacturer">Optional manufacturer instance</param>
        /// <returns>A test gaming phone instance</returns>
        public static GamingPhone CreateGamingPhone(
            int id = 3,
            string model = "Test Gaming Phone",
            int manufacturerId = 1,
            Manufacturer? manufacturer = null)
        {
            return new GamingPhone
            {
                PhoneId = id,
                Model = model,
                SerialNumber = $"TEST-GP-{id:000}",
                BatteryCapacity = 6000,
                ScreenSize = 6.8m,
                Price = 1099.99m,
                ManufacturerId = manufacturerId,
                Manufacturer = manufacturer,
                GpuName = "Test GPU",
                RefreshRate = 120,
                HasGamingTriggers = true,
                CoolingSystem = "Test Cooling"
            };
        }

        /// <summary>
        /// Creates a collection of mixed phone types for testing.
        /// </summary>
        /// <param name="manufacturer">Optional manufacturer to assign to all phones</param>
        /// <returns>A collection of test phones</returns>
        public static IEnumerable<Phone> CreateMixedPhones(Manufacturer? manufacturer = null)
        {
            var testManufacturer = manufacturer ?? CreateManufacturer();

            return new Phone[]
            {
                CreateSmartphone(1, "Test iPhone", testManufacturer.ManufacturerId, testManufacturer),
                CreateFeaturePhone(2, "Test Nokia", testManufacturer.ManufacturerId, testManufacturer),
                CreateGamingPhone(3, "Test ROG", testManufacturer.ManufacturerId, testManufacturer)
            };
        }

        /// <summary>
        /// Creates a smartphone with realistic Apple iPhone data.
        /// </summary>
        /// <returns>A realistic iPhone test instance</returns>
        public static Smartphone CreateiPhone()
        {
            var apple = CreateManufacturer(1, "Apple", "USA", 1976);
            return new Smartphone
            {
                PhoneId = 1,
                Model = "iPhone 15 Pro",
                SerialNumber = "APL-15PRO-001",
                BatteryCapacity = 3274,
                ScreenSize = 6.1m,
                Price = 1199.99m,
                ManufacturerId = apple.ManufacturerId,
                Manufacturer = apple,
                OperatingSystem = "iOS 17",
                RamSize = 8,
                StorageSize = 256,
                CameraResolution = 48,
                HasFiveG = true
            };
        }

        /// <summary>
        /// Creates a feature phone with realistic Nokia data.
        /// </summary>
        /// <returns>A realistic Nokia test instance</returns>
        public static FeaturePhone CreateNokia3310()
        {
            var nokia = CreateManufacturer(3, "Nokia", "Finland", 1865);
            return new FeaturePhone
            {
                PhoneId = 2,
                Model = "Nokia 3310",
                SerialNumber = "NOK-3310-001",
                BatteryCapacity = 1200,
                ScreenSize = 2.4m,
                Price = 59.99m,
                ManufacturerId = nokia.ManufacturerId,
                Manufacturer = nokia,
                HasPhysicalKeyboard = true,
                SmsStorageCapacity = 300,
                HasBasicGames = true
            };
        }

        /// <summary>
        /// Creates a gaming phone with realistic ASUS ROG data.
        /// </summary>
        /// <returns>A realistic ROG phone test instance</returns>
        public static GamingPhone CreateROGPhone()
        {
            var asus = CreateManufacturer(4, "ASUS ROG", "Taiwan", 1989);
            return new GamingPhone
            {
                PhoneId = 3,
                Model = "ROG Phone 8",
                SerialNumber = "ASUS-ROG8-001",
                BatteryCapacity = 6000,
                ScreenSize = 6.78m,
                Price = 1099.99m,
                ManufacturerId = asus.ManufacturerId,
                Manufacturer = asus,
                GpuName = "Adreno 750",
                RefreshRate = 165,
                HasGamingTriggers = true,
                CoolingSystem = "Vapor Chamber"
            };
        }

        /// <summary>
        /// Creates an invalid phone for testing validation.
        /// </summary>
        /// <returns>A phone with invalid data</returns>
        public static Smartphone CreateInvalidPhone()
        {
            return new Smartphone
            {
                PhoneId = -1,
                Model = "",
                SerialNumber = null!,
                BatteryCapacity = -100,
                ScreenSize = -1.0m,
                Price = -999.99m,
                ManufacturerId = 0,
                OperatingSystem = "", 
                RamSize = -1, 
                StorageSize = -1, 
                CameraResolution = -1, 
                HasFiveG = false
            };
        }
    }
}