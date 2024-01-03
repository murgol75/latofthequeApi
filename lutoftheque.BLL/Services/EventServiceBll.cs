using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using lutoftheque.api.Dto;
using lutoftheque.api.Services;
using lutoftheque.bll.models;
using lutoftheque.Entity.Models;
using Microsoft.EntityFrameworkCore;

namespace lutoftheque.bll.Services
{
    internal class EventServiceBll
    {
        //private readonly lutofthequeContext context; : Déclare une variable privée context de type lutofthequeContext.Le mot-clé readonly indique que cette variable ne peut être assignée qu'au moment de la création de l'objet GameService et pas après.
        private readonly lutofthequeContext context;
        private readonly EventService _eventService;
        private readonly PlayerService _playerService;
        private readonly KeywordService _keywordService;
        private readonly WeightCalculate _weightCalculate;


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

        public List<GameDto> ChooseGames(int id)
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
                                default :
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
                            int keywordBonus = 0;
                            switch (playerTheme.Note)
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
                            theme.Weight += keywordBonus;
                        }
                    }
                }
            }



            //Récupérer les 3 mots - clés les plus lourds






            //Récupéré les jeux des joueurs présents(nom, agemin, joueursMin, joueursMax, AverageDuration, keyword, themes) avec les critères suivants :
            //ageMin > age du plus jeune joueur
            //nombreJoueurMax > nombreJoueurs participants
            //nombreJoueurMin < nombreJoueurs participants
            //AverageDuration < EventDuration
            //un de ses mots clé est un des 3 mots - clés les plus lourds






            //Calculer le score de chaque jeux selon le poids des mot-clés + le poids des themes / 100






            //prendre les 2 jeux qui ont le plus de points avec le mot clé le plus lord






            //les Mettre dans la liste des jeux choisis et les supprimer de la liste des jeux à choisir






            //prendre les 2 jeux qui ont le plus de points avec le 2ème mot clé le plus lourd






            //les Mettre dans la liste des jeux choisis et les supprimer de la liste des jeux à choisir






            //prendre les 2 jeux qui ont le plus de points avec le 3ème mot clé le plus lourd






            //les Mettre dans la liste des jeux choisis






            //Afficher les 6 jeux choisis







            return null; // en attendant de définir le bon retour

        }


    }
}