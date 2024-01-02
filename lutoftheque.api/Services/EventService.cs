using lutoftheque.api.Dto;
using lutoftheque.Entity.Models;
using Microsoft.EntityFrameworkCore;

namespace lutoftheque.api.Services
{
    public class EventService
    {
        //private readonly lutofthequeContext context; : Déclare une variable privée context de type lutofthequeContext.Le mot-clé readonly indique que cette variable ne peut être assignée qu'au moment de la création de l'objet GameService et pas après.
        private readonly lutofthequeContext context;
        private readonly EventService _eventService;
        private readonly PlayerService _playerService;

        //Ce constructeur prend un paramètre context de type lutofthequeContext et l'assigne à la variable context de la classe.
        public EventService(lutofthequeContext context)
        {
            //this.context = context; : this est utilisé pour faire la distinction entre le paramètre context et la variable de classe context.
            this.context = context;
            this._eventService = _eventService;
            this._playerService = _playerService;
        }
        public List<EventLightDto> GetEvents()
        {
            return context.Events
                .Select(e => new EventLightDto // select 
                {
                    EventId = e.EventId,
                    StartTime = e.StartTime,
                    EndTime = e.EndTime
                }).ToList();
        }

        public EventFullDto? GetEventById(int id)
        {
            // eventItem car event est un mot dédié sur Csharp
            var eventItem = context.Events
                .Include(e => e.FkPlayers)
                    .ThenInclude(p => p.PlayerGames)
                        .ThenInclude(pg => pg.FkGame)
                .FirstOrDefault(e => e.EventId == id);

            if (eventItem == null)
            {
                return null;
            }

            var eventFullDto = new EventFullDto
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

        public void CreateEvent(DateTime start, DateTime end, int id)
        {
            var newEvent = new Event
            {
                StartTime = start,
                EndTime = end,
                FkOrganizerId = id,

            };

            context.Events.Add(newEvent);
            context.SaveChanges();
        }
        public bool UpdateEvent(EventLightDto eventToUpdate)
        {

            var existingEvent = context.Events.FirstOrDefault(e => e.EventId == eventToUpdate.EventId);

            if(existingEvent == null)
            {
                return false;
            }
            
            existingEvent.StartTime = eventToUpdate.StartTime;
            existingEvent.EndTime = eventToUpdate.EndTime;

            context.SaveChanges();

            return true;
        }

        public bool DeleteEvent(int id)
        {
            var eventToDelete = context.Events.FirstOrDefault(e => e.EventId == id);
            if (eventToDelete == null)
            {
                return false; // Événement non trouvé
            }

            context.Events.Remove(eventToDelete);
            context.SaveChanges();
            return true; // Suppression réussie
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

            //Calculer le poids de chaque mot-clé par rapport aux choix des joueurs






            //Calculer le poids de chaque theme par rapport aux choix des joueurs






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
