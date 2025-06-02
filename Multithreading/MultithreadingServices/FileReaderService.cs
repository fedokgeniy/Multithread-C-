using MultithreadingServices;
using System.Diagnostics;

public static class FileReaderService
{
    public static void ReadSingleThreaded<T>(string path)
    {
        var stopwatch = Stopwatch.StartNew();
        var data = XmlSerializerService.LoadFromXml<T>(path);
        stopwatch.Stop();

        Console.WriteLine("=== [Single-threaded reading] ===");
        foreach (var item in data)
        {
            Console.WriteLine(item);
        }
        Console.WriteLine($"Time spent: {stopwatch.ElapsedMilliseconds} ms\n");
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
                Console.WriteLine($"[Thread #1] {item}");
            }
        });

        var task2 = Task.Run(() =>
        {
            foreach (var item in part2)
            {
                Console.WriteLine($"[Thread #2] {item}");
            }
        });

        Task.WaitAll(task1, task2);
        stopwatch.Stop();

        Console.WriteLine("=== [Two-threaded reading] ===");
        Console.WriteLine($"Time spent: {stopwatch.ElapsedMilliseconds} ms\n");
    }

    public static void ReadWithManualSemaphore<T>(string path)
    {
        const int maxConcurrent = 5;
        const int totalThreads = 10;

        var lockObj = new object();
        int currentActive = 0;

        var stopwatch = Stopwatch.StartNew();
        var data = XmlSerializerService.LoadFromXml<T>(path);

        var threads = new List<Thread>();

        for (int i = 0; i < totalThreads; i++)
        {
            int threadId = i + 1;
            var thread = new Thread(() =>
            {
                lock (lockObj)
                {
                    while (currentActive >= maxConcurrent)
                    {
                        Monitor.Wait(lockObj);
                    }
                    currentActive++;
                }

                try
                {
                    var sw = Stopwatch.StartNew();
                    Console.WriteLine($"[Thread #{threadId}] started");
                    foreach (var item in data)
                    {
                        Console.WriteLine($"[Thread #{threadId}] {item}");
                    }
                    sw.Stop();
                    Console.WriteLine($"[Thread #{threadId}] finished in {sw.ElapsedMilliseconds} ms");
                }
                finally
                {
                    lock (lockObj)
                    {
                        currentActive--;
                        Monitor.PulseAll(lockObj);
                    }
                }
            });

            threads.Add(thread);
            thread.Start();
        }

        foreach (var thread in threads)
        {
            thread.Join();
        }

        stopwatch.Stop();

        Console.WriteLine("=== [Ten-threaded reading with manual semaphore] ===");
        Console.WriteLine($"Time spent: {stopwatch.ElapsedMilliseconds} ms\n");
    }

}

