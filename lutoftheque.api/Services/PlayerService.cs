using lutoftheque.api.Dto;
using lutoftheque.Entity.Models;
using Microsoft.EntityFrameworkCore;

namespace lutoftheque.api.Services
{
    public class PlayerService
    {
        private readonly lutofthequeContext context;
        public PlayerService(lutofthequeContext context)
        {
            this.context = context;
        }
        public List<PlayerParticipateDto> GetPlayers()
        {
            return context.Players
                .Include(p => p.PlayerKeywords) // on join la relation Player Keyword
                    .ThenInclude(pk => pk.FkKeyword) // on join les keyword par la FkKeyword
                .Include(p => p.PlayerThemes)  // on join la relation Player Theme
                    .ThenInclude(pt => pt.FkTheme)  // on join les themes par la FkTheme

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
        public List<PlayerByEventDto> GetPlayersByEvent(int id)
        {
            return context.Players
                .Include(p => p.PlayerKeywords)  // on join la table d'association Players Keywords
                    .ThenInclude(pk => pk.FkKeyword)  // on la relie via la FK Keyword
                .Include(p => p.PlayerThemes)  // on join la table d'association Players Themes
                    .ThenInclude(pt => pt.FkTheme)  // on la relie via la FK Theme
                .Include(p => p.FkEvents)  // on join la Fk Event
                .Where(p => p.FkEvents.Any(e => e.EventId == id)) // on ne garde que tous les joueurs qui participent à l'event qui correspond à l'ID
                .Select(p => new PlayerByEventDto
                {
                    PlayerId = p.PlayerId, // player Id
                    Nickname = p.Nickname, // player nickname
                    Birthdate = p.Birthdate, // player Birthdate
                    // dans l'evènement choisi
                    
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
                }).ToList();
        }
    }
}
