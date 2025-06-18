using System;

namespace ManufacturerPhoneApp.Models
{
    /// <summary>
    /// Represents a phone entity for database operations.
    /// </summary>
    public class Phone
    {
        /// <summary>
        /// Gets or sets the unique identifier of the phone.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the phone model.
        /// </summary>
        public string Model { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the serial number of the phone.
        /// </summary>
        public string SerialNumber { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the phone type.
        /// </summary>
        public string PhoneType { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the manufacturer identifier (foreign key).
        /// </summary>
        public int ManufacturerId { get; set; }

        /// <summary>
        /// Gets or sets the manufacturer navigation property.
        /// </summary>
        public Manufacturer? Manufacturer { get; set; }

        /// <summary>
        /// Prints phone information to the console.
        /// </summary>
        public void Print()
        {
            Console.WriteLine($"Phone: ID={Id}, Model={Model}, SerialNumber={SerialNumber}, PhoneType={PhoneType}, ManufacturerId={ManufacturerId}");
        }

        /// <summary>
        /// Returns a string representation of the phone.
        /// </summary>
        /// <returns>A string containing the phone's information.</returns>
        public override string ToString()
        {
            return $"Phone: ID={Id}, Model={Model}, SN={SerialNumber}, Type={PhoneType}, ManufacturerId={ManufacturerId}";
        }
    }
}
