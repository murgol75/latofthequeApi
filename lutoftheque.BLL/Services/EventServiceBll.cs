using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using lutoftheque.bll.models;
using lutoftheque.Entity.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Text.Json; // me sert pour le log, pour enregistrer la valeur des objets

namespace lutoftheque.bll.Services
{
    public class EventServiceBll
    {
        private readonly lutofthequeContext context;
        private readonly WeightCalculate _weightCalculate;
        //private readonly SearchGames _searchGames;
        private readonly ILogger<EventServiceBll> _logger;

        //Ce constructeur prend un paramètre context de type lutofthequeContext et l'assigne à la variable context de la classe.
        public EventServiceBll(lutofthequeContext context, PlayerServiceBll playerServiceBll, WeightCalculate weightCalculate, ILogger<EventServiceBll> logger)
        {
            this.context = context;
            this._weightCalculate = weightCalculate;
            _logger = logger; // me serts pour les logs
        }

        public EventFullDto? GetEventById(int id)
        {
            // recuperation de l'Evenement avec la liste des joueurs participants et la liste des jeux disponibles (variable eventItem car event est un mot dédié sur Csharp
            Event? eventItem = context.Events
                .Include(e => e.FkPlayers)
                    .ThenInclude(p => p.PlayerGames)
                        .ThenInclude(pg => pg.FkGame)
                .FirstOrDefault(e => e.EventId == id);

            // verification de la presence de l'évènement
            if (eventItem == null)
            {
                return null;
            }

            // mappage de eventItem pour le transformer en EventFullDto
            EventFullDto eventFullDto = new EventFullDto
            {
                EventId = eventItem.EventId,
                StartTime = eventItem.StartTime,
                EndTime = eventItem.EndTime,
                ParticipatingPlayers = eventItem.FkPlayers
                    .Select(p => p.Nickname)
                    .ToList(),
                AvailableGames = eventItem.FkPlayers
                    .SelectMany(p => p.PlayerGames)
                    .Where(pg => pg.Eligible)
                    .Select(pg => pg.FkGame.GameName)
                    .ToList(),
            };

            return eventFullDto;
        }

