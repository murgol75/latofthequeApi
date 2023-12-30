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
            PlayerId = p.PlayerId,
            Nickname = p.Nickname,
            Birthdate = p.Birthdate,
            PlayerKeywords = p.PlayerKeywords
                .Select(pk => new PlayerKeywordDto
                {
                    Name = pk.FkKeyword.KeywordName,
                    Note = pk.KeywordNote
                })
                .ToList(),
            PlayerThemes = p.PlayerThemes
                .Select(pt => new PlayerThemeDto
                {
                    Name = pt.FkTheme.ThemeName,
                    Note = pt.ThemeNote
                })
                .ToList()
            // FkEvents - projeter ces données comme nécessaire
        }).ToList();
        }
    }
}