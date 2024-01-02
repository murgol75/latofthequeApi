using lutoftheque.api.Dto;
using lutoftheque.api.Services;
using lutoftheque.Entity.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace lutoftheque.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly EventService _eventService;

        // Le constructeur reçoit le service GameService via l'injection de dépendances.
        public EventController(EventService eventService)
        {
            _eventService = eventService;
        }

        [HttpGet]
        public ActionResult<List<Event>> Get()
        {
            var games = _eventService.GetEvents();

            return Ok(games);
        }
        [HttpGet("{id}")]
        public ActionResult<Event> Get(int id)
        {
            var eventItem = _eventService.GetEventById(id);
            return Ok(eventItem);
        }
        [HttpPost]
        public IActionResult Create(EventToCreateDto eventCreated)
        {
            if (eventCreated == null || !ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {
                _eventService.CreateEvent(eventCreated.StartTime, eventCreated.EndTime, eventCreated.FkOrganizerId);
                return Ok("Evennement créé");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "erreur Interne");
            }

        }

        [HttpPut("{eventId:int}")]
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
        [HttpDelete("{eventId:int}")]
        public IActionResult Delete(int eventId)
        {
            if (_eventService.DeleteEvent(eventId))
            {
                return NoContent(); // Suppression réussie
            }
            return NotFound(); // Événement non trouvé
        }

    }
}
