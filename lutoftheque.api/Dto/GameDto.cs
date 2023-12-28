using lutoftheque.Entity.Models;

namespace lutoftheque.api.Dto
{
    // Pour le GettAll
    public class GameDto // OK
    {
        public int GameId { get; set; } 
        public string? GameName { get; set; }
        public int PlayersMin { get; set; }
        public int PlayersMax { get; set; }
        public int AverageDuration { get; set; }
        public int AgeMin { get; set; }
        public int? FkThemeId { get; set; }
        public virtual Theme? FkTheme { get; set; }
        public virtual ICollection<Keyword> FkKeywords { get; set; } = new List<Keyword>();
        public virtual ICollection<Theme> FkSecondaryThemes { get; set; } = new List<Theme>();
    }
    // Pour le GetById et l'update
    public class GameFullDto  // OK
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
        public virtual Theme? FkTheme { get; set; } 
        public virtual ICollection<Keyword> FkKeywords { get; set; } = new List<Keyword>(); 
        public virtual ICollection<Theme> FkSecondaryThemes { get; set; } = new List<Theme>(); 
    }
    // pour la creation d'un jeu
    public class GameToCreateDto  // OK
    {
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
        public virtual Theme? FkTheme { get; set; }
        public virtual ICollection<Keyword> FkKeywords { get; set; } = new List<Keyword>();
        public virtual ICollection<Theme> FkSecondaryThemes { get; set; } = new List<Theme>();
    }
    // version light à afficher dans les résultats de l'event
    public class GameLightDto  
    {
        public int GameId { get; set; } 
        public string? GameName { get; set; }
        public string? Picture { get; set; }
    }
}