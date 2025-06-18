namespace CAmodels;

/// <summary>
/// Represents a phone with basic properties.
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
    public string Model { get; set; }

    /// <summary>
    /// Gets or sets the serial number of the phone.
    /// </summary>
    public string SerialNumber { get; set; }

    /// <summary>
    /// Gets or sets the phone type.
    /// </summary>
    public string PhoneType { get; set; }

    /// <summary>
    /// Prints phone information to the console.
    /// </summary>
    public void Print()
    {
        Console.WriteLine($"Phone: ID=HIDDEN, Model={Model}, SerialNumber={SerialNumber}, PhoneType={PhoneType}");
    }

    /// <summary>
    /// Returns a string representation of the phone.
    /// </summary>
    /// <returns>A string containing the phone's model, serial number, and type.</returns>
    public override string ToString()
    {
        return $"Phone: Model={Model}, SN={SerialNumber}, Type={PhoneType}";
    }
}
