using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml;

namespace SerializationServices
{
    public static class XmlPatcher
    {

        public static void UpdateElementValueXDocument(string path, string elementName, int index, string newValue)
        {
            if (!File.Exists(path))
            {
                Console.WriteLine("Файл не найден.");
                return;
            }

            var doc = XDocument.Load(path);
            var elements = doc.Descendants(elementName).ToList();

            if (index < 0 || index >= elements.Count)
            {
                Console.WriteLine("Неверный индекс.");
                return;
            }

            elements[index].Value = newValue;
            doc.Save(path);

            Console.WriteLine($"{elementName} #{index} успешно обновлён на \"{newValue}\" (XDocument)");
        }

        public static void UpdateElementValueXmlDocument(string path, string elementName, int index, string newValue)
        {
            if (!File.Exists(path))
            {
                Console.WriteLine("Файл не найден.");
                return;
            }

            var doc = new XmlDocument();
            doc.Load(path);
            var elements = doc.GetElementsByTagName(elementName);

            if (index < 0 || index >= elements.Count)
            {
                Console.WriteLine("Неверный индекс.");
                return;
            }

            elements[index].InnerText = newValue;
            doc.Save(path);

            Console.WriteLine($"{elementName} #{index} успешно обновлён на \"{newValue}\" (XmlDocument)");
        }
    }
}
