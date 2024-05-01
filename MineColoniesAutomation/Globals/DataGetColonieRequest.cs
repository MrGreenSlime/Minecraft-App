using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Globals
{
    public class DataGetColonieRequest
    {
        public int id {  get; set; }
        public string name { get; set; }
        public bool autocomplete { get; set; }
        public bool autoArmor { get; set; }
        public bool autoTools { get; set; }
        public List<BuilderRequests> builderRequests { get; set; }
    }
}
