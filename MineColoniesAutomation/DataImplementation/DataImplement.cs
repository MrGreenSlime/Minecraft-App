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
        }
        
        public void setColonie()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "../../../../DataImplementation/requests.json" ;
            string jsonString = File.ReadAllText(path);
            try
            {
                List<Colonie> colonies = JsonSerializer.Deserialize<List<Colonie>>(jsonString);
                foreach (Colonie colo in colonies)
                {
                    foreach (BuilderRequests item in colo.BuilderRequests)
                    {
                        if (item.Requests != null)
                        {
                            using (JsonDocument document = JsonDocument.Parse(item.Requests.ToString()))
                            {
                                JsonElement root = document.RootElement;
                                if (root.ValueKind == JsonValueKind.Array)
                                {
                                    List<SpecifiedRequest> requests = new List<SpecifiedRequest>();
                                    foreach (JsonElement requestElement in root.EnumerateArray())
                                    {
                                        SpecifiedRequest request = JsonSerializer.Deserialize<SpecifiedRequest>(requestElement.GetRawText());
                                        requests.Add(request);
                                    }
                                    item.requests = requests;
                                }
                            }
                        }
                    }
                }
                world.colonies = colonies;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
