using DataInterface;
using Globals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DataImplementation
{
    public class DataImplement : DataInterface.DataInterface
    {
        public World world { get; set; }

        public DataImplement()
        {
            world = new World();
            world.colonies = new List<Colonie>();
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
                foreach (ColonieStorage item in Storage)
                {
                    foreach (StorageItem item1 in item.colone.items.playerSide)
                    {
                        item1.amount -= 32;
                    }
                    foreach (StorageItem item1 in item.colone.items.colonySide)
                    {
                        item1.amount -= 32;
                    }
                    foreach (StorageItem item1 in item.colone.patterns)
                    {
                        item1.amount -= 32;
                    }
                }
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
                //world.items = Storage;
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
