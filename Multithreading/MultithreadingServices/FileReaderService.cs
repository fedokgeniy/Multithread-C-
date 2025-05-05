using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MultithreadingServices
{
    public static class FileReaderService
    {
        public static void ReadSingleThreaded<T>(string path)
        {
            var stopwatch = Stopwatch.StartNew();
            var data = XmlSerializerService.LoadFromXml<T>(path);
            stopwatch.Stop();

            Console.WriteLine("=== [Single-threaded Reading] ===");
            foreach (var item in data)
            {
                Console.WriteLine(item);
            }
            Console.WriteLine($"Time elapsed: {stopwatch.ElapsedMilliseconds} ms\n");
        }

        public static void ReadTwoThreaded<T>(string path)
        {
            var stopwatch = Stopwatch.StartNew();
            var data = XmlSerializerService.LoadFromXml<T>(path);

            int mid = data.Count / 2;
            var part1 = data.Take(mid).ToList();
            var part2 = data.Skip(mid).ToList();

            var task1 = Task.Run(() =>
            {
                foreach (var item in part1)
                {
                    Console.WriteLine($"[T1] {item}");
                }
            });

            var task2 = Task.Run(() =>
            {
                foreach (var item in part2)
                {
                    Console.WriteLine($"[T2] {item}");
                }
            });

            Task.WaitAll(task1, task2);
            stopwatch.Stop();

            Console.WriteLine($"=== [Two-threaded Reading] ===");
            Console.WriteLine($"Time elapsed: {stopwatch.ElapsedMilliseconds} ms\n");
        }

        public static void ReadWithSemaphore<T>(string path)
        {
            const int maxConcurrent = 5;
            const int totalThreads = 10;

            var semaphore = new SemaphoreSlim(maxConcurrent, maxConcurrent);
            var stopwatch = Stopwatch.StartNew();
            var data = XmlSerializerService.LoadFromXml<T>(path);

            var tasks = new List<Task>();

            for (int i = 0; i < totalThreads; i++)
            {
                int threadId = i + 1;

                tasks.Add(Task.Run(async () =>
                {
                    await semaphore.WaitAsync();
                    try
                    {
                        var sw = Stopwatch.StartNew();
                        Console.WriteLine($"[T{threadId}] started");
                        foreach (var item in data)
                        {
                            Console.WriteLine($"[T{threadId}] {item}");
                        }
                        sw.Stop();
                        Console.WriteLine($"[T{threadId}] finished in {sw.ElapsedMilliseconds} ms");
                    }
                    finally
                    {
                        semaphore.Release();
                    }
                }));
            }

            Task.WaitAll(tasks.ToArray());
            stopwatch.Stop();

            Console.WriteLine("=== [Semaphore-limited Reading] ===");
            Console.WriteLine($"Total elapsed: {stopwatch.ElapsedMilliseconds} ms\n");
        }
    }
}
