using System.ComponentModel.DataAnnotations;

namespace PhoneInheritanceDemo.Models
{
    /// <summary>
    /// Represents a phone manufacturer with company information.
    /// </summary>
    public class Manufacturer
    {
        /// <summary>
        /// Gets or sets the unique identifier for the manufacturer.
        /// </summary>
        [Key]
        public int ManufacturerId { get; set; }

        /// <summary>
        /// Gets or sets the name of the manufacturer.
        /// </summary>
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the country where the manufacturer is based.
        /// </summary>
        [StringLength(100)]
        public string Country { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the year the manufacturer was founded.
        /// </summary>
        [Range(1800, 2030)]
        public int FoundedYear { get; set; }

        /// <summary>
        /// Gets or sets the collection of phones manufactured by this manufacturer.
        /// </summary>
        public virtual ICollection<Phone> Phones { get; set; } = new List<Phone>();

        /// <summary>
        /// Returns a string representation of the manufacturer.
        /// </summary>
        /// <returns>A formatted string containing manufacturer details</returns>
        public override string ToString()
        {
            return $"{Name} ({Country}, Founded: {FoundedYear})";
        }
    }
}