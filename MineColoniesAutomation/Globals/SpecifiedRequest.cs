using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Globals
{
    public class SpecifiedRequest
    {
        public int Available { get; set; }
        public int Delivering { get; set; }
        public string Displayname { get; set; }
        public Item Item {  get; set; } 
        public int Needed { get; set; }
        public string Status { get; set; }
    }
}
