using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Globals
{
    public class BuilderRequests
    {
        public Location location { get; set; }
        public string name { get; set; }
        public List<SpecifiedRequest> Requests { get; set; }
        public override string ToString()
        {
            if (name == null)
            {
                return "" + location.x + ',' + location.y + ',' + location.z;
            }
            return name + '|' + location.x + ',' + location.y + ',' + location.z;
        }
    }
}
