using lutoftheque.Entity.Models;

namespace lutoftheque.api.Dto
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

    public class PlayerCreationDto
    {
    public string Nickname { get; set; }
    public string Email { get; set; }
    public DateTime Birthdate { get; set; }
    public string HashPwd { get; set; }
    public bool IsAdmin { get; set; }
    }
}
