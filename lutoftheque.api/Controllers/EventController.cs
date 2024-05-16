using lutoftheque.api.Dto;
using lutoftheque.api.Services;
using lutoftheque.Entity.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using lutoftheque.bll.Services;
using lutoftheque.api.Mappers;
using lutoftheque.bll;
using Swashbuckle.AspNetCore.Annotations;
using Microsoft.AspNetCore.Authorization;

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
        [HttpGet("getEventById/{id}")]
        public ActionResult<Event> Get(int id)
        {
            var eventItem = _eventService.GetEventById(id);
            return Ok(eventItem);
        }

        [HttpPost("createEvent")]
        [Authorize(Roles = "Admin")]
        public IActionResult Create(EventToCreateDto eventCreated)
        {
            if (eventCreated == null || !ModelState.IsValid) // si le formulaire n'existe pas ou que le modèle n'est pas bien remplis, on renvoie une BadRequest
            {
                return BadRequest();
            }
            // sinon on essaye de le poster
            try
            {
                _eventService.CreateEvent(eventCreated.StartTime, eventCreated.EndTime, eventCreated.FkOrganizerId, eventCreated.RegistrationClosingDate, eventCreated.EventName, eventCreated.EventDescription );
                return Ok(eventCreated);
            }
            //et si on y arrive pas, on ressort une exception
            catch (Exception ex) // ex reçoit les détails de l'erreur... à utiliser quand je gèrerai les exceptions
            {
                return StatusCode(500, "erreur Interne");
            }
        }

        [HttpPut("updateEvent/{Id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult Update(int Id, EventLightDto eventItem) 
        
        {
            
            if (eventItem == null)
            {
                return BadRequest("Cet evènement n'existe pas");
            }

            if (eventItem.EventId != Id)
            {
                return BadRequest("l'ID de l'évènement ne correspond pas"); // mais je vois pas quand ça peut arriver
            }

            var eventToUpdate = new EventLightDto
            {
                EventId = Id,
                StartTime = eventItem.StartTime,
                EndTime = eventItem.EndTime,
            };

            if (_eventService.UpdateEvent(eventToUpdate)) 
            { 
                return NoContent(); 
            }
            return NotFound();
        }

        [HttpDelete("deleteEvent/{Id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int Id)
        {
            if (_eventService.DeleteEvent(Id))
            {
                return NoContent(); // Suppression réussie
            }
            return NotFound(); // Événement non trouvé
        }

        [HttpGet("getGamesForEvent/{Id}")]
        [SwaggerOperation(Summary = "Choisis les jeux", Description = "Permet de déterminer la liste des jeux qui correspondent aux joueurs, et à la durée de l'évènement")]
        public IActionResult GetChoosenGames(int Id)
        {

            var result = _eventServiceBll.GetEventInfoToChooseGame(Id);

            // tester su result est rempli, en fait c'est tester errormessage s'il est vide ou pas
            if (result.ErrorMessage != string.Empty)
            {
                return BadRequest(result.ErrorMessage);
            }


            return Ok(result.Games.Select(g => g.ToDTO()));
        }

    }
}
