﻿using Globals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicInterface
{
    public interface LogicInterface
    {
        public World World { get; set; }

        void Close();
        public void setColonie();
        void setInstance(string v);
        public void setStorage();
    }
}
