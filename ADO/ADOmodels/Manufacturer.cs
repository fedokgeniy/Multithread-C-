using System;

namespace ManufacturerPhoneApp.Models
{
    /// <summary>
    /// Represents a manufacturer entity for database operations.
    /// </summary>
    public class Manufacturer
    {
        /// <summary>
        /// Gets or sets the unique identifier of the manufacturer.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the manufacturer's name.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the manufacturer's address.
        /// </summary>
        public string Address { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets whether the manufacturer is a child company.
        /// </summary>
        public bool IsAChildCompany { get; set; }

        /// <summary>
        /// Prints manufacturer information to the console.
        /// </summary>
        public void Print()
        {
            Console.WriteLine($"Manufacturer: ID={Id}, Name={Name}, Address={Address}, IsAChildCompany={IsAChildCompany}");
        }

        /// <summary>
        /// Returns a string representation of the manufacturer.
        /// </summary>
        /// <returns>A string containing the manufacturer's information.</returns>
        public override string ToString()
        {
            return $"Manufacturer: ID={Id}, Name={Name}, Address={Address}, IsAChildCompany={IsAChildCompany}";
        }
    }
}
