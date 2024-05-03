using DataInterface;
using Globals;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DataImplementation
{
    public class DataImplement : DataInterface.DataInterface
    {
        public readonly string ApiUrl = "http://localhost:8080/api";
        public List<World> worlds { get; set; }
        public string InstancePath { get; set; }
        public List<WorldPath> WorldPaths { get; set; }
        public List<string> ModPaths { get; set; }
        private string tempPathString = Path.GetTempPath() + "\\MinecoloniesAutomation\\";
        public List<Recipe> Recipes { get; set; }

        public DataImplement()
        {
            Directory.CreateDirectory(tempPathString);
            worlds = new List<World>();
            foreach (World item in worlds)
            {
                item.colonies = new List<Colonie>();
            }
            WorldPaths = new List<WorldPath>();
            ModPaths = new List<string>();
            CheckStorage();
        }

        public async Task loopColonies()
        {
            worlds.Clear();
            foreach (WorldPath path in WorldPaths)
            {
                string returnData = await GetRequest("/worlds/" + path.WorldPathString.Replace("\\", "-"));
                if (returnData == null)
                {
                    await PostRequest("{\"name\":\"" + path.WorldPathString.Replace("\\", "-") + "\"}", "/worlds");
                    returnData = await GetRequest("/worlds/" + path.WorldPathString.Replace("\\", "-"));
                }
                if (returnData == null)
                    continue;
                int worldId = System.Text.Json.JsonSerializer.Deserialize<WorldGetRequest>(returnData).Data.id;
                World newWorld = new World();
                newWorld.colonies = new List<Colonie>();
                foreach (string coloniePath in path.ColonyPaths)
                {
                    
                    Colonie? colonie = SetColonie(coloniePath);
                    string returnColonieData = await GetRequest("/worlds/" + path.WorldPathString.Replace("\\", "-") + "/colonies/" + colonie.Name);
                    if (returnColonieData == null)
                    {
                        await PostRequest("{\"name\":\"" + colonie.Name + "\",\"world_id\":\""+ worldId +"\"}", "/colonies");
                        returnColonieData = await GetRequest("/worlds/" + path.WorldPathString.Replace("\\", "-") + "/colonies/" + colonie.Name);
                    }
                    if (returnColonieData == null)
                        continue;
                    DataGetColonieRequest request = System.Text.Json.JsonSerializer.Deserialize<ColonieGetRequest>(returnColonieData).Data;
                    //DataGetColonieRequest request = await GetRequest("/worlds/" + path.WorldPathString + "/colonies/" + colonie.Name);
                    colonie = SetStorage(coloniePath, colonie);
                    WriteCommands(coloniePath, colonie, request);
                    newWorld.colonies.Add(colonie);
                }
                worlds.Add(newWorld);
            }
        }

        public Colonie? SetColonie(string path)
        {
            Colonie colonie = null;
            //string path = AppDomain.CurrentDomain.BaseDirectory + "../../../../DataImplementation/requests.json" ;
            string jsonString = System.IO.File.ReadAllText(path + "\\requests.json");
            jsonString = jsonString.Replace("\"tags\":{}", "\"tags\":[]");
            jsonString = jsonString.Replace("\"tags\": {}", "\"tags\":[]");
            jsonString = jsonString.Replace("\"Requests\":{}", "\"Requests\":[]");
            jsonString = jsonString.Replace("\"Requests\": {}", "\"Requests\":[]");
            jsonString = jsonString.Replace("\"BuilderRequests\":{}", "\"BuilderRequests\":[]");
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
            string jsonString = System.IO.File.ReadAllText(path + "\\aeData.json");
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

        public void WriteCommands(string path, Colonie? colonie, DataGetColonieRequest colonieData)
        {

            if (colonie == null) return;
            //DataGetColonieRequest colonieData = await GetRequest("/worlds/" + worldPath + "/colonies/" + colonie.Name);
            List<SpecifiedRequest> requestList = new List<SpecifiedRequest>();
            bool autocomplete = true;
            bool armorComplete = false;
            bool toolComplete = false;
            if (colonieData != null)
            {
                autocomplete = colonieData.autocomplete;
                armorComplete = colonieData.autoArmor;
                toolComplete = colonieData.autoTools;
            }
            
            List<string> autocompleteList = new List<string>();
            if (autocomplete)
            {
                colonie.BuilderRequests.ForEach(x => requestList.AddRange(x.Requests));
            }
            else
            {
                List<BuilderRequests> builderRequests = colonie.BuilderRequests.Where(x => autocompleteList.Contains(x.name)).ToList();
                foreach (BuilderRequests builderRequest in builderRequests)
                {
                    requestList.AddRange(builderRequest.Requests);
                }
            }


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
                            if (colonyItem.amount - colonyReserve[request.item.name] <= 0) continue;

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
                            if (playerItem.amount - playerReserve[request.item.name] <= 0) continue;

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
                }

            }
            if (toolComplete == true || armorComplete == true)
            {
                foreach (Requests request in colonie.Requests)
                {
                    if (toolComplete == true)
                    {
                        if (request.items.Where(x => x.tags.Contains("minecraft:item/forge:tools")).Count() > 0)
                        {
                            StorageItem reqItem = colonie.items.items.playerSide.FirstOrDefault(x => x.name.Equals(request.items[0].name));
                            if (reqItem != null)
                            {
                                commands.Add(new Commands { Amount = 1, Item = request.items[0].name, NeedsCrafting = false });
                            }
                            else
                            {
                                StorageItem patternItem = colonie.items.patterns.FirstOrDefault(x => x.name.Equals(request.items[0].name));
                                if (patternItem != null)
                                {
                                    commands.Add(new Commands { Amount = 1, Item = request.items[0].name, NeedsCrafting = true });
                                }
                            }

                        }
                    }
                    if (armorComplete == true)
                    {
                        string naam = request.items[0].name;
                        if (request.items.Where(x => x.tags.Contains("minecraft:item/forge:armors")).Count() > 0)
                        {
                            StorageItem reqItem = colonie.items.items.playerSide.FirstOrDefault(x => x.name.Equals(request.items[0].name));
                            if (reqItem != null)
                            {
                                commands.Add(new Commands { Amount = 1, Item = request.items[0].name, NeedsCrafting = false });
                            } else
                            {
                                StorageItem patternItem = colonie.items.patterns.FirstOrDefault(x => x.name.Equals(request.items[0].name));
                                if (patternItem != null )
                                {
                                    commands.Add(new Commands { Amount = 1, Item = request.items[0].name, NeedsCrafting = true });
                                }
                            }
                            
                        }
                    }
                    //commands.Add(new Commands { Amount = 1, Item = request.items[0].name, NeedsCrafting = false });
                }
            }

            try
            {
                string data = JsonConvert.SerializeObject(commands);
                System.IO.File.WriteAllText(path + "\\commands.json", data);
            }
            catch
            {

            }
            if (colonie.BuilderRequests.Count != 0)
            {
                foreach (BuilderRequests item in colonie.BuilderRequests)
                {
                    item.colonies_id = colonieData.id;
                }
                PostRequest(JsonConvert.SerializeObject(colonie.BuilderRequests),"/builderrequests");
            }
            if (colonie.Requests.Count != 0)
            {
                foreach (Requests item in colonie.Requests)
                {
                    item.colonies_id = colonieData.id;
                    item.fingerprint = item.id;
                }
                 
                PostRequest(JsonConvert.SerializeObject(colonie.Requests), "/requests");
            }
            
            //PostRequest(path, "/builderrequests");
        }

        public void setInstance(string v)
        {
            string pathData = JsonConvert.SerializeObject(v);
            System.IO.File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + "../../../../DataImplementation/PathStorage.json", pathData);
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
            WorldPaths = new List<WorldPath>();
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
                            if (System.IO.File.Exists(dir2 + "\\aeInterface.lua") && System.IO.File.Exists(dir2 + "\\extractTasks.lua") && System.IO.File.Exists(dir2 + "\\JsonFileHelper.lua") && System.IO.File.Exists(dir2 + "\\main.lua") && System.IO.File.Exists(dir2 + "\\monitorWriter.lua") && System.IO.File.Exists(dir2 + "\\wrapPeripherals.lua"))
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
            List<string> recipeFiles = new List<string>();
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
                        foreach (string recipeDir in recipeDirs)
                        {
                            recipeFiles.AddRange(Directory.EnumerateFiles(recipeDir, "*", SearchOption.AllDirectories));
                        }
                    }
                }
            }

            if (Recipes == null) Recipes = new List<Recipe>();
            else Recipes.Clear();

            foreach (string recipeFile in recipeFiles)
            {
                try
                {
                    Recipes.Add(ExtractRecipe(recipeFile));
                } catch
                {

                }
            }
        }

        private Recipe ExtractRecipe(string recipePath)
        {
            Recipe recipe = new Recipe { Inputs = new Dictionary<string, int>(), Results = new Dictionary<string, int>() };
            string jsonString = System.IO.File.ReadAllText(recipePath);
            JsonNode recipeNode = JsonNode.Parse(jsonString)!;
            recipe.Type = recipeNode["type"]!.ToString();
            string resultName = "";
            int resultCount = 0;
            JsonNode? ingredientName = null;
            switch (recipe.Type)
            {
                case "recipe":
                    if (recipeNode["result"] == null) break;
                    resultName = recipeNode["result"]!.ToString();
                    resultCount = ((int?)recipeNode["count"]) ?? 1;
                    recipe.Results.Add(resultName, resultCount);
                    var inputs = recipeNode["inputs"]!.AsArray();
                    var inputItemsString = 
                    recipe.Inputs = inputs.Select(x => {
                        string name = x!["item"]!.ToString();
                        int realCount = 1;
                        var count = x!["count"];
                        if (count != null)
                        {
                            realCount = ((int)count);
                        }

                        return new KeyValuePair<string, int>(name, realCount);

                    }).ToDictionary();
                    break;
                case "minecraft:crafting_shaped":
                    var result = recipeNode["result"]!;
                    resultName = result["item"]!.ToString();
                    resultCount = ((int?)result["count"]) ?? 1;
                    recipe.Results.Add(resultName, resultCount);
                    var pattern = recipeNode["pattern"]!.AsArray();
                    Dictionary<char, int> keys = new Dictionary<char, int>();
                    foreach (var item in pattern)
                    {
                        string patternLine = item!.ToString();
                        foreach (char key in patternLine)
                        {
                            if (!keys.ContainsKey(key)) keys.Add(key, 1);
                            else keys[key] += 1;
                        }
                    }
                    var keyDefs = recipeNode["key"]!;
                    foreach (var key in keys)
                    {
                        if (Char.IsWhiteSpace(key.Key)) continue;
                        var itemName = keyDefs[key.Key.ToString()]!["item"];
                        if (itemName == null) itemName = keyDefs[key.Key.ToString()]!["tag"];
                        if (!recipe.Inputs.ContainsKey(itemName!.ToString())) recipe.Inputs.Add(itemName!.ToString(), key.Value);
                    }
                    break;
                case "minecraft:smelting":
                case "minecraft:blasting":
                case "minecraft:campfire_cooking":
                case "minecraft:smoking":
                    ingredientName = recipeNode["ingredient"]!["item"];
                    if (ingredientName == null) ingredientName = recipeNode["ingredient"]!["tag"];
                    if (!recipe.Inputs.ContainsKey(ingredientName!.ToString())) recipe.Inputs.Add(ingredientName!.ToString(), 1);
                    recipe.Results.Add(recipeNode["result"]!.ToString(), 1);
                    break;
                case "minecraft:stonecutting":
                    ingredientName = recipeNode["ingredient"]!["item"];
                    if (ingredientName == null) ingredientName = recipeNode["ingredient"]!["tag"];
                    if (!recipe.Inputs.ContainsKey(ingredientName!.ToString())) recipe.Inputs.Add(ingredientName!.ToString() , 1);
                    recipe.Results.Add(recipeNode["result"]!.ToString(), ((int)recipeNode["count"]!));
                    break;
            }
            return recipe;
        }

        public void Close()
        {
            Directory.Delete(tempPathString, true);
        }
        public void start()
        {
            loopColonies();
        }
        public void CheckStorage()
        {
            string tempPath = AppDomain.CurrentDomain.BaseDirectory + "../../../../DataImplementation/PathStorage.json";
            try
            {
                if (System.IO.File.Exists(tempPath))
                {
                    string jsonString = System.IO.File.ReadAllText(tempPath);
                    InstancePath = System.Text.Json.JsonSerializer.Deserialize<string>(jsonString);
                    GetModPaths();


                    GetWorldPaths();

                    GetRecipes();
                }
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.Message);
            }
        }
        public async Task<bool> PostRequest(string data, string url)
        {
            url = ApiUrl + url;
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpContent content = new StringContent(data, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.PostAsync(url, content);
                    if (response.IsSuccessStatusCode)
                    {
                        // Read the response content as a string
                        Console.WriteLine(response);
                        return true;
                    }
                    else
                    {
                        Console.WriteLine("Error: " + response.StatusCode);
                        return false;
                    }
                }
                catch
                {
                    return false;
                }
            }
        }
        public async Task<string> GetRequest(string url)
        {
            url = ApiUrl + url;
            //url = "http://localhost:8080/api/worlds/AdminsWorld/colonies/SteamBotBro's Colony";
            
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    
                    HttpResponseMessage response = await client.GetAsync(url);
                    if (response.IsSuccessStatusCode)
                    {
                        // Read the response content as a string
                        return await response.Content.ReadAsStringAsync();
                        //string responseBody = await response.Content.ReadAsStringAsync();
                        //return System.Text.Json.JsonSerializer.Deserialize<ColonieGetRequest>(responseBody).Data;
                    }
                    else
                    {
                        Console.WriteLine("Error: " + response.StatusCode);
                    }
                }
                catch (System.Exception exc)
                {
                    Console.WriteLine(exc);
                }
            }
            return null;
        }
    }
}
