using lutoftheque.bll.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace lutoftheque.bll.Services
{

    public class SearchGames  
    {
        // methode pour chercher des jeux qui comportent le motclé TopThreeKeywords indexé
        public List<GameWithWeightDto> SearchGamesWithIndexedKeyword(List<GameWithWeightDto> games, List<string> topThreeKeywords, int keywordIndex)
        {
            if (topThreeKeywords.Count > keywordIndex)
            {
                string selectedKeyword = topThreeKeywords[keywordIndex];
                List<GameWithWeightDto> filteredGameDtos = games
                    .Where(g => g.FkKeywords.Contains(selectedKeyword))
                    .ToList();

                return filteredGameDtos;
            }

            return new List<GameWithWeightDto>(); // Retourne une liste vide si l'index est hors limites
        }

        // méthode qui va prendre les 2 jeux les plus lourds dans la liste ci-dessus
        public List<GameWithWeightDto> getTopTwoHaviestGames(List<GameWithWeightDto> games)
        {
            // si la liste est nulle (il n'y a rien dans la liste), on ne renvoie rien (déjà traité)
            if (games == null || !games.Any())
            {
                return new List<GameWithWeightDto>(); // Retourne une liste vide si la liste est vide ou null
            }

            List <GameWithWeightDto> gamesList = games.OrderByDescending(g => g.Weight).Take(2).ToList();

            return gamesList.OrderByDescending(g => g.Weight).Take(2).ToList();
        }

    }
}