using System.Numerics;
using MultithreadingModels;
using MultithreadingServices;
using MultithreadingData;

List<Phone> phones = DataGenerator.GeneratePhones(20);
List<Manufacturer> manufacturers = DataGenerator.GenerateManufacturers(20);

Thread t1 = new(() =>
{
    XmlSerializerService.SaveToXml(phones.GetRange(0, 10), Constants.phonesFile1);
    XmlSerializerService.SaveToXml(manufacturers.GetRange(0, 10), Constants.manufacturersFile1);
});
Thread t2 = new(() =>
{
    XmlSerializerService.SaveToXml(phones.GetRange(10, 10), Constants.phonesFile1);
    XmlSerializerService.SaveToXml(manufacturers.GetRange(10, 10), Constants.manufacturersFile2);
});

t1.Start();
t2.Start();
t1.Join();
t2.Join();

Console.WriteLine("Task 1 complete. Files created.");

ParallelMergerService.MergeAlternating<Phone>(Constants.phonesFile1, Constants.phonesFile1, Constants.phonesMergedFile);

ParallelMergerService.MergeAlternating<Manufacturer>(Constants.manufacturersFile1, Constants.manufacturersFile2, Constants.manufacturersMergedFile);

Console.WriteLine("Task 2 complete: merged with interleaving.");


// Задание 3.1
FileReaderService.ReadSingleThreaded<Phone>(Constants.phonesMergedFile);
// Задание 3.2
FileReaderService.ReadTwoThreaded<Phone>(Constants.phonesMergedFile);
// Задание 3.3
FileReaderService.ReadWithSemaphore<Phone>(Constants.phonesMergedFile);