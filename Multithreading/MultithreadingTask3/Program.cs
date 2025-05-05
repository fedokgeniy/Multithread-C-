using System.Numerics;
using MultithreadingModels;
using MultithreadingServices;

List<Phone> phones = DataGenerator.GeneratePhones(20);
List<Manufacturer> manufacturers = DataGenerator.GenerateManufacturers(20);

Thread t1 = new(() =>
{
    XmlSerializerService.SaveToXml(phones.GetRange(0, 10), "phones_part1.xml");
    XmlSerializerService.SaveToXml(manufacturers.GetRange(0, 10), "manufacturers_part1.xml");
});
Thread t2 = new(() =>
{
    XmlSerializerService.SaveToXml(phones.GetRange(10, 10), "phones_part2.xml");
    XmlSerializerService.SaveToXml(manufacturers.GetRange(10, 10), "manufacturers_part2.xml");
});

t1.Start();
t2.Start();
t1.Join();
t2.Join();

Console.WriteLine("Task 1 complete. Files created.");

ParallelMergerService.MergeAlternating<Phone>("phones_part1.xml", "phones_part2.xml", "phones_merged.xml");

ParallelMergerService.MergeAlternating<Manufacturer>("manufacturers_part1.xml", "manufacturers_part2.xml", "manufacturers_merged.xml");

Console.WriteLine("Task 2 complete: merged with interleaving.");

// Файл 3 из задания 2 
var mergedPhoneFile = "phones_merged.xml";
var mergedManufacturerFile = "manufacturers_merged.xml";

// Задание 3.1
FileReaderService.ReadSingleThreaded<Phone>(mergedPhoneFile);
// Задание 3.2
FileReaderService.ReadTwoThreaded<Phone>(mergedPhoneFile);
// Задание 3.3
FileReaderService.ReadWithSemaphore<Phone>(mergedPhoneFile);