using DataInterface;
using Globals;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DataImplementation
{
    public class DataImplement : DataInterface.DataInterface
    {
        public World world { get; set; }
        public string InstancePath { get; set; }
        public List<WorldPath> WorldPaths { get; set; }
        public List<string> ModPaths { get; set; }
        private string tempPathString = Path.GetTempPath() + "\\MinecoloniesAutomation\\";
        public WorldPath SelectedWorldPath { get; set; }

        public DataImplement()
        {
            Directory.CreateDirectory(tempPathString);
            world = new World();
            world.colonies = new List<Colonie>();
            WorldPaths = new List<WorldPath>();
            ModPaths = new List<string>();
            //setColonie();
            //setStorage();
        }
        
        public void setColonie()
        {
            world.colonies.Clear();
            //string path = AppDomain.CurrentDomain.BaseDirectory + "../../../../DataImplementation/requests.json" ;
            foreach (string colonyPath in SelectedWorldPath.ColonyPaths)
            {
                string jsonString = File.ReadAllText(colonyPath + "\\requests.json");
                jsonString = jsonString.Replace("\"tags\":{}", "\"tags\":[]");
                jsonString = jsonString.Replace("\"tags\": {}", "\"tags\":[]");
                jsonString = jsonString.Replace("\"Requests\":{}", "\"Requests\":[]");
                jsonString = jsonString.Replace("\"Requests\": {}", "\"Requests\":[]");
                try
                {
                    List<Colonie> colonies = System.Text.Json.JsonSerializer.Deserialize<List<Colonie>>(jsonString);
                    world.colonies.AddRange(colonies);
                    List<SpecifiedRequest> requestList = new List<SpecifiedRequest>();
                    foreach (BuilderRequests item1 in colonies[0].BuilderRequests)
                    {
                        requestList.AddRange(item1.Requests);
                    }
                    writeCommands(requestList, colonies[0].Requests, colonyPath + "\\commands.json");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            setStorage();
            
        }
        public void setStorage()
        {
            foreach (string colonyPath in SelectedWorldPath.ColonyPaths)
            {
                //string path = AppDomain.CurrentDomain.BaseDirectory + "../../../../DataImplementation/aeData.json";
                string jsonString = File.ReadAllText(colonyPath + "\\aeData.json");
                jsonString = jsonString.Replace("\"tags\":{}", "\"tags\":[]");
                jsonString = jsonString.Replace("\"tags\": {}", "\"tags\":[]");
                try
                {
                    List<ItemsInStorage> Storage = System.Text.Json.JsonSerializer.Deserialize<List<ItemsInStorage>>(jsonString);
                    for (int i = 0; i < Storage.Count; i++)
                    {
                        for (int j = 0; j < world.colonies.Count; j++)
                        {
                            if (Storage[i].colony.Equals(world.colonies[j].Name))
                            {
                                world.colonies[j].items = Storage[i];
                                if (Storage[i].items.colonySide == null)
                                {
                                    world.colonies[j].items.items.colonySide = new List<StorageItem>();
                                }
                                if (Storage[i].items.playerSide == null)
                                {
                                    world.colonies[j].items.items.playerSide = new List<StorageItem>();
                                }
                                if (Storage[i].patterns == null)
                                {
                                    world.colonies[j].items.patterns = new List<StorageItem>();
                                }
                                
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
        public void writeCommands(List<SpecifiedRequest> requests, List<Requests> regularRequests, string path)
        {
            List<Commands> commands = new List<Commands>();
            foreach (SpecifiedRequest item in requests)
            {
                commands.Add(new Commands { Amount = item.needed, Item = item.item.name, NeedsCrafting = false });
            }
            foreach (Requests request in regularRequests)
            {
                commands.Add(new Commands { Amount = 1, Item = request.items[0].name, NeedsCrafting = false });
            }
            try
            {
                string data = JsonConvert.SerializeObject(commands);
                File.WriteAllText(path, data);
            }
            catch
            {

            }
        }
        public void setInstance(string v)
        {
            InstancePath = v;
            //Check if correct mods are installed

            GetModPaths();

            //Extract different world paths

            GetWorldPaths();

            GetRecipes();
        }

        private void GetModPaths()
        {
            if (Path.Exists(InstancePath + "\\mods"))
            {
                ModPaths = Directory.EnumerateFiles(InstancePath + "\\mods").ToList();
                var mod = Directory.EnumerateFiles(InstancePath + "\\mods", "*AdvancedPeripherals*").ToList();
                if (mod.Count() == 0) throw new Exception("Instance does not have AdvancedPeripherals installed");
                mod = Directory.EnumerateFiles(InstancePath + "\\mods", "*appliedenergistics2*").ToList();
                if (mod.Count() == 0) throw new Exception("Instance does not have Applied Energistics 2 installed");
                mod = Directory.EnumerateFiles(InstancePath + "\\mods", "*cc-tweaked*").ToList();
                if (mod.Count() == 0) throw new Exception("Instance does not have cc-tweaked installed");
                mod = Directory.EnumerateFiles(InstancePath + "\\mods", "*minecolonies*").ToList();
                if (mod.Count() == 0) throw new Exception("Instance does not have minecolonies installed");
            }
            else throw new ArgumentException("Instance does not have a mod folder");
        }

        private void GetWorldPaths()
        {
            if (Path.Exists(InstancePath + "\\saves"))
            {
                List<string> tempWorldPaths = Directory.EnumerateDirectories(InstancePath + "\\saves").ToList();
                foreach (var dir in tempWorldPaths)
                {
                    WorldPath worldPath = new WorldPath { WorldPathString = dir, ColonyPaths = new List<string>(), ComputerPaths = new List<string>() };
                    if (Path.Exists(dir + "\\computercraft\\computer"))
                    {
                        worldPath.ComputerPaths = Directory.EnumerateDirectories(dir + "\\computercraft\\computer").ToList();
                        List<string> toKeep = new List<string>();
                        toKeep = worldPath.ComputerPaths.ToList();
                        foreach (var dir2 in worldPath.ComputerPaths)
                        {
                            if (File.Exists(dir2 + "\\aeInterface.lua") && File.Exists(dir2 + "\\extractTasks.lua") && File.Exists(dir2 + "\\JsonFileHelper.lua") && File.Exists(dir2 + "\\main.lua") && File.Exists(dir2 + "\\monitorWriter.lua") && File.Exists(dir2 + "\\wrapPeripherals.lua"))
                            {
                                worldPath.ColonyPaths.Add(dir2);
                                toKeep.Remove(dir2);
                            }
                        }
                        worldPath.ComputerPaths = toKeep;
                    }
                    WorldPaths.Add(worldPath);
                }
            }
        }

        async private void GetRecipes()
        {
            foreach (string modPath in ModPaths)
            {
                if (!Directory.Exists(tempPathString + modPath.Split('\\').Last()))
                    await Task.Run(() => { ZipFile.ExtractToDirectory(modPath, tempPathString + modPath.Split('\\').Last()); });


            }
            //TODO find and extract recipes
        }

        public void Close()
        {
            Directory.Delete(tempPathString, true);
        }
        public void setWorldPath (WorldPath path)
        {
            if (WorldPaths.Contains(path))
            {
                SelectedWorldPath = path;
            }
            setColonie();
        }
    }
}
