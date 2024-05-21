using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Globals
{
    public class PostItems
    {
        public string name {  get; set; }
        public string displayName { get; set; }
        public long amount { get; set; }
        public string fingerprint { get; set; }
        public bool isCraftable { get; set; }
        public ItemType type { get; set; }
        public int colonie_id { get; set; }
    }
}
