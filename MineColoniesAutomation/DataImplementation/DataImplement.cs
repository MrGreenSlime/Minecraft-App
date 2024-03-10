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
        public Colonie Colonie { get; set; }

        public DataImplement()
        {
            Colonie = new Colonie();
        }
        public void setColonie()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "../../../../DataImplementation/test.json" ;
            string jsonString = File.ReadAllText(path);
            
            Console.WriteLine(jsonString);
            World World = new World();
            try
            {
                World = JsonSerializer.Deserialize<World>(jsonString);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            

            
        }
    }
}
