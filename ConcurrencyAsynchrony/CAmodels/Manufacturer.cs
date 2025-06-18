using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAmodels
{
    /// <summary>
    /// Represents a manufacturer with basic properties.
    /// </summary>
    public class Manufacturer
    {
        /// <summary>
        /// Gets or sets the manufacturer's name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the manufacturer's address.
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Gets or sets whether the manufacturer is a child company.
        /// </summary>
        public bool IsAChildCompany { get; set; }

        /// <summary>
        /// Prints manufacturer information to the console.
        /// </summary>
        public void Print()
        {
            Console.WriteLine($"Manufacturer: Name={Name}, Address={Address}, IsAChildCompany=HIDDEN");
        }

        /// <summary>
        /// Returns a string representation of the manufacturer.
        /// </summary>
        /// <returns>A string containing the manufacturer's name and address.</returns>
        public override string ToString()
        {
            return $"Manufacturer: Name={Name}, Address={Address}";
        }
    }
}
