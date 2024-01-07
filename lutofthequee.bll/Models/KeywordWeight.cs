using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lutofthequee.bll.Models
{
    public class KeywordWeight
    {
        public string Name { get; set; }
        public int Weight { get; set; }

        public KeywordWeight(string name)
        {
            Name = name;
            Weight = 0;
        }
    }
}
