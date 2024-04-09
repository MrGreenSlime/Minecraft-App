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
        public World world { get; set; }

        public void Close();
        public List<WorldPath> WorldPaths { get; set; }
        public void setColonie();
        public void setInstance(string v);
        public void setStorage();
        public void setWorldPath(WorldPath path);
    }
}
