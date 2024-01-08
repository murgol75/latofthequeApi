using lutoftheque.api.Dto;
using lutoftheque.api.Services;
using lutoftheque.Entity.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using lutoftheque.bll.Services;
using lutoftheque.api.Mappers;
using lutoftheque.bll;
using Swashbuckle.AspNetCore.Annotations;

namespace lutoftheque.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly EventService _eventService;
        private readonly EventServiceBll _eventServiceBll;

        public EventController(EventService eventService, EventServiceBll eventServiceBll)
        {
            _eventService = eventService;
            _eventServiceBll = eventServiceBll;
        }

        [HttpGet("getAllEvents")]
        public ActionResult<List<Event>> Get()
        {
            var events = _eventService.GetEvents();
            return Ok(events);
        }
        [HttpGet("getEventById/{eventId}")]
        public ActionResult<Event> Get(int id)
        {
            var eventItem = _eventService.GetEventById(id);
            return Ok(eventItem);
        }

        [HttpPost("createEvent")]
        public IActionResult Create(EventToCreateDto eventCreated)
        {
            if (eventCreated == null || !ModelState.IsValid) // si le formulaire n'existe pas ou que le modèle n'est pas bien remplis, on renvoie une BadRequest
            {
                return BadRequest();
            }
            // sinon on essaye de le poster
            try
            {
                _eventService.CreateEvent(eventCreated.StartTime, eventCreated.EndTime, eventCreated.FkOrganizerId);
                return Ok("Evennement créé");
            }
            //et si on y arrive pas, on ressort une exception
            catch (Exception ex) // ex reçoit les détails de l'erreur... à utiliser quand je gèrerai les exceptions
            {
                return StatusCode(500, "erreur Interne");
            }
        }

        [HttpPut("updateEvent/{eventId}")]
        public IActionResult Update(int eventId, EventLightDto eventItem) 
        
        {
            
            if (eventItem == null)
            {
                return BadRequest("Cet evènement n'existe pas");
            }

            if (eventItem.EventId != eventId)
            {
                return BadRequest("l'ID de l'évènement ne correspond pas"); // mais je vois pas quand ça peut arriver
            }

            var eventToUpdate = new EventLightDto
            {
                EventId = eventId,
                StartTime = eventItem.StartTime,
                EndTime = eventItem.EndTime,
            };

            if (_eventService.UpdateEvent(eventToUpdate)) 
            { 
                return NoContent(); 
            }
            return NotFound();
        }

        [HttpDelete("deleteEvent/{eventId}")]
        public IActionResult Delete(int eventId)
        {
            if (_eventService.DeleteEvent(eventId))
            {
                return NoContent(); // Suppression réussie
            }
            return NotFound(); // Événement non trouvé
        }

        //[HttpGet("getGamesForEvent/{eventId}")]
        //public IActionResult GetChoosenGames(int eventId)
        //{
        //    IEnumerable<GameLightDto>? games = _eventServiceBll.ChooseGamesBll(eventId).Select(g => g.ToDTO());

        //    return Ok(games);
        //}

[HttpGet("getGamesForEvent/{eventId}")]
        [SwaggerOperation(Summary = "Choisis les jeux", Description = "Permet de déterminer la liste des jeux qui correspondent aux joueurs, et à la durée de l'évènement")]
        public IActionResult GetChoosenGames(int eventId)
{
    var result = _eventServiceBll.ChooseGamesBll(eventId);

    if (!string.IsNullOrEmpty(result.ErrorMessage))
    {
        // Retourne une erreur si le message d'erreur est défini
        return BadRequest(result.ErrorMessage);
    }

    if (result.Games == null || !result.Games.Any())
    {
        // Gère le cas où il n'y a pas de jeux à retourner
        return NotFound($"Aucun jeu trouvé pour l'événement avec l'ID {eventId}");
    }

    return Ok(result.Games.Select(g => g.ToDTO()));
}

    }
}
