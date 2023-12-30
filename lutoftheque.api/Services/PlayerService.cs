using lutoftheque.api.Dto;
using lutoftheque.Entity.Models;
using Microsoft.EntityFrameworkCore;

namespace lutoftheque.api.Services
{
    public class PlayerService
    {
        //private readonly lutofthequeContext context; : Déclare une variable privée context de type lutofthequeContext.Le mot-clé readonly indique que cette variable ne peut être assignée qu'au moment de la création de l'objet GameService et pas après.
        private readonly lutofthequeContext context;

        //Ce constructeur prend un paramètre context de type lutofthequeContext et l'assigne à la variable context de la classe.
        public PlayerService(lutofthequeContext context)
        {
            //this.context = context; : this est utilisé pour faire la distinction entre le paramètre context et la variable de classe context.
            this.context = context;
        }
        public List<PlayerParticipateDto> GetPlayers()
        {
            return context.Players
                .Include(p => p.PlayerKeywords)
                    .ThenInclude(pk => pk.FkKeyword)
                .Include(p => p.PlayerThemes)
                    .ThenInclude(pt => pt.FkTheme)

                .Select(p => new PlayerParticipateDto
                {
                    PlayerId = p.PlayerId, // player Id
                    Nickname = p.Nickname, // player nickname
                    Birthdate = p.Birthdate, // player Birthdate
                    // les keyword et leurs cotes
                    PlayerKeywords = p.PlayerKeywords
                        .Select(pk => new PlayerKeywordDto
                        {
                            Name = pk.FkKeyword.KeywordName,
                            Note = pk.KeywordNote
                        })
                        .ToList(),
                    // les themes et leurs cotes
                    PlayerThemes = p.PlayerThemes
                        .Select(pt => new PlayerThemeDto
                        {
                            Name = pt.FkTheme.ThemeName,
                            Note = pt.ThemeNote
                        })
                        .ToList(),
                    // Les jeux qui lui appartiennent
                    PlayerGames = p.PlayerGames
                    .Select(pg => new PlayerGameDto
                    {
                        Name = pg.FkGame.GameName,
                        Number = pg.NumberPossessed,
                        Eligible = pg.Eligible
                    })
                    .ToList(),
                    // Les events auxquels il participe
                    Events = p.FkEvents
                    .Select(e => e.EventId)
                    .ToList(),
                }).ToList();
        }
    }
}