using DataInterface;
using Globals;
using System;
using System.Collections.Generic;
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
        public List<string> WorldPaths { get; set; }
        public List<string> ModPaths { get; set; }
        private string tempPathString = Path.GetTempPath() + "\\MinecoloniesAutomation\\";

        public DataImplement()
        {
            Directory.CreateDirectory(tempPathString);
            world = new World();
            world.colonies = new List<Colonie>();
            WorldPaths = new List<string>();
            setColonie();
            setStorage();
        }
        
        public void setColonie()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "../../../../DataImplementation/requests.json" ;
            string jsonString = File.ReadAllText(path);
            jsonString = jsonString.Replace("\"tags\":{}", "\"tags\":[]");
            jsonString = jsonString.Replace("\"Requests\":{}", "\"Requests\":[]");
            try
            {
                List<Colonie> colonies = JsonSerializer.Deserialize<List<Colonie>>(jsonString);
                world.colonies = colonies;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public void setStorage()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "../../../../DataImplementation/aeData.json";
            string jsonString = File.ReadAllText(path);
            jsonString = jsonString.Replace("\"tags\": {}", "\"tags\": []");
            try
            {
                List<ColonieStorage> Storage = JsonSerializer.Deserialize<List<ColonieStorage>>(jsonString);
                for (int i = 0; i < Storage.Count ; i++)
                {
                    for (int j = 0; j < world.colonies.Count ; j++)
                    {
                        if (Storage[i].colone.colony.Equals(world.colonies[j].Name))
                        {
                            world.colonies[j].items = Storage[i].colone;
                        }
                    }
                }
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void setInstance(string v)
        {
            InstancePath = v;
            //Check if correct mods are installed
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
            //Extract different world paths
            if (Path.Exists(InstancePath + "\\saves"))
            {
                WorldPaths = Directory.EnumerateDirectories(InstancePath + "\\saves").ToList();
            }
            GetRecipes();
        }

        async private void GetRecipes()
        {
            foreach (string modPath in ModPaths)
            {
                ZipFile.ExtractToDirectory(modPath, tempPathString + modPath.Split('\\').Last());
            }
            //TODO find and extract recipes
        }

        public void Close()
        {
            Directory.Delete(tempPathString, true);
        }
    }
}
