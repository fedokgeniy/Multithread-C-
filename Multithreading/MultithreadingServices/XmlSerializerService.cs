using System.Xml.Serialization;

namespace MultithreadingServices;

public static class XmlSerializerService
{
    public static void SaveToXml<T>(List<T> data, string path)
    {
        XmlSerializer serializer = new(typeof(List<T>));
        using FileStream fs = new(path, FileMode.Create);
        serializer.Serialize(fs, data);
    }

    public static List<T> LoadFromXml<T>(string path)
    {
        if (!File.Exists(path))
        {
            return new List<T>();
        }

        XmlSerializer serializer = new(typeof(List<T>));
        using FileStream fs = new(path, FileMode.Open);
        return (List<T>)serializer.Deserialize(fs);
    }
}
