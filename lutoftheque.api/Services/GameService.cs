using lutoftheque.api.Dto;
using lutoftheque.Entity.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Reflection.Metadata;
using System.Runtime.Intrinsics.X86;
using System.Text;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

namespace lutoftheque.api.Services
{
    public class GameService
    {
        //private readonly lutofthequeContext context; : Déclare une variable privée context de type lutofthequeContext.Le mot-clé readonly indique que cette variable ne peut être assignée qu'au moment de la création de l'objet GameService et pas après.
        private readonly lutofthequeContext context;
        private readonly EventService _eventService;

        //Ce constructeur prend un paramètre context de type lutofthequeContext et l'assigne à la variable context de la classe.
        public GameService (lutofthequeContext context)
        {
            //this.context = context; : this est utilisé pour faire la distinction entre le paramètre context et la variable de classe context.
            this.context = context;
            
         }


        // public List<Game> GetGames() : Déclare une méthode publique GetGames qui retourne une liste d'objets Game.
        public List<GameDto> GetGames()
        {
            return context.Games
                .Include(g => g.FkTheme)
                .Include(g => g.FkKeywords)
                .Include(g => g.FkSecondaryThemes)
                .Select(g => new GameDto
                {
                    GameId = g.GameId,
                    GameName = g.GameName,
                    PlayersMin = g.PlayersMin,
                    PlayersMax = g.PlayersMax,
                    AverageDuration = g.AverageDuration,
                    AgeMin = g.AgeMin,
                    FkTheme = g.FkTheme.ThemeName, 
                    FkKeywords = g.FkKeywords.Select(k => k.KeywordName).ToList(),
                    FkSecondaryThemes = g.FkSecondaryThemes.Select(st => st.ThemeName).ToList()
                }).ToList();
        }

        public GameFullDto? GetGameById(int id)
        {
            var game = context.Games
                .Include(g => g.FkTheme)
                .Include(g => g.FkKeywords)
                .Include(g => g.FkSecondaryThemes)
                .FirstOrDefault(g => g.GameId == id);

            if (game == null)
            {
                return null;
            }

            var gameFullDto = new GameFullDto
            {
                GameId = game.GameId,
                GameName = game.GameName,
                PlayersMin = game.PlayersMin,
                PlayersMax = game.PlayersMax,
                AverageDuration = game.AverageDuration,
                AgeMin = game.AgeMin,
                Picture = game.Picture,
                GameDescription = game.GameDescription,
                Video = game.Video,
                FkThemeId = game.FkThemeId,
                IsExtension = game.IsExtension,
                FkTheme = game.FkTheme.ThemeName,
                FkKeywords = game.FkKeywords.Select(k => k.KeywordName).ToList(),
                FkSecondaryThemes = game.FkSecondaryThemes.Select(k => k.ThemeName).ToList(),
            };

            return gameFullDto;
        }

        public List<GameFullDto> GetGamesForActualEvent(int eventId)
        {
            return context.Games
                .Include(g => g.FkTheme)
                .Include(g => g.FkKeywords)
                .Include(g => g.FkSecondaryThemes)
                .Select(g => new GameFullDto
                {
                    GameId = g.GameId,
                    GameName = g.GameName,
                    PlayersMin = g.PlayersMin,
                    PlayersMax = g.PlayersMax,
                    AverageDuration = g.AverageDuration,
                    AgeMin = g.AgeMin,
                    FkTheme = g.FkTheme.ThemeName,
                    FkKeywords = g.FkKeywords.Select(k => k.KeywordName).ToList(),
                    FkSecondaryThemes = g.FkSecondaryThemes.Select(st => st.ThemeName).ToList()
                }).ToList();
        }




        public List<GameLightDto> GetGamesForEvent()
        {
            return context.Games
                .Select(g => new GameLightDto
                {
                    GameId = g.GameId,
                    GameName = g.GameName,
                    Picture = g.Picture
                }).ToList();
        }


    }
}
