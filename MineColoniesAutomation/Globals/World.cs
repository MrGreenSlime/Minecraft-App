using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Globals
{
    public class World
    {
        public string Name { get; set; }
        public List<Colonie> colonies {  get; set; }
        public List<ColonieStorage> items { get; set; }
    }
}
