using NUnit.Framework;
using MultithreadingServices;
using MultithreadingModels;
using System.Collections.Generic;
using System.IO;

namespace MultithreadingTests
{
    [TestFixture]
    public class ParallelMergerServiceTests
    {
        private string _file1, _file2, _output;

        [SetUp]
        public void SetUp()
        {
            _file1 = Path.GetTempFileName();
            _file2 = Path.GetTempFileName();
            _output = Path.GetTempFileName();
        }

        [TearDown]
        public void TearDown()
        {
            foreach (var file in new[] { _file1, _file2, _output })
                if (File.Exists(file)) File.Delete(file);
        }

        [Test]
        public void MergeAlternating_MergesTwoFiles()
        {
            var phones1 = new List<Phone> { new Phone { Id = 1, Model = "A", SerialNumber = "X", PhoneType = "T1" } };
            var phones2 = new List<Phone> { new Phone { Id = 2, Model = "B", SerialNumber = "Y", PhoneType = "T2" } };
            XmlSerializerService.SaveToXml(phones1, _file1);
            XmlSerializerService.SaveToXml(phones2, _file2);

            ParallelMergerService.MergeAlternating<Phone>(_file1, _file2, _output);

            var merged = XmlSerializerService.LoadFromXml<Phone>(_output);
            Assert.That(merged, Has.Count.EqualTo(2));
            Assert.That(merged.Exists(p => p.Model == "A"));
            Assert.That(merged.Exists(p => p.Model == "B"));
        }
    }
}
