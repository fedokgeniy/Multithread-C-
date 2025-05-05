using MultithreadingModels;

namespace MultithreadingServices;

public static class DataGenerator
{
    private static readonly Random rand = new();

    private static string RandomString(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[rand.Next(s.Length)]).ToArray());
    }

    public static List<Phone> GeneratePhones(int count)
    {
        var list = new List<Phone>();
        for (int i = 0; i < count; i++)
        {
            list.Add(new Phone
            {
                Id = i + 1,
                Model = $"Model {RandomString(4)}",
                SerialNumber = $"SN{rand.Next(1000, 9999)}",
                PhoneType = $"Type {rand.Next(1, 4)}"
            });
        }

        return list;
    }

    public static List<Manufacturer> GenerateManufacturers(int count)
    {
        var list = new List<Manufacturer>();
        for (int i = 0; i < count; i++)
        {
            list.Add(new Manufacturer
            {
                Name = $"Company {RandomString(5)}",
                Address = $"Street {rand.Next(1, 100)}, City {RandomString(3)}",
                IsAChildCompany = rand.Next(0, 2) == 1
            });
        }

        return list;
    }
}