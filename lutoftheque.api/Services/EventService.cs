using lutoftheque.api.Dto;
using lutoftheque.Entity.Models;
using Microsoft.EntityFrameworkCore;
using lutoftheque.bll.Services;

namespace lutoftheque.api.Services
{
    public class EventService
    {
        private readonly lutofthequeContext context;
        private readonly EventServiceBll eventServiceBll;

        //Ce constructeur prend un paramètre context de type lutofthequeContext et l'assigne à la variable context de la classe.
        public EventService(lutofthequeContext context)
        {
            this.context = context;
            this.eventServiceBll = eventServiceBll;
        }
        public List<EventLightDto> GetEvents()
        {
            return context.Events
                .Select(e => new EventLightDto // Mappe l'Event en EvenLightDto
                {
                    EventId = e.EventId,
                    StartTime = e.StartTime,
                    EndTime = e.EndTime,
                    RegistrationClosingDate = e.RegistrationClosingDate,
                    EventName = e.EventName,
                    EventDescription = e.EventDescription,
                    RegistrationClosed = e.RegistrationClosed
                }).ToList();
        }

        public EventFullDto? GetEventById(int id)
        {
            // eventItem car event est un mot dédié sur Csharp
            var eventItem = context.Events 
                .Include(e => e.FkPlayers) // pour chaque event, on join les players associés à l'event
                    .ThenInclude(p => p.PlayerGames) // pour chaque joueur on join également la table de relation joueur possède jeu
                        .ThenInclude(pg => pg.FkGame) // et on joint les jeux correspondant à la FK Game
                .FirstOrDefault(e => e.EventId == id); // retourne la première ligne qui correspond à l'eventId qui est égal à l'Id en paramètre

            if (eventItem == null)
            {
                return null;
            }

            // On le mappe pour en faire un EventFullDto
            EventFullDto eventFullDto = new EventFullDto
            {
                EventId = eventItem.EventId,
                StartTime = eventItem.StartTime,
                EndTime = eventItem.EndTime,
                RegistrationClosingDate = eventItem.RegistrationClosingDate,
                EventName = eventItem.EventName,
                EventDescription = eventItem.EventDescription,
                RegistrationClosed = eventItem.RegistrationClosed,
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

        public void CreateEvent(DateTime start, DateTime end, int id,DateTime close,string eventName, string eventDescription)
        {
            Event newEvent = new Event
            {
                StartTime = start,
                EndTime = end,
                FkOrganizerId = id,
                RegistrationClosingDate = close,
                EventName = eventName,
                EventDescription = eventDescription
    };

            context.Events.Add(newEvent); 
            context.SaveChanges();  // à faire pour enregistrer l'entrée
        }
        public bool UpdateEvent(EventLightDto eventToUpdate)
        {

            Event existingEvent = context.Events.FirstOrDefault(e => e.EventId == eventToUpdate.EventId);

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
            Event eventToDelete = context.Events.FirstOrDefault(e => e.EventId == id);
            if (eventToDelete == null)
            {
                return false; // Événement non trouvé
            }

            context.Events.Remove(eventToDelete);
            context.SaveChanges();
            return true; // Suppression réussie
        }
    }
}