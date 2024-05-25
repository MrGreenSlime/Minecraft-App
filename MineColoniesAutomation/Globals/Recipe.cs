using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Globals
{
    public class Recipe
    {
        public List<RecipeItem> Results { get; set; }
        public List<RecipeItem> Inputs { get; set; }
        public string Type { get; set; }
    }
}
