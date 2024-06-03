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
        public List<World> worlds { get; set; }

        public void Close();
        public string InstancePath { get; set; }
        public List<WorldPath> WorldPaths { get; set; }
        public void setInstance(string v);
        public void start();
        public Task loopColonies();
        public Task refresh();
        public Task Login(string username, string password);
        void InstallLuaFile(string selectedPath);
        public void GetWorldPaths();

        public bool LoggedIn { get; set; }
    }
}
