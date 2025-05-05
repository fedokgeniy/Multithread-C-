using SerializationModels;
using SerializationServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerializationData
{
    public static class Menu
    {
        public static readonly List<MenuItem> MenuItems = new()
        {
            new MenuItem
            {
                Key = "1",
                Description = "Создать 10 телефонов и вывести",
                Action = () =>
                {
                    DataStore.phones = DataGenerator.GeneratePhones(10);
                    DataStore.phones.ForEach(p => p.Print());
                }
            },
            new MenuItem
            {
                Key = "2",
                Description = "Создать 10 производителей и вывести",
                Action = () =>
                {
                    DataStore.manufacturers = DataGenerator.GenerateManufacturers(10);
                    DataStore.manufacturers.ForEach(m => m.Print());
                }
            },
            new MenuItem
            {
                Key = "3",
                Description = "Сериализовать телефоны",
                Action = () =>
                {
                    XmlSerializerService.SaveToXml(DataStore.phones, Constants.phonesFile);
                    Console.WriteLine("Телефоны сериализованы.");
                }
            },
            new MenuItem
            {
                Key = "4",
                Description = "Сериализовать производителей",
                Action = () =>
                {
                    XmlSerializerService.SaveToXml(DataStore.manufacturers, Constants.manufacturersFile);
                    Console.WriteLine("Производители сериализованы.");
                }
            },
            new MenuItem
            {
                Key = "5",
                Description = "Показать телефоны из XML",
                Action = () =>
                {
                    var loadedPhones = XmlSerializerService.LoadFromXml<Phone>(Constants.phonesFile);
                    if (loadedPhones.Count == 0) Console.WriteLine("Файл пуст.");
                    else loadedPhones.ForEach(p => p.Print());
                }
            },
            new MenuItem
            {
                Key = "6",
                Description = "Показать производителей из XML",
                Action = () =>
                {
                    var loadedManufacturers = XmlSerializerService.LoadFromXml<Manufacturer>(Constants.manufacturersFile);
                    if (loadedManufacturers.Count == 0) Console.WriteLine("Файл пуст.");
                    else loadedManufacturers.ForEach(m => m.Print());
                }
            },
            new MenuItem
            {
                Key = "7",
                Description = "Показать все Model через XDocument",
                Action = () => XmlReaderService.PrintModelsWithXDocument(Constants.phonesFile)
            },
            new MenuItem
            {
                Key = "8",
                Description = "Показать все Model через XmlDocument",
                Action = () => XmlReaderService.PrintModelsWithXmlDocument(Constants.phonesFile)
            },
            new MenuItem
            {
                Key = "9",
                Description = "Изменить значение элемента через XDocument",
                Action = () =>
                {
                    Console.Write("Введите имя элемента (например, Model): ");
                    string elemNameX = Console.ReadLine();
                    Console.Write("Введите индекс (начиная с 0): ");
                    if (!int.TryParse(Console.ReadLine(), out int indexX))
                    {
                        Console.WriteLine("Некорректный индекс.");
                        return;
                    }
                    Console.Write("Введите новое значение: ");
                    string newValX = Console.ReadLine();

                    XmlPatcher.UpdateElementValueXDocument(Constants.phonesFile, elemNameX, indexX, newValX);
                }
            },
            new MenuItem
            {
                Key = "10",
                Description = "Изменить значение элемента через XmlDocument",
                Action = () =>
                {
                    Console.Write("Введите имя элемента (например, Model): ");
                    string elemNameXml = Console.ReadLine();
                    Console.Write("Введите индекс (начиная с 0): ");
                    if (!int.TryParse(Console.ReadLine(), out int indexXml))
                    {
                        Console.WriteLine("Некорректный индекс.");
                        return;
                    }
                    Console.Write("Введите новое значение: ");
                    string newValXml = Console.ReadLine();

                    XmlPatcher.UpdateElementValueXmlDocument(Constants.phonesFile, elemNameXml, indexXml, newValXml);
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