using TPLmodels;

namespace TPLservices;

/// <summary>
/// Provides demo operations for generating, serializing, merging, and asynchronously reading phone data.
/// </summary>
public static class PhoneDemoService
{
    private static readonly object fileLock = new();

    /// <summary>
    /// The path to the first phones XML file.
    /// </summary>
    public const string phonesFile1 = "phones1.xml";

    /// <summary>
    /// The path to the second phones XML file.
    /// </summary>
    public const string phonesFile2 = "phones2.xml";

    /// <summary>
    /// The path to the merged phones XML file.
    /// </summary>
    public const string phonesMergedFile = "phones_merged.xml";

    /// <summary>
    /// Generates 20 phone instances and serializes them in parallel into two separate files.
    /// The first 10 phones are saved to one file, the second 10 to another.
    /// Uses parallel tasks and synchronization primitives for thread safety.
    /// </summary>
    public static void GenerateAndSerialize()
    {
        var phones = DataGenerator.GeneratePhones(20);
        var firstHalf = phones.Take(10).ToList();
        var secondHalf = phones.Skip(10).Take(10).ToList();

        var task1 = Task.Run(() => { lock (fileLock) { XmlSerializerService.SaveToXml(firstHalf, phonesFile1); } });
        var task2 = Task.Run(() => { lock (fileLock) { XmlSerializerService.SaveToXml(secondHalf, phonesFile2); } });

        Task.WaitAll(task1, task2);
        Console.WriteLine("Data is serialized into two files in parallel.");
    }

    /// <summary>
    /// Reads two phone files in parallel, merges the data, and saves the result to a third file.
    /// File access is synchronized for thread safety.
    /// </summary>
    public static void MergeFiles()
    {
        var readTask1 = Task.Run(() => { lock (fileLock) { return XmlSerializerService.LoadFromXml<Phone>(phonesFile1); } });
        var readTask2 = Task.Run(() => { lock (fileLock) { return XmlSerializerService.LoadFromXml<Phone>(phonesFile2); } });

        Task.WaitAll(readTask1, readTask2);

        var combined = new List<Phone>();
        combined.AddRange(readTask1.Result);
        combined.AddRange(readTask2.Result);

        lock (fileLock)
        {
            XmlSerializerService.SaveToXml(combined, phonesMergedFile);
        }

        Console.WriteLine("The files are combined into a third file.");
    }

    /// <summary>
    /// Asynchronously reads the merged phones file and prints each phone in parallel.
    /// </summary>
    /// <returns>A task representing the asynchronous print operation.</returns>
    public static async Task ReadAndPrintAsync()
    {
        List<Phone> data;
        lock (fileLock)
        {
            data = XmlSerializerService.LoadFromXml<Phone>(phonesMergedFile);
        }
        var tasks = data.Select(item => Task.Run(() => Console.WriteLine(item)));
        await Task.WhenAll(tasks);
        Console.WriteLine("Asynchronous reading and output completed.");
    }
}

