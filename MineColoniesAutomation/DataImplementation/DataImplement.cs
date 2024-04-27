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
        public List<World> worlds { get; set; }
        public string InstancePath { get; set; }
        public List<WorldPath> WorldPaths { get; set; }
        public List<string> ModPaths { get; set; }
        private string tempPathString = Path.GetTempPath() + "\\MinecoloniesAutomation\\";
        public WorldPath SelectedWorldPath { get; set; }

        public DataImplement()
        {
            Directory.CreateDirectory(tempPathString);
            worlds = new List<World>();
            foreach (World item in worlds)
            {
                item.colonies = new List<Colonie>();
            }
            //world.colonies = new List<Colonie>();
            WorldPaths = new List<WorldPath>();
            ModPaths = new List<string>();
            //setColonie();
            //setStorage();
        }

        public void loopColonies()
        {
            worlds.Clear();
            foreach (WorldPath path in WorldPaths)
            {
                World newWorld = new World();
                newWorld.colonies = new List<Colonie>();
                foreach (string coloniePath in path.ColonyPaths)
                {
                    Colonie? colonie = SetColonie(coloniePath);
                    colonie = SetStorage(coloniePath, colonie);
                    WriteCommands(coloniePath, colonie);
                    newWorld.colonies.Add(colonie);
                }
                worlds.Add(newWorld);
            }
        }

        public Colonie? SetColonie(string path)
        {
            Colonie colonie = null;
            //string path = AppDomain.CurrentDomain.BaseDirectory + "../../../../DataImplementation/requests.json" ;
            string jsonString = File.ReadAllText(path + "\\requests.json");
            jsonString = jsonString.Replace("\"tags\":{}", "\"tags\":[]");
            jsonString = jsonString.Replace("\"tags\": {}", "\"tags\":[]");
            jsonString = jsonString.Replace("\"Requests\":{}", "\"Requests\":[]");
            jsonString = jsonString.Replace("\"Requests\": {}", "\"Requests\":[]");
            try
            {
                colonie = System.Text.Json.JsonSerializer.Deserialize<List<Colonie>>(jsonString)![0];
                //List<SpecifiedRequest> requestList = new List<SpecifiedRequest>();
                //foreach (BuilderRequests item1 in colonie.BuilderRequests)
                //{
                //    requestList.AddRange(item1.Requests);
                //}
                //colonies[0].items.items;
                //writeCommands(requestList, colonie.Requests, path + "\\commands.json", colonie);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return colonie;
        }

        public Colonie? SetStorage(string path, Colonie? colonie)
        {
            string jsonString = File.ReadAllText(path + "\\aeData.json");
            jsonString = jsonString.Replace("\"tags\":{}", "\"tags\":[]");
            jsonString = jsonString.Replace("\"tags\": {}", "\"tags\":[]");
            jsonString = jsonString.Replace("\"colonySide\":{}", "\"colonySide\":[]");
            jsonString = jsonString.Replace("\"playerSide\":{}", "\"playerSide\":[]");
            jsonString = jsonString.Replace("\"patterns\":{}", "\"patterns\":[]");
            try
            {
                if (colonie == null) throw new ArgumentException("colonie Does not exist");
                List<ItemsInStorage> Storage = System.Text.Json.JsonSerializer.Deserialize<List<ItemsInStorage>>(jsonString);
                for (int i = 0; i < Storage.Count; i++)
                {
                    colonie.items = Storage[i];
                    if (Storage[i].items.colonySide == null)
                    {
                        colonie.items.items.colonySide = new List<StorageItem>();
                    }
                    if (Storage[i].items.playerSide == null)
                    {
                        colonie.items.items.playerSide = new List<StorageItem>();
                    }
                    if (Storage[i].patterns == null)
                    {
                        colonie.items.patterns = new List<StorageItem>();
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return colonie;
        }

        public void WriteCommands(string path, Colonie? colonie)
        {
            List<SpecifiedRequest> requestList = new List<SpecifiedRequest>();
            colonie.BuilderRequests.ForEach(x => requestList.AddRange(x.Requests));

            List<Commands> commands = new List<Commands>();
            Dictionary<string, long> colonyReserve = new Dictionary<string, long>();
            Dictionary<string, long> playerReserve = new Dictionary<string, long>();
            foreach (SpecifiedRequest request in requestList)
            {
                if (!request.status.Equals("NOT_NEEDED,  HAVE_ENOUGH, IN_DELIVERY, NEED_MORE, DONT_HAVE "))
                {
                    long already_have = 0;
                    if (colonie.items == null)
                    {
                        break;
                    }
                    if (colonie.items.items.colonySide != null)
                    {
                        StorageItem colonyItem = colonie.items.items.colonySide.FirstOrDefault(x => x.name.Equals(request.item.name));
                        if (colonyItem != null)
                        {
                            if (!colonyReserve.ContainsKey(colonyItem.name)) colonyReserve.Add(colonyItem.name, 0);
                            if (colonyItem.amount - colonyReserve[request.item.name] > 0) continue;

                            if (request.needed > colonyItem.amount - colonyReserve[request.item.name])
                            {
                                colonyReserve[request.item.name] += colonyItem.amount;
                                already_have += colonyItem.amount;
                            }
                            else
                            {
                                colonyReserve[request.item.name] += request.needed;
                                continue;
                            }

                        }
                    }
                    if (colonie.items.items.playerSide != null)
                    {
                        StorageItem playerItem = colonie.items.items.playerSide.FirstOrDefault(x => x.name.Equals(request.item.name));
                        if (playerItem != null)
                        {
                            if (!playerReserve.ContainsKey(playerItem.name)) playerReserve.Add(playerItem.name, 0);
                            if (playerItem.amount - playerReserve[request.item.name] > 0) continue;

                            if (request.needed - already_have > playerItem.amount - playerReserve[request.item.name])
                            {
                                playerReserve[request.item.name] += playerItem.amount;
                                already_have += playerItem.amount;
                                commands.Add(new Commands { Amount = playerItem.amount, Item = request.item.name, NeedsCrafting = false });
                            }
                            else
                            {
                                playerReserve[request.item.name] += request.needed - already_have;
                                commands.Add(new Commands { Amount = request.needed - already_have, Item = request.item.name, NeedsCrafting = false });
                                continue;
                            }
                        }
                    }
                    if (colonie.items.patterns != null)
                    {
                        StorageItem patternItem = colonie.items.patterns.FirstOrDefault(x => x.name.Equals(request.item.name));
                        if (patternItem != null)
                        {
                            commands.Add(new Commands { Amount = request.needed - already_have, Item = request.item.name, NeedsCrafting = true });
                        }
                    }

                    //commands.Add(new Commands { Amount = request.needed, Item = request.item.name, NeedsCrafting = false });
                }

            }
            //foreach (Requests request in colonie.Requests)
            //{
            //    commands.Add(new Commands { Amount = 1, Item = request.items[0].name, NeedsCrafting = false });
            //}
            try
            {
                string data = JsonConvert.SerializeObject(commands);
                File.WriteAllText(path + "\\commands.json", data);
            }
            catch
            {

            }
        }


        public void setColonie()
        {
            worlds.Clear();
            foreach (WorldPath item in WorldPaths)
            {
                World newWorld = new World();
                newWorld.colonies = new List<Colonie>();
                //string path = AppDomain.CurrentDomain.BaseDirectory + "../../../../DataImplementation/requests.json" ;
                foreach (string colonyPath in item.ColonyPaths)
                {
                    string jsonString = File.ReadAllText(colonyPath + "\\requests.json");
                    jsonString = jsonString.Replace("\"tags\":{}", "\"tags\":[]");
                    jsonString = jsonString.Replace("\"tags\": {}", "\"tags\":[]");
                    jsonString = jsonString.Replace("\"Requests\":{}", "\"Requests\":[]");
                    jsonString = jsonString.Replace("\"Requests\": {}", "\"Requests\":[]");
                    try
                    {
                        List<Colonie> colonies = System.Text.Json.JsonSerializer.Deserialize<List<Colonie>>(jsonString);
                        newWorld.colonies.AddRange(colonies);
                        List<SpecifiedRequest> requestList = new List<SpecifiedRequest>();
                        foreach (BuilderRequests item1 in colonies[0].BuilderRequests)
                        {
                            requestList.AddRange(item1.Requests);
                        }
                        //colonies[0].items.items;
                        writeCommands(requestList, colonies[0].Requests, colonyPath + "\\commands.json", colonies[0]);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                newWorld = setStorage(item, newWorld);
                worlds.Add(newWorld);

                //foreach (Colonie item1 in newWorld.colonies)
                //{
                //    List<SpecifiedRequest> requestList = new List<SpecifiedRequest>();
                //    foreach (BuilderRequests item2 in item1.BuilderRequests)
                //    {
                //        requestList.AddRange(item2.Requests);
                //    }
                //    writeCommands(requestList, item1.Requests, colonyPath + "\\commands.json", item1);
                //}
            }

        }
        public World setStorage(WorldPath path, World newWorld)
        {
            foreach (string colonyPath in SelectedWorldPath.ColonyPaths)
            {
                //string path = AppDomain.CurrentDomain.BaseDirectory + "../../../../DataImplementation/aeData.json";
                string jsonString = File.ReadAllText(colonyPath + "\\aeData.json");
                jsonString = jsonString.Replace("\"tags\":{}", "\"tags\":[]");
                jsonString = jsonString.Replace("\"tags\": {}", "\"tags\":[]");
                jsonString = jsonString.Replace("\"colonySide\":{}", "\"colonySide\":[]");
                jsonString = jsonString.Replace("\"playerSide\":{}", "\"playerSide\":[]");
                jsonString = jsonString.Replace("\"patterns\":{}", "\"patterns\":[]");
                try
                {
                    List<ItemsInStorage> Storage = System.Text.Json.JsonSerializer.Deserialize<List<ItemsInStorage>>(jsonString);
                    for (int i = 0; i < Storage.Count; i++)
                    {
                        for (int j = 0; j < newWorld.colonies.Count; j++)
                        {
                            if (Storage[i].colony.Equals(newWorld.colonies[j].Name))
                            {
                                newWorld.colonies[j].items = Storage[i];
                                if (Storage[i].items.colonySide == null)
                                {
                                    newWorld.colonies[j].items.items.colonySide = new List<StorageItem>();
                                }
                                if (Storage[i].items.playerSide == null)
                                {
                                    newWorld.colonies[j].items.items.playerSide = new List<StorageItem>();
                                }
                                if (Storage[i].patterns == null)
                                {
                                    newWorld.colonies[j].items.patterns = new List<StorageItem>();
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
            return newWorld;
        }
        public void writeCommands(List<SpecifiedRequest> requests, List<Requests> regularRequests, string path, Colonie colonie)
        {
            List<Commands> commands = new List<Commands>();
            Dictionary<string, long> colonyReserve = new Dictionary<string, long>();
            Dictionary<string, long> playerReserve = new Dictionary<string, long>();
            foreach (SpecifiedRequest item in requests)
            {
                if (!item.status.Equals("NOT_NEEDED,  HAVE_ENOUGH, IN_DELIVERY, NEED_MORE, DONT_HAVE "))
                if (!item.status.Equals("NOT_NEEDED"))
                {
                    // ,  HAVE_ENOUGH, IN_DELIVERY, NEED_MORE, DONT_HAVE 
                    long already_have = 0;
                    if (colonie.items == null)
                    {
                        break;
                    }
                    if (colonie.items.items.colonySide != null)
                    {
                        StorageItem colonyItem = colonie.items.items.colonySide.FirstOrDefault(x => x.name.Equals(item.item.name));
                        if (colonyItem != null)
                        {
                            if (item.needed >= colonyItem.amount - colonyReserve[item.item.name])
                            {
                                colonyReserve[item.item.name] += colonyItem.amount;
                                already_have += colonyItem.amount;
                            }
                            else
                            {
                                colonyReserve[item.item.name] += item.needed;
                                continue;
                            }

                        }
                    }
                    if (colonie.items.items.playerSide != null)
                    {
                        StorageItem playerItem = colonie.items.items.playerSide.FirstOrDefault(x => x.name.Equals(item.item.name));
                        if (playerItem != null)
                        {
                            if (item.needed - already_have >= playerItem.amount - playerReserve[item.item.name])
                            {
                                playerReserve[item.item.name] += playerItem.amount;
                                already_have += playerItem.amount;
                                commands.Add(new Commands { Amount = playerItem.amount, Item = item.item.name, NeedsCrafting = false });
                            }
                            else
                            {
                                playerReserve[item.item.name] += item.needed - already_have;
                                continue;
                            }
                        }
                    }
                    if (colonie.items.patterns != null)
                    {
                        StorageItem patternItem = colonie.items.patterns.FirstOrDefault(x => x.name.Equals(item.item.name));
                        if (patternItem != null)
                        {

                            commands.Add(new Commands { Amount = item.needed - already_have, Item = item.item.name, NeedsCrafting = true });
                        }
                    }

                    commands.Add(new Commands { Amount = item.needed, Item = item.item.name, NeedsCrafting = false });
                }

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
            List<string> tempPaths = new List<string>();
            foreach (string modPath in ModPaths)
            {
                tempPaths.Add(tempPathString + modPath.Split('\\').Last());
                if (!Directory.Exists(tempPathString + modPath.Split('\\').Last()))
                    await Task.Run(() => { ZipFile.ExtractToDirectory(modPath, tempPathString + modPath.Split('\\').Last()); });
            }

            //TODO find and extract recipes
            foreach (string tempPath in tempPaths)
            {
                if (Directory.Exists(tempPath + "\\data"))
                {
                    foreach (string dir in Directory.EnumerateDirectories(tempPath + "\\data"))
                    {
                        List<string> dataDirs = Directory.EnumerateDirectories(dir).ToList();
                        List<string> recipeDirs = dataDirs.Where(x => x.Contains("recipe")).ToList();
                        List<string> recipeFiles = new List<string>();
                        foreach (string recipeDir in recipeDirs)
                        {
                            recipeFiles.AddRange(Directory.EnumerateFiles(recipeDir, "*", SearchOption.AllDirectories));
                        }
                    }
                }
            }
        }

        public void Close()
        {
            Directory.Delete(tempPathString, true);
        }
        public void setWorldPath(WorldPath path)
        {
            if (WorldPaths.Contains(path))
            {
                SelectedWorldPath = path;
            }
            loopColonies();
        }
    }
}
