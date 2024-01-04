using lutoftheque.api.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lutoftheque.bll.Services
{

    public class SearchGames
    {
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

        // créer la méthode qui va prendre le jeu le plus lourd dans la liste ci-dessus
        public List<GameWithWeightDto> getTopTwoHaviestGames(List<GameWithWeightDto> games)
        {
            if (games == null || !games.Any())
            {
                return new List<GameWithWeightDto>(); // Retourne une liste vide si la liste est vide ou null
            }

            return games.OrderByDescending(g => g.Weight).Take(2).ToList();
        }

        //// créer la méthode qui va ajouter le jeu trouvé ci-dessus dans la liste des jeux finaux

        //public List<GameWithWeightDto> addGameToChoosenList(List<GameWithWeightDto> newList, List<GameWithWeightDto> games)
        //{
        //    newList.AddRange(games);

        //    return newList;

        //}

        // créer la méthode qui va supprimer le jeu trouvé ci-dessus de la liste des jeux eligibles

        //public List<GameWithWeightDto> deleteGameFromEligibleList(List<GameWithWeightDto> fullList, List<GameWithWeightDto> games)
        //{
        //    foreach (var game in games)
        //    {
        //        fullList.Remove(game);
        //    }
        //    return fullList;

        //}

    }
}