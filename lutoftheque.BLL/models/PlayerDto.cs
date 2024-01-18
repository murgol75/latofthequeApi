using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lutoftheque.bll.models
{
    public class PlayerParticipateDto
    {
        public int PlayerId { get; set; }
        public string? Nickname { get; set; }
        public DateTime Birthdate { get; set; }
        public virtual ICollection<PlayerThemeDto> PlayerThemes { get; set; } = new List<PlayerThemeDto>();
        public virtual ICollection<PlayerKeywordDto> PlayerKeywords { get; set; } = new List<PlayerKeywordDto>();
        public virtual ICollection<PlayerGameDto> PlayerGames { get; set; } = new List<PlayerGameDto>();
        public virtual List<int> Events { get; set; } = new List<int>();
    }
    public class PlayerByEventDto
    {
        public int PlayerId { get; set; }
        public string? Nickname { get; set; }
        public DateTime Birthdate { get; set; }
        public virtual ICollection<PlayerThemeDto> PlayerThemes { get; set; } = new List<PlayerThemeDto>();
        public virtual ICollection<PlayerKeywordDto> PlayerKeywords { get; set; } = new List<PlayerKeywordDto>();
        public virtual ICollection<PlayerGameDto> PlayerGames { get; set; } = new List<PlayerGameDto>();
    }
    public class PlayerThemeDto
    {
        public string? Name { get; set; }
        public double? Note { get; set; }
    }
    public class PlayerKeywordDto
    {
        public string? Name { get; set; }
        public int? Note { get; set; }
    }
    public class PlayerGameDto
    {
        public string? Name { get; set; }
        public int? Number { get; set; }
        public bool Eligible { get; set; }
    }

    public class PlayerLightDto // c'est le format retourné en front et stocké dans le localStorage
    {
        public int PlayerId { get; set; }
        public string? Nickname { get; set; }
    }
}
