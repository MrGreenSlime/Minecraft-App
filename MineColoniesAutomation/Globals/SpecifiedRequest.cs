using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Globals
{
    public class SpecifiedRequest
    {
        public int available { get; set; }
        public int delivering { get; set; }
        public string displayName { get; set; }
        public RequestItem item {  get; set; } 
        public int needed { get; set; }
        public string status { get; set; }
        public override string ToString()
        {
            return needed + " " + item.displayName.Replace("[", "").Replace("]", "");
        }
    }
}
