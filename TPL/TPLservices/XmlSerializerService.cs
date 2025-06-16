using System.Xml.Serialization;

namespace TPLservices;

/// <summary>
/// Provides XML serialization and deserialization utilities for lists of objects.
/// </summary>
public static class XmlSerializerService
{
    /// <summary>
    /// Serializes a list of objects to an XML file at the specified path.
    /// </summary>
    /// <param name="data">The list of objects to serialize.</param>
    /// <param name="path">The file path to save the XML data.</param>
    public static void SaveToXml<T>(List<T> data, string path)
    {
        var serializer = new XmlSerializer(typeof(List<T>));
        using var fs = new FileStream(path, FileMode.Create);
        serializer.Serialize(fs, data);
    }

    /// <summary>
    /// Deserializes a list of objects from an XML file at the specified path.
    /// </summary>
    /// <param name="path">The file path to read the XML data from.</param>
    /// <returns>The deserialized list of objects, or an empty list if the file does not exist.</returns>
    public static List<T> LoadFromXml<T>(string path)
    {
        if (!File.Exists(path))
            return new List<T>();

        var serializer = new XmlSerializer(typeof(List<T>));
        using var fs = new FileStream(path, FileMode.Open);
        return (List<T>)serializer.Deserialize(fs);
    }
}
