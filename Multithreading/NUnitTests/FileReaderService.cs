using NUnit.Framework;
using MultithreadingServices;
using MultithreadingModels;
using System;
using System.IO;
using System.Collections.Generic;

namespace MultithreadingTests
{
    [TestFixture]
    public class FileReaderServiceTests
    {
        private string _tempFile;
         
        [SetUp]
        public void SetUp()
        {
            _tempFile = Path.GetTempFileName();
        }

        [TearDown]
        public void TearDown()
        {
            if (File.Exists(_tempFile))
                File.Delete(_tempFile);
        }

        [Test]
        public void ReadSingleThreaded_PrintsData()
        {
            var phones = new List<Phone>
            {
                new Phone { Id = 1, Model = "A", SerialNumber = "X", PhoneType = "Type1" }
            };
            XmlSerializerService.SaveToXml(phones, _tempFile);

            using var sw = new StringWriter();
            Console.SetOut(sw);

            FileReaderService.ReadSingleThreaded<Phone>(_tempFile);

            var output = sw.ToString();
            Assert.That(output, Does.Contain("Single-threaded reading"));
            Assert.That(output, Does.Contain("Model=A"));
        }
    }
}
