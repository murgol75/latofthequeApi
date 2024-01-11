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

            //if ((lm.nickname != "admin" && lm.nickname != "Mike") || lm.password != "Test1234$")
            //{
            //    return BadRequest();
            //}



            // generer le token et le renvoyer
            // 1.stringkey vers byte key
            byte[] skey = Encoding.UTF8.GetBytes(_jwtOptions.SigningKey);
            SymmetricSecurityKey laCle = new SymmetricSecurityKey(skey);

            // 2. les claims
            Claim infoNom = new Claim(ClaimTypes.Name, "Tof");
            Claim Role = new Claim(ClaimTypes.Role, "admin");

            List<Claim> mesClaims = new List<Claim>();
            mesClaims.Add(infoNom);
            mesClaims.Add(Role);

            JwtSecurityToken Token = new JwtSecurityToken(

                issuer: _jwtOptions.Issuer,
                audience: _jwtOptions.Audience,
                claims: mesClaims,
                expires: DateTime.Now.AddSeconds(_jwtOptions.Expiration),
                signingCredentials: new SigningCredentials(laCle, SecurityAlgorithms.HmacSha256));

            string TokenToSend = new JwtSecurityTokenHandler().WriteToken(Token);

            return Ok(new { Nom = lm.nickname, Token = TokenToSend });
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
