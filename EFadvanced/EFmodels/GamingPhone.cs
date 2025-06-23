using System.ComponentModel.DataAnnotations;

namespace PhoneInheritanceDemo.Models;

/// <summary>
/// Represents a gaming phone optimized for mobile gaming.
/// </summary>
public class GamingPhone : Phone
{
    /// <summary>
    /// Gets or sets the operating system of the gaming phone.
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
    /// Gets or sets the GPU model for enhanced gaming performance.
    /// </summary>
    [Required]
    [MaxLength(100)]
    public string GpuModel { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the refresh rate of the display in Hz.
    /// </summary>
    public int RefreshRate { get; set; }

    /// <summary>
    /// Gets or sets whether the phone has dedicated gaming triggers.
    /// </summary>
    public bool HasGamingTriggers { get; set; }

    /// <summary>
    /// Gets or sets the cooling system type.
    /// </summary>
    [MaxLength(100)]
    public string CoolingSystem { get; set; } = string.Empty;

    /// <summary>
    /// Prints gaming phone information to the console.
    /// </summary>
    public override void Print()
    {
        base.Print();
        Console.WriteLine($"  Gaming Phone: OS={OperatingSystem}, RAM={RamCapacity}GB, " +
                         $"Storage={StorageCapacity}GB, GPU={GpuModel}, Refresh={RefreshRate}Hz, " +
                         $"Gaming Triggers={HasGamingTriggers}, Cooling={CoolingSystem}");
    }
}