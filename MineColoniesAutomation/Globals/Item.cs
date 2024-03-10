using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Globals
{
    public class Item
    {
        public int Count { get; set; }
        public string DisplayName { get; set; }
        public string FingerPrint { get; set; }
        public int MaxStackSize { get; set; }
        public string Name { get; set; }
        public Dictionary<string, object> NBT { get; set; }
        public List<string> Tags { get; set; }
    }
}
