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
        public static void PrintModelsWithXDocument(string path)
        {
            if (!File.Exists(path))
            {
                Console.WriteLine("Файл не найден.");
                return;
            }

            var doc = XDocument.Load(path);
            var models = doc.Descendants("Phone")
                            .Select(e => e.Element("Model")?.Value)
                            .Where(v => !string.IsNullOrWhiteSpace(v));

            Console.WriteLine("Model values (via XDocument):");
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

            var modelNodes = doc.GetElementsByTagName("Model");
            Console.WriteLine("Model values (via XmlDocument):");
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
