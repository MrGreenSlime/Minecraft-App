using Globals;
using LogicInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LogicImplementation
{
    public class LogicImplementation : LogicInterface.LogicInterface
    {
        public DataInterface.DataInterface Data;
        private CancellationTokenSource cancellationTokenSource;
        public List<World> World { get; set; }
        public List<WorldPath> paths { get; set; }
        public bool instanceSelected { get; set; }

        public LogicImplementation(DataInterface.DataInterface data)
        {
            cancellationTokenSource = new CancellationTokenSource();
            Data = data;
            World = Data.worlds;
            if (Data.InstancePath != null)
            {
                instanceSelected = true;
            }
        }


        public async Task setColonie()
        {
            await Data.loopColonies();
            World = Data.worlds;
        }

        public void setInstance(string v)
        {
            Data.setInstance(v);
        }

        public void Close()
        {
            Data.Close();
        }
        public void start()
        {
            var token = cancellationTokenSource.Token;
            Task task = Task.Run(async () =>
            {
                while (!token.IsCancellationRequested)
                {
                    await setColonie();
                    Thread.Sleep(10000);
                }

            });
        }
        public void stop()
        {
            cancellationTokenSource.Cancel();
            cancellationTokenSource = new CancellationTokenSource();
        }
        public void setPaths()
        {
            paths = Data.WorldPaths;
        }
        public bool IsLoggedIn()
        {
            return Data.LoggedIn;
        }
        public async Task Login(string email, string password)
        {
            await Data.Login(email, password);
        }

        public void InstallLuaFiles(string selectedPath)
        {
            Data.InstallLuaFile(selectedPath);
            Data.GetWorldPaths();
            setPaths();
        }
    }
}
