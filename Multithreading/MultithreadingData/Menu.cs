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
                Description = "Сгенерировать 20 объектов и сериализовать их в два файла (многопоточно)",
                Action = () =>
                {
                    DataStore.phones = DataGenerator.GeneratePhones(20);

                    var firstHalf = DataStore.phones.GetRange(0, 10);
                    var secondHalf = DataStore.phones.GetRange(10, 10);

                    var t1 = Task.Run(() => XmlSerializerService.SaveToXml(firstHalf, Constants.phonesFile1));
                    var t2 = Task.Run(() => XmlSerializerService.SaveToXml(secondHalf, Constants.phonesFile2));
                    Task.WaitAll(t1, t2);

                    Console.WriteLine("20 объектов сгенерированы и сериализованы в два файла параллельно.");
                }
            },
            new MenuItem
            {
                Key = "2",
                Description = "Слить два файла в третий, чередуя записи (многопоточно)",
                Action = () =>
                {
                    ParallelMergerService.MergeAlternating<Phone>(Constants.phonesFile1, Constants.phonesFile2, Constants.phonesMergedFile);
                    Console.WriteLine("Файлы объединены в третий файл с чередованием записей.");
                }
            },
            new MenuItem
            {
                Key = "3",
                Description = "Однопоточное чтение объединённого файла с выводом времени",
                Action = () =>
                {
                    FileReaderService.ReadSingleThreaded<Phone>(Constants.phonesMergedFile);
                }
            },
            new MenuItem
            {
                Key = "4",
                Description = "Двухпоточное чтение объединённого файла с выводом времени",
                Action = () =>
                {
                    FileReaderService.ReadTwoThreaded<Phone>(Constants.phonesMergedFile);
                }
            },
            new MenuItem
            {
                Key = "5",
                Description = "Десять потоков читают файл с ограничением до 5 одновременных потоков (семафор)",
                Action = () =>
                {
                    FileReaderService.ReadWithSemaphore<Phone>(Constants.phonesMergedFile);
                }
            },
            new MenuItem
            {
                Key = "0",
                Description = "Выход",
                Action = () => Environment.Exit(0)
            }
        };
    }
}
