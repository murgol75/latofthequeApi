using lutoftheque.bll.models;
using lutoftheque.Entity.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lutoftheque.bll.Services
{
    public class PlayerServiceBll
    {
        private readonly lutofthequeContext context;

        public PlayerServiceBll(lutofthequeContext context)
        {
            this.context = context;
        }
        
        public List<PlayerByEventDto> GetPlayersByEvent(int id)
        {
            return context.Players
                .Include(p => p.PlayerKeywords)
                    .ThenInclude(pk => pk.FkKeyword)
                .Include(p => p.PlayerThemes)
                    .ThenInclude(pt => pt.FkTheme)
                .Include(p => p.FkEvents)
                .Where(p => p.FkEvents.Any(e => e.EventId == id))
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