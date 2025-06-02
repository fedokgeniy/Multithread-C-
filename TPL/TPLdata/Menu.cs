using MultithreadingData;
using MultithreadingModels;
using MultithreadingServices;
using System;
using System.Collections.Generic;

namespace MultithreadingData
{
    public static class Menu
    {
        public static readonly List<MenuItem> MenuItems = new()
        {
            new MenuItem
            {
                Key = "1",
                Description = "Generate 20 objects and serialize them into two files (multithreaded)",
                Action = () =>
                {
                    DataStore.phones = DataGenerator.GeneratePhones(20);

                    var firstHalf = DataStore.phones.GetRange(0, 10);
                    var secondHalf = DataStore.phones.GetRange(10, 10);

                    var t1 = Task.Run(() => XmlSerializerService.SaveToXml(firstHalf, Constants.phonesFile1));
                    var t2 = Task.Run(() => XmlSerializerService.SaveToXml(secondHalf, Constants.phonesFile2));
                    Task.WaitAll(t1, t2);

                    Console.WriteLine("20 objects have been generated and serialized into two files in parallel.");
                }
            },
            new MenuItem
            {
                Key = "2",
                Description = "Merge two files into a third one, alternating records (multithreaded)",
                Action = () =>
                {
                    ParallelMergerService.MergeAlternating<Phone>(Constants.phonesFile1, Constants.phonesFile2, Constants.phonesMergedFile);
                    Console.WriteLine("Files have been merged into a third file with alternating records.");
                }
            },
            new MenuItem
            {
                Key = "3",
                Description = "Single-threaded reading of the merged file with time output",
                Action = () =>
                {
                    FileReaderService.ReadSingleThreaded<Phone>(Constants.phonesMergedFile);
                }
            },
            new MenuItem
            {
                Key = "4",
                Description = "Two-threaded reading of the merged file with time output",
                Action = () =>
                {
                    FileReaderService.ReadTwoThreaded<Phone>(Constants.phonesMergedFile);
                }
            },
            new MenuItem
            {
                Key = "5",
                Description = "Ten threads read the file with a limit of 5 simultaneous threads (semaphore)",
                Action = () =>
                {
                    FileReaderService.ReadWithSemaphore<Phone>(Constants.phonesMergedFile);
                }
            },
            new MenuItem
            {
                Key = "0",
                Description = "Exit",
                Action = () => Environment.Exit(0)
            }
        };
    }
}

