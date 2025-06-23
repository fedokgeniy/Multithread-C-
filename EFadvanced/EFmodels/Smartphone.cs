using System.ComponentModel.DataAnnotations;

namespace PhoneInheritanceDemo.Models;

/// <summary>
/// Represents a smartphone with advanced features.
/// </summary>
public class Smartphone : Phone
{
    /// <summary>
    /// Gets or sets the operating system of the smartphone.
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string OperatingSystem { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the RAM capacity in GB.
    /// </summary>
    public int RamCapacity { get; set; }

    /// <summary>
    /// Gets or sets the storage capacity in GB.
    /// </summary>
    public int StorageCapacity { get; set; }

    /// <summary>
    /// Gets or sets the camera resolution in megapixels.
    /// </summary>
    public decimal CameraResolution { get; set; }

    /// <summary>
    /// Gets or sets whether the phone supports 5G connectivity.
    /// </summary>
    public bool Supports5G { get; set; }

    /// <summary>
    /// Prints smartphone information to the console.
    /// </summary>
    public override void Print()
    {
        base.Print();
        Console.WriteLine($"  Smartphone: OS={OperatingSystem}, RAM={RamCapacity}GB, " +
                         $"Storage={StorageCapacity}GB, Camera={CameraResolution}MP, 5G={Supports5G}");
    }
}