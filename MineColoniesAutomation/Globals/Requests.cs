using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Globals
{
    public class Requests
    {
        public int Count { get; set; }
        public string Name { get; set; }
        public string Desc { get; set; }
        public string Id { get; set; }
        public int MinCount { get; set; }
        public string State { get; set; }
        public string Target { get; set; }
        public List<Item> Items { get; set; }
    }
}
