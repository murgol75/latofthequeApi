using lutoftheque.api.Dto;
using lutoftheque.api.Services;
using lutoftheque.Entity.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using lutoftheque.bll.Services;
using lutoftheque.api.Mappers;

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

        [HttpGet("Recupère tous les events")]
        public ActionResult<List<Event>> Get()
        {
            var events = _eventService.GetEvents();
            return Ok(events);
        }
        [HttpGet("GetEventById {id}")]
        public ActionResult<Event> Get(int id)
        {
            var eventItem = _eventService.GetEventById(id);
            return Ok(eventItem);
        }

        [HttpPost("Create Event")]
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

        [HttpPut("Update Event {eventId:int}")]
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

        [HttpDelete("Delete Event {eventId:int}")]
        public IActionResult Delete(int eventId)
        {
            if (_eventService.DeleteEvent(eventId))
            {
                return NoContent(); // Suppression réussie
            }
            return NotFound(); // Événement non trouvé
        }

        [HttpGet("GetGamesForEvent {eventId}")]
        public IActionResult GetChoosenGames(int eventId)
        {
            IEnumerable<GameLightDto>? games = _eventServiceBll.ChooseGamesBll(eventId).Select(g => g.ToDTO());
            
            return Ok(games);
        }
    }
}
