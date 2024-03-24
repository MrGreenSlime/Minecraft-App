using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Globals
{
    public class ItemsInStorage
    {
        public StorageItems items {  get; set; }
        public List<StorageItem> patterns { get; set; }
        public string colony { get; set; }
        public override string ToString()
        {
            return colony;
        }
    }
}
