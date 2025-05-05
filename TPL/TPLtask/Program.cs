using System;
using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;
using MultithreadingModels;
using MultithreadingServices;

class Program
{
    static readonly object fileLock = new();

    static void Main()
    {
        Console.WriteLine("=== PHONE DEMO ===");
        RunDemo<Phone>(DataGenerator.GeneratePhones(20), "phones1.xml", "phones2.xml", "phones_result.xml");

        Console.WriteLine("\n=== MANUFACTURER DEMO ===");
        RunDemo<Manufacturer>(DataGenerator.GenerateManufacturers(20), "manuf1.xml", "manuf2.xml", "manuf_result.xml");
    }

    static void RunDemo<T>(List<T> objects, string file1, string file2, string resultFile)
    {
        // Задание 1: Параллельная сериализация первых и вторых 10 объектов
        var firstHalf = objects.Take(10).ToList();
        var secondHalf = objects.Skip(10).Take(10).ToList();

        var task1 = Task.Run(() =>
        {
            lock (fileLock)
            {
                XmlSerializerService.SaveToXml(firstHalf, file1);
            }
        });

        var task2 = Task.Run(() =>
        {
            lock (fileLock)
            {
                XmlSerializerService.SaveToXml(secondHalf, file2);
            }
        });

        Task.WaitAll(task1, task2);

        // Задание 2: Параллельное чтение двух файлов и объединение в третий
        var readTask1 = Task.Run(() =>
        {
            lock (fileLock)
            {
                return XmlSerializerService.LoadFromXml<T>(file1);
            }
        });

        var readTask2 = Task.Run(() =>
        {
            lock (fileLock)
            {
                return XmlSerializerService.LoadFromXml<T>(file2);
            }
        });

        Task.WaitAll(readTask1, readTask2);

        var combined = new List<T>();
        combined.AddRange(readTask1.Result);
        combined.AddRange(readTask2.Result);

        lock (fileLock)
        {
            XmlSerializerService.SaveToXml(combined, resultFile);
        }

        // Задание 3: Асинхронное чтение из итогового файла и параллельный вывод
        Console.WriteLine($"\n--- Async reading from {resultFile} ---");
        ReadAndPrintAsync<T>(resultFile).GetAwaiter().GetResult();
    }

    static async Task ReadAndPrintAsync<T>(string path)
    {
        List<T> data;
        lock (fileLock)
        {
            data = XmlSerializerService.LoadFromXml<T>(path);
        }
        var tasks = data.Select(item => Task.Run(() => Console.WriteLine(item)));
        await Task.WhenAll(tasks);
    }
}
