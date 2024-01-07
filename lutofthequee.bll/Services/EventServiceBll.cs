using lutofthequee.bll.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using lutoftheque.api.Dto;
using lutoftheque.api.Services;
//using lutoftheque.bll.models;
//using lutoftheque.Entity.Models;
//using Microsoft.EntityFrameworkCore;

namespace lutofthequee.bll.Services
{
    internal class EventServiceBll
    {
        //private readonly lutofthequeContext context; : Déclare une variable privée context de type lutofthequeContext.Le mot-clé readonly indique que cette variable ne peut être assignée qu'au moment de la création de l'objet GameService et pas après.
        private readonly lutofthequeContext context;
        private readonly EventService _eventService;
        private readonly PlayerService _playerService;
        private readonly KeywordService _keywordService;
        private readonly WeightCalculate _weightCalculate;
        private readonly SearchGames _searchGames;


        //Ce constructeur prend un paramètre context de type lutofthequeContext et l'assigne à la variable context de la classe.
        public EventServiceBll(lutofthequeContext context)
        {
            //this.context = context; : this est utilisé pour faire la distinction entre le paramètre context et la variable de classe context.
            this.context = context;
            this._eventService = _eventService;
            this._playerService = _playerService;
            this._keywordService = _keywordService;
            this._weightCalculate = _weightCalculate;
        }

