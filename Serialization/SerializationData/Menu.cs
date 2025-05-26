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
                Description = "Create 10 phones and display them",
                Action = () =>
                {
                    DataStore.phones = DataGenerator.GeneratePhones(10);
                    DataStore.phones.ForEach(p => p.Print());
                }
            },
            new MenuItem
            {
                Key = "2",
                Description = "Create 10 manufacturers and display them",
                Action = () =>
                {
                    DataStore.manufacturers = DataGenerator.GenerateManufacturers(10);
                    DataStore.manufacturers.ForEach(m => m.Print());
                }
            },
            new MenuItem
            {
                Key = "3",
                Description = "Serialize phones",
                Action = () =>
                {
                    XmlSerializerService.SaveToXml(DataStore.phones, Constants.phonesFile);
                    Console.WriteLine("Phones have been serialized.");
                }
            },
            new MenuItem
            {
                Key = "4",
                Description = "Serialize manufacturers",
                Action = () =>
                {
                    XmlSerializerService.SaveToXml(DataStore.manufacturers, Constants.manufacturersFile);
                    Console.WriteLine("Manufacturers have been serialized.");
                }
            },
            new MenuItem
            {
                Key = "5",
                Description = "Show phones from XML",
                Action = () =>
                {
                    var loadedPhones = XmlSerializerService.LoadFromXml<Phone>(Constants.phonesFile);
                    if (loadedPhones.Count == 0) Console.WriteLine("File is empty.");
                    else loadedPhones.ForEach(p => p.Print());
                }
            },
            new MenuItem
            {
                Key = "6",
                Description = "Show manufacturers from XML",
                Action = () =>
                {
                    var loadedManufacturers = XmlSerializerService.LoadFromXml<Manufacturer>(Constants.manufacturersFile);
                    if (loadedManufacturers.Count == 0) Console.WriteLine("File is empty.");
                    else loadedManufacturers.ForEach(m => m.Print());
                }
            },
            new MenuItem
            {
                Key = "7",
                Description = "Show all Model using XDocument",
                Action = () => XmlReaderService.PrintModelsWithXDocument(Constants.phonesFile)
            },
            new MenuItem
            {
                Key = "8",
                Description = "Show all Model using XmlDocument",
                Action = () => XmlReaderService.PrintModelsWithXmlDocument(Constants.phonesFile)
            },
            new MenuItem
            {
                Key = "9",
                Description = "Change element value using XDocument",
                Action = () =>
                {
                    Console.Write("Enter element name (for example, Model): ");
                    string elemNameX = Console.ReadLine();
                    Console.Write("Enter index (starting from 0): ");
                    if (!int.TryParse(Console.ReadLine(), out int indexX))
                    {
                        Console.WriteLine("Invalid index.");
                        return;
                    }
                    Console.Write("Enter new value: ");
                    string newValX = Console.ReadLine();

                    XmlPatcher.UpdateElementValueXDocument(Constants.phonesFile, elemNameX, indexX, newValX);
                }
            },
            new MenuItem
            {
                Key = "10",
                Description = "Change element value using XmlDocument",
                Action = () =>
                {
                    Console.Write("Enter element name (for example, Model): ");
                    string elemNameXml = Console.ReadLine();
                    Console.Write("Enter index (starting from 0): ");
                    if (!int.TryParse(Console.ReadLine(), out int indexXml))
                    {
                        Console.WriteLine("Invalid index.");
                        return;
                    }
                    Console.Write("Enter new value: ");
                    string newValXml = Console.ReadLine();

                    XmlPatcher.UpdateElementValueXmlDocument(Constants.phonesFile, elemNameXml, indexXml, newValXml);
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
