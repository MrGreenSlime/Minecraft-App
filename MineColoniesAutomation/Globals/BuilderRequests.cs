using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Globals
{
    public class BuilderRequests
    {
        public Location Location { get; set; }
        public string Name { get; set; }
        public List<SpecifiedRequest> Requests { get; set; }
    }
}
