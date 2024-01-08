using lutoftheque.bll.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lutoftheque.bll
{
    public class GameSelectionResult
    {
        public IEnumerable<GameLightDtoBll>? Games { get; set; }
        
        public string? ErrorMessage { get; set; }
    }
}
