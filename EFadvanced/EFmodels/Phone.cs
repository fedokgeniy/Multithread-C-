using System.ComponentModel.DataAnnotations;

namespace PhoneInheritanceDemo.Models;

/// <summary>
/// Base class representing a phone with common properties.
/// </summary>
public abstract class Phone
{
    /// <summary>
    /// Gets or sets the unique identifier of the phone.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the phone model.
    /// </summary>
    [Required]
    [MaxLength(100)]
    public string Model { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the serial number of the phone.
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string SerialNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the manufacturer identifier (foreign key).
    /// </summary>
    public int ManufacturerId { get; set; }

    /// <summary>
    /// Gets or sets the manufacturer navigation property.
    /// </summary>
    public virtual Manufacturer? Manufacturer { get; set; }

    /// <summary>
    /// Gets or sets the battery capacity in mAh.
    /// </summary>
    public int BatteryCapacity { get; set; }

    /// <summary>
    /// Gets or sets the screen size in inches.
    /// </summary>
    public decimal ScreenSize { get; set; }

    /// <summary>
    /// Prints phone information to the console.
    /// </summary>
    public virtual void Print()
    {
        Console.WriteLine($"Phone: ID={Id}, Model={Model}, SerialNumber={SerialNumber}, " +
                         $"ManufacturerId={ManufacturerId}, Battery={BatteryCapacity}mAh, Screen={ScreenSize}"");
    }

    /// <summary>
    /// Returns a string representation of the phone.
    /// </summary>
    /// <returns>A string containing the phone's information.</returns>
    public override string ToString()
    {
        return $"Phone: ID={Id}, Model={Model}, SN={SerialNumber}, " +
               $"ManufacturerId={ManufacturerId}, Battery={BatteryCapacity}mAh";
    }
}