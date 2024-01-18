using lutoftheque.api.Dto;
using lutoftheque.api.Models;
using lutoftheque.api.Services;
using lutoftheque.bll.models;
using lutoftheque.bll.Services;
using lutoftheque.Entity.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace lutoftheque.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly AuthService _authService;
        private readonly AuthServiceBll _authServiceBll;
        private readonly JwtOptions _jwtOptions;

        public AuthController(JwtOptions jwtOptions, AuthService authService, AuthServiceBll authServiceBll)
        {
            _jwtOptions = jwtOptions;
            _authService = authService;
            _authServiceBll = authServiceBll;
        }

        [HttpPost("Login")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PlayerToken))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Login(LoginModel lm)
        {
            if (lm == null)
            {
                return NotFound();

            }
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            // recuperer les informations complementaires sur l'utilisateur : rôle et email

            Player loggedUser = _authService.AuthenticateUser(lm.nickname, lm.password);
            if (loggedUser == null)
            {
                return BadRequest();
            }

            string token = _authService.GenerateJwtToken(loggedUser);
            PlayerLightDto Member = new PlayerLightDto
            {
                PlayerId = loggedUser.PlayerId,
                Nickname = loggedUser.Nickname,
            };

            return Ok(new PlayerToken()
            {
                Member = Member,
                Token = token,
            });

           
        }

        /// <summary>
        /// Obtient les infos de l'utilisateur courant
        /// </summary>
        /// <remarks>
        /// Cette méthode extrait les informations de l'utilisateur à partir du token JWT fourni et renvoie les détails de l'utilisateur.
        /// Elle nécessite que l'utilisateur soit authentifié.
        /// </remarks>
        /// <returns>un joueur (son id et Nickname)</returns>
        [HttpGet("UserInfo")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)] 
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult UserInfo()
        {
            // Extraire le nom d'utilisateur ou l'ID de l'utilisateur à partir du token JWT
            var userName = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;

            if (string.IsNullOrEmpty(userName))
            {
                return Unauthorized();
            }

            // Récupérer les informations de l'utilisateur à partir du nom d'utilisateur
            var user = _authServiceBll.GetPlayerInfo(userName);
            if (user == null)
            {
                return NotFound();
            }

            // Retourner les informations de l'utilisateur
            return Ok(user);
        }

        [HttpPost("CreatePlayer")]
        public IActionResult CreatePlayer(PlayerCreationDto playerCreated)
        {
            if (playerCreated == null || !ModelState.IsValid) // si le formulaire n'existe pas ou que le modèle n'est pas bien remplis, on renvoie une BadRequest
            {
                return BadRequest();
            }
            // sinon on essaye de le poster
            try
            {
                _authService.CreatePlayer(playerCreated.Nickname, playerCreated.HashPwd, playerCreated.Email, playerCreated.Birthdate, playerCreated.IsAdmin);

                return Ok("Player créé");
            }
            //et si on y arrive pas, on ressort une exception
            catch (Exception) // ex reçoit les détails de l'erreur... à utiliser quand je gèrerai les exceptions
            {
                return StatusCode(500, "erreur Interne");
            }
        }

    }
}
