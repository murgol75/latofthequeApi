﻿using lutoftheque.api.Services;
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
        public ActionResult <Game> Get(int gameId)
        {
            var game = _gameService.GetGameById(gameId);
            return Ok(game);
        }
        //[HttpPost("createGame")]

    }
}
