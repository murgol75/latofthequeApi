using lutoftheque.api.Dto;
using lutoftheque.api.Services;
using lutoftheque.Entity.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace lutoftheque.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayerController : ControllerBase
    {
        private readonly PlayerService _playerService;
        public PlayerController(PlayerService playerService)
        {
            _playerService = playerService;
        }

        [HttpGet("getAllPlayers")]
        public ActionResult<List<Player>> Get()
        {
            var players = _playerService.GetPlayers();

            return Ok(players);
        }

        [HttpGet("getPlayersInAnEvent/{eventId}")]
        public ActionResult<List<Player>> GetPlayerByEventId(int eventId)
        {
            var players = _playerService.GetPlayersByEvent(eventId);

            return Ok(players);
        }
        //[HttpPost("createPlayer")]
        //public IActionResult Create(PlayerCreationDto playerCreated)
        //{
        //    if (playerCreated == null || !ModelState.IsValid) // si le formulaire n'existe pas ou que le modèle n'est pas bien remplis, on renvoie une BadRequest
        //    {
        //        return BadRequest();
        //    }
        //    // sinon on essaye de le poster
        //    try
        //    {
        //        _playerService.CreatePlayer(playerCreated.Nickname, playerCreated.Email, playerCreated.Birthdate, playerCreated.HashPwd,playerCreated.PlayerKeywords,playerCreated.PlayerThemes);
        //        return Ok("Evennement créé");
        //    }
        //    //et si on y arrive pas, on ressort une exception
        //    catch (Exception ex) // ex reçoit les détails de l'erreur... à utiliser quand je gèrerai les exceptions
        //    {
        //        return StatusCode(500, "erreur Interne");
        //    }
        //}
    }
}
