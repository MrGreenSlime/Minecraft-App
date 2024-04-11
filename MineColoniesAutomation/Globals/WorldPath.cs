using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Globals
{
    public class WorldPath : IEquatable<WorldPath>
    {
        public string WorldPathString { get; set; }
        public List<string> ComputerPaths { get; set;}
        public List<string> ColonyPaths { get; set; }

        public bool Equals(WorldPath? other)
        {
            return WorldPathString == other.WorldPathString;
        }

        public override string ToString()
        {
            
            return WorldPathString.Split('\\').Last();
        }
    }
}
