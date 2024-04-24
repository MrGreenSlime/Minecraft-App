using Globals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataInterface
{
    public interface DataInterface
    {
        public List<World> world { get; set; }

        public void Close();
        public List<WorldPath> WorldPaths { get; set; }
        public void setColonie();
        public void setInstance(string v);
        public World setStorage(WorldPath path, World newWorld);
        public void setWorldPath(WorldPath path);
    }
}
