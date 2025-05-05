using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerializationModels
{
    public class MenuItem
    {
        public string Key { get; set; }
        public string Description { get; set; }
        public Action Action { get; set; }
    }
}
