using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Globals
{
    public class ColonieGetRequest
    {
        [JsonPropertyName("data")]
        public DataGetColonieRequest Data {  get; set; }
    }
}
