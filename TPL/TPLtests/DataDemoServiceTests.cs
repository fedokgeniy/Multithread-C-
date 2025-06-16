using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using TPLservices;
using TPLmodels;

namespace TPLtests
{
    [TestFixture]
    public class PhoneDemoServiceTests
    {
        private readonly string file1 = "phones1.xml";
        private readonly string file2 = "phones2.xml";
        private readonly string resultFile = "phones_result.xml";

        [TearDown]
        public void TearDown()
        {
            if (File.Exists(file1)) File.Delete(file1);
            if (File.Exists(file2)) File.Delete(file2);
            if (File.Exists(resultFile)) File.Delete(resultFile);
        }

        [Test]
        public void GenerateAndSerialize_ShouldCreateTwoFiles()
        {
            PhoneDemoService.GenerateAndSerialize();

            Assert.That(File.Exists(file1), Is.True);
            Assert.That(File.Exists(file2), Is.True);
        }

        [Test]
        public async Task ReadAndPrintAsync_ShouldReadAndPrintData()
        {
            var testData = new List<Phone>
            {
                new Phone { Id = 1, Model = "TestModel", SerialNumber = "SN1", PhoneType = "Smartphone" }
            };
            XmlSerializerService.SaveToXml(testData, resultFile);

            await PhoneDemoService.ReadAndPrintAsync();
        }
    }
}