        public List<GameLightDtoBll> ChooseGamesBll(int id)
        {
            //declaration des variables
            List<int> playersAge = new List<int>(); // la liste avec l'age de tous les joueurs
            int youngest = 0; // l'âge du plus jeune
            DateTime today = DateTime.Today; // date d'aujourd'hui
            
            // récupérer l'évènement actuel (avec joueurs et jeux)
            EventFullDto? actualEvent = GetEventById(id); 

            //_logger.LogInformation("**********La valeur de actualEvent est {actualEvent}", JsonSerializer.Serialize(actualEvent));  // insère une ligne dans le log (je garde ici pour l'exemple.  le ********** me sert à repérer mes lignes dans le log)

            // récupérer la durée de l'evènement puis transformation en minutes
            TimeSpan duration = actualEvent.EndTime - actualEvent.StartTime;
            int durationInMinuts = ((int)duration.TotalMinutes);

            // Récupérer la liste des joueurs de cet évènement
            // instanciationde l'objet playerServiceBll de type PlayerServiceBll
            PlayerServiceBll playerServiceBll = new PlayerServiceBll(context);

            // appel de GetPlayersByEvent(id) qui est une méthode dans playersServiceBll pour assigner à players qui est une liste de PlayerByEventDto
            List<PlayerByEventDto> players = playerServiceBll.GetPlayersByEvent(id);

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

            WeightCalculate weightCalculate = new WeightCalculate(context);
            List<KeywordWeight> keywords = weightCalculate.CreateKeywordWeightList();
            List<ThemeWeight> themes = weightCalculate.CreateThemeWeightList();

            // pour Keyword faire le tour des mots clés de chaque joueurs pour modifier Calculer le poids de chaque keyword par rapport aux choix des joueurs
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
                                default :
                                    keywordBonus = 0;
                                    break;
                            }
                            keyword.Weight += keywordBonus;
                        }
                    }
                }
            }

            // Idem pour Theme 
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

            List<string?> eligibleGames = players
                .SelectMany(p => p.PlayerGames)  // prends chaque player dans players et en extrait la lise de jeux
                .Where(pg => pg.Eligible) // seulement si le jeu est eligible
                .Select(pg => pg.Name) // on en ressort le nom
                .Distinct() // on évite les doublons
                .ToList(); // transforme en liste

            // filtrer les jeux selon ces critères : 
            // ageMin > age du plus jeune joueur
            // nombreJoueurMax > nombreJoueurs participants
            // nombreJoueurMin < nombreJoueurs participants
            // AverageDuration < EventDuration
            // un de ses mots clé est un des 3 mots - clés les plus lourds
            
            // TODO : Si un joueur a mis 1(-5) en theme, ce jeu ne peut pas sortir

            List<GameWithWeightDto> filteredGames = context.Games
                .Where(g => eligibleGames.Contains(g.GameName) &&
                g.AgeMin <= youngest &&
                g.PlayersMin <= numberOfPlayers &&
                g.PlayersMax >= numberOfPlayers &&
                g.AverageDuration <= durationInMinuts &&
                g.FkKeywords.Any(kw => topTroisKeywords.Contains(kw.KeywordName)))
                .Select(g => new GameWithWeightDto
                {
                    GameId = g.GameId,
                    GameName = g.GameName,
                    PlayersMin = g.PlayersMin,
                    PlayersMax = g.PlayersMax,
                    AverageDuration = g.AverageDuration,
                    AgeMin = g.AgeMin,
                    Picture = g.Picture,
                    GameDescription = g.GameDescription,
                    Video = g.Video,
                    FkThemeId = g.FkThemeId,
                    IsExtension = g.IsExtension,
                    FkTheme = g.FkTheme.ThemeName,
                    FkKeywords = g.FkKeywords.Select(k => k.KeywordName).ToList(),
                    FkSecondaryThemes = g.FkSecondaryThemes.Select(st => st.ThemeName).ToList(),
                    Weight = 0
                }).ToList();

            //Calculer le score de chaque jeux selon le poids des mot-clés + le poids des themes /100
            foreach (GameWithWeightDto game in filteredGames) // pour chaque jeu dans la liste filtrée
            {
                // calcul pour les keywords
                foreach (string gameKeyword in game.FkKeywords) // pour chaque mot-clé présent dans le jeu
                {
                    foreach(KeywordWeight keyword in keywords) // on fait le tour de tous les mots clés
                    {
                        if (gameKeyword == keyword.Name) // si les 2 mots-clés correspondent alors on modifie le poids du jeu par le poids du mot-clé
                        { 
                            game.Weight += keyword.Weight;
                        }
                    }
                }

                // calcul pour le theme
                foreach(ThemeWeight theme in themes) // on fait le tour de tous les themes
                {
                    if (theme.Name == game.FkTheme) // si les 2 mots-clés correspondent alors on modifie le poids du jeu par le poids du mot-clé
                    {
                        double themeBonus = theme.Weight/100; // le themeBonus est divisé par 100 pour signifier la variation de score
                        game.Weight += themeBonus; // poids du jeu modifié
                    }
                }
            }

            // trouver les 2 jeux dont le poids total est le plus haut pour le premier mot clé (topTroisKeyword[0]) et les ajouter  à la liste des jeux choisis et les supprimer de la liste des jeux eligibles

            // creer une liste de jeux choisis vide (à remplir)bien sur) 
            List<GameWithWeightDto> choosenGames = new List<GameWithWeightDto>();

            // TODO : il prend les jeux par ordre alphabetique s'ils ont le meme score. Donc : SOLUTION 1 : verifier s'il y a au moins 2 jeux par mots clés, si non, prendre un savant mélange pour arriver à 6, exemple, mot clé 2 y a un seul jeu, on sort 3 mots clé 1, 1 mots clé 2, 2 mots clés 3 ... SOLUTION 2 : choix aleatoire parmis tous les possibles. SOLUTION 3 : ajouter des mots clé et themes aux jeux pour avoir des résultats plus variés
            // TODO : A modifier (2) : à la creation un joueur ne peut mettre plus de 1 theme à 1(-5)

            for (int i = 0; i<3;i++)
            {
                // je recupère la liste des jeux avec le keyword spécifique (index 0 puis 1 puis 2)
                SearchGames searchGames = new SearchGames(); // instancier searchGames

                // recuperer les jeux avec le mot-clé spécifique
                List<GameWithWeightDto> specificKeywordGames = searchGames.SearchGamesWithIndexedKeyword(filteredGames, topTroisKeywords, i); 

                // en extraire les 2 jeux les plus lourds
                List<GameWithWeightDto> twoHaviest = searchGames.getTopTwoHaviestGames(specificKeywordGames);

                // les ajouter à la liste finale
                choosenGames.AddRange(twoHaviest);

                // les supprimer de la liste filtrée de base
                foreach (var game in twoHaviest)
                {
                    filteredGames.Remove(game);
                }
            }

            //mapper choosenGames en GameLightDto pour n'en avoir que l'Id, le nom et le dessin qui correspond
            List<GameLightDtoBll> finalChoice = choosenGames
                .Select(g => new GameLightDtoBll
                {
                    GameId = g.GameId,
                    GameName = g.GameName,
                    Picture = g.Picture
                })
                .ToList();

            return finalChoice; 
        }
    }
}