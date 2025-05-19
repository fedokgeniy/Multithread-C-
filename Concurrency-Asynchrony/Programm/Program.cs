using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;

namespace ConcurrencyAsynchrony
{
    class Program
    {
        static readonly string[] FileNames = { "file1.txt", "file2.txt", "file3.txt", "file4.txt", "file5.txt" };
        static readonly int TotalObjects = 50;
        static readonly int ObjectsPerFile = 10;

        // Потокобезопасный словарь: ключ - имя файла, значение - коллекция строк (записей)
        static ConcurrentDictionary<string, ConcurrentBag<string>> fileData = new();

        static void Main()
        {
            // Задание 0: Генерация и запись объектов в файлы
            GenerateAndSaveObjects();

            // Задание 1: Параллельное чтение файлов с ProgressBar
            ParallelReadFilesWithProgressBar();

            // Задание 2: Запуск обработчика сортировки
            var sorter = new Sorter(fileData);
            sorter.Start();

            // Даем время сортировщику поработать
            Thread.Sleep(2000);

            // Вывод результата после сортировки
            Console.WriteLine("\n--- Содержимое файлов после сортировки ---");
            foreach (var kvp in fileData)
            {
                Console.WriteLine($"\nФайл: {kvp.Key}");
                foreach (var entry in kvp.Value)
                    Console.WriteLine(entry);
            }

            sorter.Stop();
        }

        static void GenerateAndSaveObjects()
        {
            var objects = new List<object>();
            // 25 телефонов
            for (int i = 0; i < 25; i++)
            {
                objects.Add(new Phone
                {
                    Id = i + 1,
                    Model = $"Model_{i + 1}",
                    SerialNumber = $"SN_{1000 + i}",
                    PhoneType = (i % 2 == 0) ? "Smartphone" : "Landline"
                });
            }
            // 25 производителей
            for (int i = 0; i < 25; i++)
            {
                objects.Add(new Manufacturer
                {
                    Name = $"Manufacturer_{i + 1}",
                    Address = $"Address_{i + 1}",
                    IsAChildCompany = (i % 2 == 0)
                });
            }
            // Перемешаем для наглядности
            var rnd = new Random();
            objects = objects.OrderBy(_ => rnd.Next()).ToList();

            // Запись в 5 файлов по 10 объектов
            for (int f = 0; f < 5; f++)
            {
                using var sw = new StreamWriter(FileNames[f]);
                foreach (var obj in objects.Skip(f * ObjectsPerFile).Take(ObjectsPerFile))
                    sw.WriteLine(obj.ToString());
            }
            Console.WriteLine("Экземпляры записаны в файлы.\n");
        }

        static void ParallelReadFilesWithProgressBar()
        {
            var tasks = new List<Task>();
            foreach (var file in FileNames)
            {
                tasks.Add(Task.Run(() =>
                {
                    var bag = new ConcurrentBag<string>();
                    using var sr = new StreamReader(file);
                    int count = 0;
                    string? line;
                    Console.WriteLine($"\nЧтение {file}:");
                    while ((line = sr.ReadLine()) != null)
                    {
                        bag.Add(line);
                        count++;
                        ShowProgressBar(count, ObjectsPerFile);
                        Thread.Sleep(100); // Для наглядности прогресса
                    }
                    Console.WriteLine(); // Перенос строки после прогресс-бара
                    fileData[file] = bag;
                }));
            }
            Task.WaitAll(tasks.ToArray());

            // Выводим содержимое словаря после чтения
            Console.WriteLine("\n--- Содержимое файлов после чтения ---");
            foreach (var kvp in fileData)
            {
                Console.WriteLine($"\nФайл: {kvp.Key}");
                foreach (var entry in kvp.Value)
                    Console.WriteLine(entry);
            }
        }

        static void ShowProgressBar(int current, int total)
        {
            int width = 30;
            int progress = (int)((double)current / total * width);
            Console.Write("\r[");
            Console.Write(new string('█', progress));
            Console.Write(new string(' ', width - progress));
            Console.Write($"] {current}/{total}");
        }
    }

    // Класс-обработчик для сортировки значений по ID (или по алфавиту, если ID нет)
    class Sorter
    {
        private ConcurrentDictionary<string, ConcurrentBag<string>> _dict;
        private CancellationTokenSource _cts = new();

        public Sorter(ConcurrentDictionary<string, ConcurrentBag<string>> dict)
        {
            _dict = dict;
        }

        public void Start()
        {
            Task.Run(() =>
            {
                while (!_cts.Token.IsCancellationRequested)
                {
                    foreach (var key in _dict.Keys)
                    {
                        var sorted = _dict[key]
                            .OrderBy(ParseIdFromString)
                            .ThenBy(x => x)
                            .ToList();
                        _dict[key] = new ConcurrentBag<string>(sorted);
                    }
                    Thread.Sleep(500);
                }
            }, _cts.Token);
        }

        public void Stop() => _cts.Cancel();

        private int ParseIdFromString(string line)
        {
            // Для Phone: "Phone: Model=Model_1, SN=SN_1000, Type=Smartphone"
            // Для Manufacturer: "Manufacturer: Name=Manufacturer_1, Address=Address_1"
            if (line.StartsWith("Phone:"))
            {
                var modelPart = line.Split(',').FirstOrDefault(x => x.Contains("Model="));
                if (modelPart != null)
                {
                    var digits = new string(modelPart.Where(char.IsDigit).ToArray());
                    if (int.TryParse(digits, out int id))
                        return id;
                }
            }
            else if (line.StartsWith("Manufacturer:"))
            {
                var namePart = line.Split(',').FirstOrDefault(x => x.Contains("Name="));
                if (namePart != null)
                {
                    var digits = new string(namePart.Where(char.IsDigit).ToArray());
                    if (int.TryParse(digits, out int id))
                        return id + 1000; // Смещаем, чтобы не пересекалось с ID телефонов
                }
            }
            return int.MaxValue;
        }
    }
}