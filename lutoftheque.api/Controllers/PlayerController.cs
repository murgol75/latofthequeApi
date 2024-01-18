using lutoftheque.api.Dto;
using lutoftheque.api.Services;
using lutoftheque.Entity.Models;
using Microsoft.AspNetCore.Authorization;
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
        //[Authorize(Roles ="Admin")] // peut se mettre au dessus d'une methode mais aussi au -dessus du controller pour tout sécuriser
        [HttpGet("getAllPlayers")]
        [Authorize(Roles = "Admin")]
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
       
    }
}
