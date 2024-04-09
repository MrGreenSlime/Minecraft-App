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
        public List<string> WorldPaths { get; set; }
        public void setColonie();
        void setInstance(string v);
        public void setStorage();
        void setWorldPath(string path);
    }
}
