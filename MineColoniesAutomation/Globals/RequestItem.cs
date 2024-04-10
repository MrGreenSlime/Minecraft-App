using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Globals
{
    public class RequestItem
    {
        public int count { get; set; }
        public string displayName { get; set; }
        public string fingerPrint { get; set; }
        public int maxStackSize { get; set; }
        public string name { get; set; }
        public Dictionary<string, object> nbt { get; set; }
        public string[] tags { get; set; }
        public override string ToString()
        {
            return " " + displayName;
        }
    }
}
