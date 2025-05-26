using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml;

namespace SerializationServices
{
    public static class XmlReaderService
    {
        private const string modelName = "Phone";
        private const string model = "Model";
        public static void PrintModelsWithXDocument(string path)
        {
            if (!File.Exists(path))
            {
                Console.WriteLine("Файл не найден.");
                return;
            }

            var doc = XDocument.Load(path);
            var models = doc.Descendants(modelName)
                            .Select(e => e.Element(model)?.Value)
                            .Where(v => !string.IsNullOrWhiteSpace(v));

            Console.WriteLine("Значения моделей (через XDocument):");
            foreach (var model in models)
            {
                Console.WriteLine(model);
            }
        }

        public static void PrintModelsWithXmlDocument(string path)
        {
            if (!File.Exists(path))
            {
                Console.WriteLine("Файл не найден.");
                return;
            }

            var doc = new XmlDocument();
            doc.Load(path);

            var modelNodes = doc.GetElementsByTagName("model");
            Console.WriteLine("Значения моделей (через XmlDocument):");
            foreach (XmlNode node in modelNodes)
            {
                if (node != null && !string.IsNullOrWhiteSpace(node.InnerText))
                {
                    Console.WriteLine(node.InnerText);
                }
            }
        }
    }
}
