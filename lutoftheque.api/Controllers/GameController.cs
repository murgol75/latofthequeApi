using lutoftheque.api.Dto;
using lutoftheque.api.Exceptions;
using lutoftheque.api.Services;
using lutoftheque.Entity.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace lutoftheque.api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize(Roles = "Admin")] // Ici il controlle l'autorization de tous les controllers 

    public class GameController : ControllerBase
    {
        private readonly GameService _gameService;

        // Le constructeur reçoit le service GameService via l'injection de dépendances.
        public GameController(GameService gameService)
        {
            _gameService = gameService;
        }



        [HttpGet("getAllGames")]
        public ActionResult<List<Game>> Get()
        {
            var games = _gameService.GetGames();

            return Ok(games);
        }

        [HttpGet("getGameById/{gameId}")]
        public ActionResult<Game> Get(int gameId)
        {
            var game = _gameService.GetGameById(gameId);
            return Ok(game);
        }
        //[HttpPost("createGame")]
        [HttpPost("createGame")]
        [Authorize(Roles = "Admin")]

        public IActionResult CreateGame([FromBody] GameRequest gameCreated)
        {
            if (gameCreated == null || !ModelState.IsValid) // si le formulaire n'existe pas ou que le modèle n'est pas bien remplis, on renvoie une BadRequest
            {
                return BadRequest();
            }
            // sinon on essaye de le poster
            try
            {
                _gameService.CreateGame(
                    gameCreated.GameName,
                    gameCreated.PlayersMin,
                    gameCreated.PlayersMax,
                    gameCreated.AverageDuration,
                    gameCreated.AgeMin,
                    gameCreated.Picture,
                    gameCreated.GameDescription,
                    gameCreated.Video,
                    gameCreated.FkThemeId,
                    gameCreated.IsExtension,
                    gameCreated.FkKeywordsId);
                return StatusCode(201);
            }

            //// exception si le pseudo est déjà pris
            //catch (DuplicateNicknameException ex)
            //{
            //    return Conflict(ex.Message); // Renvoie un code d'état HTTP 409 avec le message de l'exception
            //}

            //et si on y arrive pas, on ressort une exception
            catch (Exception) // ex reçoit les détails de l'erreur... à utiliser quand je gèrerai les exceptions
            {
                return StatusCode(500, "erreur Interne");
            }
        }




        [HttpGet("getAllThemes")]
        public ActionResult<List<Theme>> GetThemes()
        {
            var themes = _gameService.GetThemes();

            return Ok(themes);
        }

        [HttpGet("getAllKeywords")]
        public ActionResult<List<Keyword>> GetKeywords()
        {
            var keywords = _gameService.GetKeywords();

            return Ok(keywords);
        }


        //SUPPRIMER UN JEU
        [HttpDelete("deleteGame/{gameId}")]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteGame(int gameId)
        {
            //try
            //{
            //    _gameService.DeleteGame(gameId);
            //    return NoContent(); // Renvoie un statut 204 No Content si la suppression est réussie
            //}
            //catch (NotFoundException ex)
            //{
            //    return NotFound(ex.Message); // Renvoie un statut 404 si le jeu n'est pas trouvé
            //}
            //catch (Exception)
            //{
            //    return StatusCode(500, "Erreur interne"); // Renvoie un statut 500 en cas d'erreur interne
            //}

            if (_gameService.DeleteGame(gameId))
            {
                return NoContent(); // Suppression réussie
            }
            return NotFound(); // Événement non trouvé

        }

    }
}