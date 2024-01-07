using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lutoftheque.bll.models
{
        public class GameWithWeightDto  // OK
        {
            public int GameId { get; set; }
            public string? GameName { get; set; }
            public int PlayersMin { get; set; }
            public int PlayersMax { get; set; }
            public int AverageDuration { get; set; }
            public int AgeMin { get; set; }
            public string? Picture { get; set; }
            public string? GameDescription { get; set; }
            public string? Video { get; set; }
            public int? FkThemeId { get; set; }
            public bool? IsExtension { get; set; }
            public string? FkTheme { get; set; }
            public List<string>? FkKeywords { get; set; }
            public List<string>? FkSecondaryThemes { get; set; }
            public double Weight { get; set; }
        }
    public class GameLightDtoBll
    {
        public int GameId { get; set; }
        public string? GameName { get; set; }
        public string? Picture { get; set; }
    }
}
