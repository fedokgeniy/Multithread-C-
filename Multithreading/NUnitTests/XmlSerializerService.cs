using NUnit.Framework;
using MultithreadingServices;
using MultithreadingModels;
using System.Collections.Generic;
using System.IO;

namespace MultithreadingTests
{
    [TestFixture]
    public class XmlSerializerServiceTests
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
        public void SaveToXml_And_LoadFromXml_ShouldPersistData()
        {
            var phones = new List<Phone>
            {
                new Phone { Id = 1, Model = "Test", SerialNumber = "123", PhoneType = "Smart" }
            };

            XmlSerializerService.SaveToXml(phones, _tempFile);
            var loaded = XmlSerializerService.LoadFromXml<Phone>(_tempFile);

            Assert.That(loaded, Has.Count.EqualTo(1));
            Assert.That(loaded[0].Model, Is.EqualTo("Test"));
            Assert.That(loaded[0].SerialNumber, Is.EqualTo("123"));
            Assert.That(loaded[0].PhoneType, Is.EqualTo("Smart"));
        }

        [Test]
        public void LoadFromXml_NonExistentFile_ReturnsEmptyList()
        {
            var result = XmlSerializerService.LoadFromXml<Phone>("nonexistent.xml");

            Assert.That(result, Is.Empty);
        }
    }
}