        public List<GameLightDto> ChooseGames(int id)
        {
            //declaration des variables
            List<int> playersAge = new List<int>(); // la liste avec l'age de tous les joueurs
            int youngest = 0; // l'âge du plus jeune
            DateTime today = DateTime.Today; // date d'aujourd'hui

            // récupérer l'évènement actuel
            var actualEvent = _eventService.GetEventById(id);

            // récupérer la durée de l'evènement en minutes
            TimeSpan duration = actualEvent.EndTime - actualEvent.StartTime;
            int durationInMinuts = ((int)duration.TotalMinutes);

            // Récupérer la liste des joueurs de cet évènement
            List<PlayerByEventDto> players = _playerService.GetPlayersByEvent(id);

            //récuperer le nombre de joueurs

            int numberOfPlayers = players.Count;

            //Calculer l'age de chaque joueur et en extraire le plus jeune

            foreach (PlayerByEventDto player in players)
            {
                int playerAge = today.Year - player.Birthdate.Year;
                playersAge.Add(playerAge);
            }

            youngest = playersAge.Min();

            //Calculer le poids de chaque mot-clé et themes par rapport aux choix des joueurs
            // 1. recuperer la liste des mots-clés et themes avec une valeur de 0

            List<KeywordWeight> keywords = _weightCalculate.CreateKeywordWeightList();
            List<ThemeWeight> themes = _weightCalculate.CreateThemeWeightList();

            // pour Keyword, puis pour Theme faire le tour des mots clés de chaque joueurs pour modifier Calculer le poids de chaque theme par rapport aux choix des joueurs

            foreach (KeywordWeight keyword in keywords)
            {
                foreach (PlayerByEventDto player in players)
                {
                    foreach (PlayerKeywordDto playerKeyword in player.PlayerKeywords)
                    {
                        if (playerKeyword.Name == keyword.Name)
                        {
                            int keywordBonus = 0;
                            switch (playerKeyword.Note)
                            {
                                case 1:
                                    keywordBonus = -5;
                                    break;
                                case 2:
                                    keywordBonus = -2;
                                    break;
                                case 4:
                                    keywordBonus = 2;
                                    break;
                                case 5:
                                    keywordBonus = 5;
                                    break;
                                default:
                                    keywordBonus = 0;
                                    break;
                            }
                            keyword.Weight += keywordBonus;
                        }
                    }
                }
            }


            foreach (ThemeWeight theme in themes)
            {
                foreach (PlayerByEventDto player in players)
                {
                    foreach (PlayerThemeDto playerTheme in player.PlayerThemes)
                    {
                        if (playerTheme.Name == theme.Name)
                        {
                            int themeBonus = 0;
                            switch (playerTheme.Note)
                            {
                                case 1:
                                    themeBonus = -5;
                                    break;
                                case 2:
                                    themeBonus = -2;
                                    break;
                                case 4:
                                    themeBonus = 2;
                                    break;
                                case 5:
                                    themeBonus = 5;
                                    break;
                                default:
                                    themeBonus = 0;
                                    break;
                            }
                            theme.Weight += themeBonus;
                        }
                    }
                }
            }



            //Récupérer les 3 mots - clés les plus lourds

            List<string> topTroisKeywords = keywords
                .OrderByDescending(kw => kw.Weight)
                .Take(3)
                .Select(kw => kw.Name)
                .ToList();

            // récupérer la liste des jeux éligibles
            var eligibleGames = players
                .SelectMany(p => p.PlayerGames)
                .Where(pg => pg.Eligible)
                .Select(pg => pg.Name)
                .Distinct()
                .ToList();

            // filtrer les jeux selon ces critères : 
            // ageMin > age du plus jeune joueur
            // nombreJoueurMax > nombreJoueurs participants
            // nombreJoueurMin < nombreJoueurs participants
            // AverageDuration < EventDuration
            // un de ses mots clé est un des 3 mots - clés les plus lourds

            var filteredGames = context.Games
                .Where(g => eligibleGames.Contains(g.GameName) &&
                g.AgeMin <= youngest &&
                g.PlayersMin <= numberOfPlayers &&
                g.PlayersMax >= numberOfPlayers &&
                g.AverageDuration <= durationInMinuts &&
                g.FkKeywords.Any(kw => topTroisKeywords.Contains(kw.KeywordName)))
                .ToList();

            List<GameWithWeightDto> gameDtos = filteredGames
                .Select(g => new GameWithWeightDto
                {
                    GameId = g.GameId,
                    GameName = g.GameName,
                    PlayersMin = g.PlayersMin,
                    PlayersMax = g.PlayersMax,
                    AverageDuration = g.AverageDuration,
                    AgeMin = g.AgeMin,
                    Picture = g.Picture,
                    FkTheme = g.FkTheme.ThemeName,
                    FkKeywords = g.FkKeywords.Select(k => k.KeywordName).ToList(),
                    FkSecondaryThemes = g.FkSecondaryThemes.Select(st => st.ThemeName).ToList(),
                    Weight = 0
                })
                .ToList();

            //Calculer le score de chaque jeux selon le poids des mot-clés + le poids des themes /100 (penser à en faire un double

            // foreach filtered games // foreach keyword puis foreach theme totalWeight

            foreach (GameWithWeightDto game in gameDtos)
            {
                foreach (string gameKeyword in game.FkKeywords)
                {
                    foreach (KeywordWeight keyword in keywords)
                    {
                        if (gameKeyword == keyword.Name)
                        {
                            game.Weight += keyword.Weight;
                        }
                    }
                }
                // pour chaque theme dans la liste des themes existants, si le theme du jeu = le theme de la liste alors on ajoute et à la fin c'est /100 
                foreach (ThemeWeight theme in themes)
                {
                    if (theme.Name == game.FkTheme)
                    {
                        game.Weight += theme.Weight / 100;
                    }
                }
            }

            // trouver les 2 jeux dont le poids total est le plus haut pour le premier mot clé (topTroisKeyword[0]) et les ajouter  à la liste des jeux choisis et les supprimer de la liste des jeux eligibles

            // creer une liste de jeux choisis (vide bien sur) chercher le jeu le plus lourd avec le 1er mot clé, l'ajouter à la liste de jeux choisis et le supprimer de la liste de base... faire ça 2 fois

            List<GameWithWeightDto> choosenGames = new List<GameWithWeightDto>();

            List<GameWithWeightDto> searchInGames = gameDtos;



            for (int i = 0; i < 2; i++)
            {
                // je recupère la liste des jeux avec le keyword spécifique
                List<GameWithWeightDto> specificKeywordGames = _searchGames.SearchGamesWithIndexedKeyword(gameDtos, topTroisKeywords, i);

                // j'extrais les 2 jeux les plus lourds avec ce mot clé
                List<GameWithWeightDto> twoHaviest = _searchGames.getTopTwoHaviestGames(specificKeywordGames);

                // j'ajoute les 2 jeux trouvés à la liste finale

                choosenGames.AddRange(twoHaviest);

                // je supprime les 2 jeux de la liste de base

                foreach (var game in twoHaviest)
                {
                    gameDtos.Remove(game);
                }

            }

            //mapper choosenGames en GameLightDto

            List<GameLightDto> finalChoice = choosenGames
                .Select(g => new GameLightDto
                {
                    GameId = g.GameId,
                    GameName = g.GameName,
                    Picture = g.Picture
                })
                .ToList();

            // Et en théorie j'ai mes 6 jeux

            return finalChoice;
        }
    }
}
