using System.ComponentModel.DataAnnotations;

namespace PhoneInheritanceDemo.Models
{
    /// <summary>
    /// Abstract base class representing a phone with common properties.
    /// Serves as the base for inheritance strategies demonstration.
    /// </summary>
    public abstract class Phone
    {
        /// <summary>
        /// Gets or sets the unique identifier for the phone.
        /// </summary>
        [Key]
        public int PhoneId { get; set; }

        /// <summary>
        /// Gets or sets the phone model name.
        /// </summary>
        [Required]
        [StringLength(100)]
        public string Model { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the unique serial number of the phone.
        /// </summary>
        [Required]
        [StringLength(50)]
        public string SerialNumber { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the battery capacity in mAh.
        /// </summary>
        [Range(500, 10000)]
        public int BatteryCapacity { get; set; }

        /// <summary>
        /// Gets or sets the screen size in inches.
        /// </summary>
        [Range(1.0, 15.0)]
        public decimal ScreenSize { get; set; }

        /// <summary>
        /// Gets or sets the price of the phone.
        /// </summary>
        [Range(0.01, 99999.99)]
        public decimal Price { get; set; }

        /// <summary>
        /// Gets or sets the foreign key to the manufacturer.
        /// </summary>
        public int ManufacturerId { get; set; }

        /// <summary>
        /// Gets or sets the manufacturer of the phone.
        /// </summary>
        public virtual Manufacturer Manufacturer { get; set; } = null!;

        /// <summary>
        /// Returns a string representation of the phone with basic information.
        /// </summary>
        /// <returns>A formatted string containing phone details</returns>
        public virtual string GetPhoneInfo()
        {
            return $"{Manufacturer?.Name} {Model} - {ScreenSize} screen, {BatteryCapacity}mAh battery, ${Price:F2}";
        }

        /// <summary>
        /// Returns the type name of the phone for display purposes.
        /// </summary>
        /// <returns>The simple type name of the phone</returns>
        public virtual string GetPhoneType()
        {
            return GetType().Name;
        }
    }
}