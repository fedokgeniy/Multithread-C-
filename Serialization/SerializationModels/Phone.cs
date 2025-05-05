namespace SerializationModels;

public class Phone
{
    public int Id { get; set; }
    public string Model { get; set; }
    public string SerialNumber { get; set; }
    public string PhoneType { get; set; }

    public void Print()
    {
        Console.WriteLine($"Phone: ID=HIDDEN, Model={Model}, SerialNumber={SerialNumber}, PhoneType={PhoneType}");
    }
}
