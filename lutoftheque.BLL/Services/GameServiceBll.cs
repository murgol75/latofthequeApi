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
    public class GameServiceBll
    {
        //private readonly lutofthequeContext context; : Déclare une variable privée context de type lutofthequeContext.Le mot-clé readonly indique que cette variable ne peut être assignée qu'au moment de la création de l'objet GameService et pas après.
        private readonly lutofthequeContext context;

        //Ce constructeur prend un paramètre context de type lutofthequeContext et l'assigne à la variable context de la classe.
        public GameServiceBll(lutofthequeContext context)
        {
            //this.context = context; : this est utilisé pour faire la distinction entre le paramètre context et la variable de classe context.
            this.context = context;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
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
    }
}
