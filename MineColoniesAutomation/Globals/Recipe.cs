using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Globals
{
    public class Recipe
    {
        public Dictionary<string, int> Results { get; set; }
        public Dictionary<string, int> Inputs { get; set; }
        public string Type { get; set; }
    }
}
