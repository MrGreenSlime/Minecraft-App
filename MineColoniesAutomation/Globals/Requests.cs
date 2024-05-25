using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Globals
{
    public class Requests
    {
        public int colonies_id { get; set; }
        public string fingerprint { get; set; }
        public int count { get; set; }
        public string name { get; set; }
        public string desc { get; set; }
        public string id { get; set; }
        public int minCount { get; set; }
        public string state { get; set; }
        public string target { get; set; }
        public List<RequestItem> items { get; set; }
        public override string ToString()
        {
            return name + " for " + target;
        }
    }
}
