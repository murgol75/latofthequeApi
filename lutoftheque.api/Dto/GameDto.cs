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
        public string? FkTheme { get; set; }
        public List<string>? FkKeywords { get; set; }
        public List<string>? FkSecondaryThemes { get; set; }
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
        public string? FkTheme { get; set; }
        public List<string>? FkKeywords { get; set; }
        public List<string>? FkSecondaryThemes { get; set; }
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
        public string? FkTheme { get; set; }
        public List<string>? FkKeywords { get; set; }
        public List<string>? FkSecondaryThemes { get; set; }

    }
    // version light à afficher dans les résultats de l'event
    public class GameLightDto  
    {
        public int GameId { get; set; } 
        public string? GameName { get; set; }
        public string? Picture { get; set; }
    }
}


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