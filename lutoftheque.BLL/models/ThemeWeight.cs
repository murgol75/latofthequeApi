using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lutoftheque.bll.models
{
    public class ThemeWeight
    {
        public string Name { get; set; }
        public int Weight { get; set; }

        public ThemeWeight(string name)
        {
            Name = name;
            Weight = 0;
        }
    }
}
