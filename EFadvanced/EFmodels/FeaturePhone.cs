using System.ComponentModel.DataAnnotations;

namespace PhoneInheritanceDemo.Models;

/// <summary>
/// Represents a feature phone with basic functionality.
/// </summary>
public class FeaturePhone : Phone
{
    /// <summary>
    /// Gets or sets whether the phone has a physical keypad.
    /// </summary>
    public bool HasPhysicalKeypad { get; set; }

    /// <summary>
    /// Gets or sets the maximum SMS storage capacity.
    /// </summary>
    public int SmsStorageCapacity { get; set; }

    /// <summary>
    /// Gets or sets whether the phone supports basic games.
    /// </summary>
    public bool SupportsBasicGames { get; set; }

    /// <summary>
    /// Prints feature phone information to the console.
    /// </summary>
    public override void Print()
    {
        base.Print();
        Console.WriteLine($"  Feature Phone: Keypad={HasPhysicalKeypad}, SMS Storage={SmsStorageCapacity}, " +
                         $"Basic Games={SupportsBasicGames}");
    }
}