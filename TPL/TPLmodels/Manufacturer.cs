using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultithreadingModels
{
    public class Manufacturer
    {
        public string Name { get; set; }

        public string Address { get; set; }

        public bool IsAChildCompany { get; set; }

        public void Print()
        {
            Console.WriteLine($"Manufacturer: Name={Name}, Address={Address}, IsAChildCompany=HIDDEN");
        }

        public override string ToString()
        {
            return $"Manufacturer: Name={Name}, Address={Address}";
        }
    }
}
