using lutoftheque.api.Services;
using lutoftheque.Entity.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace lutoftheque.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private readonly GameService _gameService;

        // Le constructeur reçoit le service GameService via l'injection de dépendances.
        public GameController(GameService gameService)
        {
            _gameService = gameService;
        }

        [HttpGet]
        public ActionResult<List<Game>> Get()
        {
            var games = _gameService.GetGames();

            return Ok(games);
        }

    }
}
