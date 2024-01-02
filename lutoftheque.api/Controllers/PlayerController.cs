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
        [HttpGet]
        public ActionResult<List<Player>> Get()
        {
            var players = _playerService.GetPlayers();

            return Ok(players);
        }

    [HttpGet("{id}")]
    public ActionResult<List<Player>> GetPlayerByEventId(int id)
    {
        var players = _playerService.GetPlayersByEvent(id);

        return Ok(players);
    }
    }
}
