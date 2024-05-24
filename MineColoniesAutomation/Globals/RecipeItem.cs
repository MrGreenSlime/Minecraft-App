using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Globals
{
    public class RecipeItem : IEquatable<RecipeItem>
    {
        public List<string> Items { get; set; } = new List<string>();
        public int Amount {  get; set; }

        public bool Equals(RecipeItem? other)
        {
            if (other == null) return false;
            return Items.Equals(other.Items) && Amount.Equals(other.Amount);
        }
    }
}
