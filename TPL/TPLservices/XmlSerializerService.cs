using System.Xml.Serialization;

namespace MultithreadingServices;

public static class XmlSerializerService
{
    public static void SaveToXml<T>(List<T> data, string path)
    {
        var serializer = new XmlSerializer(typeof(List<T>));
        using var fs = new FileStream(path, FileMode.Create);
        serializer.Serialize(fs, data);
    }

    public static List<T> LoadFromXml<T>(string path)
    {
        if (!File.Exists(path))
            return new List<T>();

        var serializer = new XmlSerializer(typeof(List<T>));
        using var fs = new FileStream(path, FileMode.Open);
        return (List<T>)serializer.Deserialize(fs);
    }
}
