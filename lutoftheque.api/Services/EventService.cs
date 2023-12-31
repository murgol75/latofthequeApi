﻿using lutoftheque.api.Dto;
using lutoftheque.Entity.Models;
using Microsoft.EntityFrameworkCore;

namespace lutoftheque.api.Services
{
    public class EventService
    {
        //private readonly lutofthequeContext context; : Déclare une variable privée context de type lutofthequeContext.Le mot-clé readonly indique que cette variable ne peut être assignée qu'au moment de la création de l'objet GameService et pas après.
        private readonly lutofthequeContext context;

        //Ce constructeur prend un paramètre context de type lutofthequeContext et l'assigne à la variable context de la classe.
        public EventService(lutofthequeContext context)
        {
            //this.context = context; : this est utilisé pour faire la distinction entre le paramètre context et la variable de classe context.
            this.context = context;
        }
        public List<EventLightDto> GetEvents()
        {
            return context.Events
                .Select(e => new EventLightDto
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
        public EventLightDto UpdateEvent(EventLightDto eventToUpdate)
        {

            var existingEvent = context.Events.FirstOrDefault(e => e.EventId == eventToUpdate.EventId);
            
            // Mettre à jour les propriétés de existingEvent avec celles de eventToUpdate
            existingEvent.StartTime = eventToUpdate.StartTime;
            existingEvent.EndTime = eventToUpdate.EndTime;
            // Mettez à jour d'autres propriétés si nécessaire

            context.SaveChanges();

            return eventToUpdate; // Ou mapper existingEvent à un DTO si nécessaire
        }


    }
}