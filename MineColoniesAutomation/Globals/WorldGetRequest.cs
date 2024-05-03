using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Globals
{
    public class WorldGetRequest
    {
        [JsonPropertyName("data")]
        public WorldGetDataRequest Data { get; set; }
    }
}
