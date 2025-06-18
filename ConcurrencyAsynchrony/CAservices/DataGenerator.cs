using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CAmodels;

namespace ConcurrencyAsynchrony
{
    /// <summary>
    /// Responsible for creating objects and saving them to files.
    /// </summary>
    public class ObjectGenerator
    {
        /// <summary>
        /// Asynchronously creates objects and saves them to files.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="InvalidOperationException">Thrown when an error occurs during object creation or saving.</exception>
        public async Task GenerateAndSaveObjectsAsync()
        {
            try
            {
                Console.WriteLine("Creating objects...");
                var objects = CreateObjects();
                await SaveObjectsToFilesAsync(objects);
                Console.WriteLine("Objects successfully written to files.\n");
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error occurred while creating and saving objects", ex);
            }
        }

        /// <summary>
        /// Creates a collection of objects (phones and manufacturers).
        /// </summary>
        /// <returns>List of created objects.</returns>
        private List<object> CreateObjects()
        {
            var objects = new List<object>();

            for (int i = 0; i < AppConstants.PhonesCount; i++)
            {
                objects.Add(new Phone
                {
                    Id = i + 1,
                    Model = $"Model_{i + 1}",
                    SerialNumber = $"SN_{1000 + i}",
                    PhoneType = (i % 2 == 0) ? "Smartphone" : "Landline"
                });
            }

            for (int i = 0; i < AppConstants.ManufacturersCount; i++)
            {
                objects.Add(new Manufacturer
                {
                    Name = $"Manufacturer_{i + 1}",
                    Address = $"Address_{i + 1}",
                    IsAChildCompany = (i % 2 == 0)
                });
            }

            var random = new Random();
            return objects.OrderBy(_ => random.Next()).ToList();
        }

        /// <summary>
        /// Asynchronously saves objects to files in parallel.
        /// </summary>
        /// <param name="objects">Collection of objects to save.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        private async Task SaveObjectsToFilesAsync(List<object> objects)
        {
            var tasks = new List<Task>();

            for (int fileIndex = 0; fileIndex < AppConstants.FileNames.Length; fileIndex++)
            {
                int capturedIndex = fileIndex;
                tasks.Add(WriteToFileAsync(
                    AppConstants.FileNames[capturedIndex],
                    objects.Skip(capturedIndex * AppConstants.ObjectsPerFile).Take(AppConstants.ObjectsPerFile)
                ));
            }

            await Task.WhenAll(tasks);
        }

        /// <summary>
        /// Asynchronously writes objects to the specified file.
        /// </summary>
        /// <param name="fileName">File name to write to.</param>
        /// <param name="objects">Collection of objects to write.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        private async Task WriteToFileAsync(string fileName, IEnumerable<object> objects)
        {
            using var writer = new StreamWriter(fileName);
            foreach (var obj in objects)
            {
                await writer.WriteLineAsync(obj.ToString());
            }
        }
    }
}