using lutoftheque.Entity.Models;

namespace lutoftheque.api.Dto
{
    public class PlayerParticipateDto
    {
        public int PlayerId { get; set; }
        public string? Nickname { get; set; }
        public DateTime Birthdate { get; set; }
        //public virtual ICollection<PlayerTheme> PlayerThemes { get; set; } = new List<PlayerTheme>();
        public virtual ICollection<PlayerThemeDto> PlayerThemes { get; set; } = new List<PlayerThemeDto>();
        public virtual ICollection<PlayerKeywordDto> PlayerKeywords { get; set; } = new List<PlayerKeywordDto>();
        //public virtual ICollection<PlayerKeyword> PlayerKeywords { get; set; } = new List<PlayerKeyword>();
        public virtual ICollection<PlayerGame> PlayerGames { get; set; } = new List<PlayerGame>();
        public virtual ICollection<Event> FkEvents { get; set; } = new List<Event>();
    }
}
