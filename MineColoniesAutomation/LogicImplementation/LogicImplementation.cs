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
        public Colonie Colonie { get; set; }

        public LogicImplementation(DataInterface.DataInterface data)
        {
            Data = data;
        }


        public void setColonie()
        {
            Data.setColonie();
        }
    }
}
