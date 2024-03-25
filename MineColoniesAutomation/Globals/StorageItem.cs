using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Globals
{
    public class StorageItem : IComparable
    {
        public string[] tags { get; set; }
        public string name { get; set; }
        public string displayName { get; set; }
        public long amount { get; set; }
        public string fingerprint { get; set; }
        public bool isCraftable { get; set; }
        public Dictionary<string, object> nbt { get; set; }

        public int CompareTo(object other)
        {
            if (!(other is StorageItem))
            {
                return -1;
            }
            if (((StorageItem)other).amount < amount)
            {
                return -1;
            } else if (((StorageItem)other).amount == amount)
            {
                return displayName.CompareTo(((StorageItem)other).displayName);
            }
            return 1;
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return amount + " " + displayName;
        }
        
    }
}
