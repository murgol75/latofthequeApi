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
        // TESTING : vérifier qu'il y en a minimum 2
        public List<GameWithWeightDto> SearchGamesWithIndexedKeyword(List<GameWithWeightDto> games, List<string> topThreeKeywords, int keywordIndex)
        {
            if (topThreeKeywords.Count > keywordIndex) // si le nombre de mots clés trouvé (enthéorie 3) > que l'index actuel à vérifier (en théorie 0, 1 ou 2) alors on peut chercher
            {
                string selectedKeyword = topThreeKeywords[keywordIndex];  // le mot clé à chercher est égal au mot clé dans le tableau à l'index spécifié
                List<GameWithWeightDto> filteredGameDtos = games
                    .Where(g => g.FkKeywords.Contains(selectedKeyword))
                    .ToList();
                return filteredGameDtos;  // pas de vérification car au pire il renvoie 0 ou 1 jeu et ça sera traité dans la suite de la méthode
            }

            return new List<GameWithWeightDto>(); // Retourne une liste vide si l'index est hors limites
        }

        // méthode qui va prendre les 2 jeux les plus lourds dans la liste ci-dessus
        public List<GameWithWeightDto> getTopTwoHaviestGames(List<GameWithWeightDto> games)
        {
            // vérifier 1. la liste est nulle ?

            if (games == null || !games.Any())
            {
                return new List<GameWithWeightDto>(); // Retourne une liste vide si la liste est vide ou null
            }

           // 2. est-ce qu'il y a moins de 2 jeux && !index[0] ? si oui, il sort le jeu (ou 0 jeux) et la difference sera renvoyée au keyword [0] sous forme de bonus (si keyword [0] alors prendre 2 + ce qui n'a pas été pris par les autres)
           
            // 3. index [0] ? si oui, alors il doit trouver 2+reste jeux 

            // 4. est-ce qu'il y a des égalités ? genre les jeux les plus lourds sont 12 et ont tous le même score ? si oui alors on en fait la liste on les mélange et on prend le nombre nécessaire




            List <GameWithWeightDto> gamesList = games.OrderByDescending(g => g.Weight).Take(2).ToList();



            return gamesList;

            //return gamesList.OrderByDescending(g => g.Weight).Take(2).ToList();
        }

    }
}