using lutoftheque.api.Dto;
using lutoftheque.api.Services;
using lutoftheque.Entity.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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
        [HttpGet("getPlayerById/{playerId}")]
        [Authorize]
        // version ChatGPT 4o

        public ActionResult<Player> GetPlayerById(int playerId)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "userId");
            var userRoleClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role);

            if (userIdClaim == null)
            {
                Console.WriteLine("Claim 'userId' not found");
                return Unauthorized("Claim 'userId' not found");
            }

            if (userRoleClaim == null)
            {
                Console.WriteLine("Claim 'role' not found");
                return Unauthorized("Claim 'role' not found");
            }


            var userId = userIdClaim.Value;
            var userRole = userRoleClaim.Value;
            Console.WriteLine($"Claim 'userId' value: {userId}");
            Console.WriteLine($"Claim 'role' value: {userRole}");

            if (!int.TryParse(userId, out int parsedUserId))
            {
                Console.WriteLine("Invalid 'userId' claim");
                return Unauthorized("Invalid 'userId' claim");
            }

            if (parsedUserId != playerId && userRole != "Admin")
            {
                Console.WriteLine("User ID does not match player ID");
                return Forbid("You are not allowed to access this player's profile");
            }

            var player = _playerService.GetPlayerById(playerId);

            if (player == null)
            {
                return NotFound();
            }


            return Ok(player);
        }











        // version ChatGPT
        //public ActionResult<Player> GetPlayerById(int playerId)
        //{







        //    var userIdString = User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
        //    int userId = 0;
        //    if (int.TryParse(userIdString, out userId))
        //    {
        //        // Conversion réussie, vous pouvez maintenant utiliser userId
        //        var userRole = User.Claims.FirstOrDefault(c => c.Type == "role")?.Value;

        //        if (userId == playerId || userRole == "Admin")
        //        {
        //            var user = _playerService.GetPlayerById(playerId);

        //            if (user == null)
        //            {
        //                return NotFound();
        //            }

        //            return Ok(user);
        //        }
        //    }
        //    else
        //    {
        //        return NotFound();
        //        // Gérer le cas où la conversion échoue
        //        // Par exemple, assignez une valeur par défaut ou lancez une exception
        //    }



        //    return Forbid("Access Denied: Unauthorized to access this profile.");
        //}




        // Ma version
        //public ActionResult<Player> GetPlayerById(int playerId)
        //{
        //    var player = _playerService.GetPlayerById(playerId);

        //    return Ok(player);
        //}

    }
}
