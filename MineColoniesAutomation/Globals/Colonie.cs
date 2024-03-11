using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Globals
{
    public class Colonie
    {
        public string Name { get; set; }
        public List<BuilderRequests> BuilderRequests { get; set; }
        public List<Requests> Requests { get; set; }
        public override string ToString()
        {
            return Name;
        }
    }
}
