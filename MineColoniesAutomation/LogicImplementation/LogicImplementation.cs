using Globals;
using LogicInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicImplementation
{
    public class LogicImplementation : LogicInterface.LogicInterface
    {
        public DataInterface.DataInterface Data;
        public World World { get; set; }

        public LogicImplementation(DataInterface.DataInterface data)
        {
            Data = data;
            World = Data.world;
        }


        public void setColonie()
        {
            Data.setColonie();
            World = Data.world;
        }
        public void setStorage()
        {
            Data.setStorage();
            World = Data.world;
        }
    }
}
