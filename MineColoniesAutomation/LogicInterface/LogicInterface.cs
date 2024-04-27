using Globals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicInterface
{
    public interface LogicInterface
    {
        public List<World> World { get; set; }
        public bool instanceSelected { get; set; }
        void Close();
        public List<WorldPath> paths { get; set; }
        public void setColonie();
        void setInstance(string v);
        //public void setStorage();
        public void start();
        public void setPaths();
    }
}
