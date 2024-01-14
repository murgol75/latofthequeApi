﻿using lutoftheque.api.Models;
using lutoftheque.bll.models;
using lutoftheque.bll.Services;
using lutoftheque.bll.Utils;
using lutoftheque.Entity.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace lutoftheque.api.Services
{
    public class AuthService
    {
        private readonly lutofthequeContext context;
        private readonly AuthServiceBll authServiceBll;
        private readonly JwtOptions _jwtOptions;



        //Ce constructeur prend un paramètre context de type lutofthequeContext et l'assigne à la variable context de la classe.
        public AuthService(JwtOptions jwtOptions, lutofthequeContext context)
        {
            _jwtOptions = jwtOptions; 
            this.context = context;
            this.authServiceBll = authServiceBll;
        }

        public Player AuthenticateUser(string nickname, string password)
        {
            Player user = context.Players.FirstOrDefault(u => u.Nickname == nickname);

            if (user != null && PasswordHasher.VerifyPassword(password, user.HashPwd))
            {
                return user;
            }
            return null;
        }


        public bool Login(Player lm)
        {
            Player user = context.Players.FirstOrDefault(u => u.Nickname == lm.Nickname);

            if (user != null) 
            {
                return PasswordHasher.VerifyPassword(lm.HashPwd, user.HashPwd);
            }
            return false;
        }
        public String GenerateJwtToken(Player user) 
        {
            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, user.Nickname),
        new Claim(ClaimTypes.Role, user.IsAdmin ? "Admin" : "User") 
    };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SigningKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                _jwtOptions.Issuer,
                _jwtOptions.Audience,
                claims,
                expires: DateTime.Now.AddSeconds(_jwtOptions.Expiration),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);

            #region à effacer plus tard
            //// generer le token et le renvoyer
            //// 1.stringkey vers byte key
            //byte[] skey = Encoding.UTF8.GetBytes(_jwtOptions.SigningKey);
            //SymmetricSecurityKey laCle = new SymmetricSecurityKey(skey);

            //// 2. les claims
            //Claim infoNom = new Claim(ClaimTypes.Name, lm.nickname);
            //Claim Role = new Claim(ClaimTypes.Role, "Admin"?"User"); // a remplacer par ce qui est reçu de la DB

            //List<Claim> mesClaims = new List<Claim>();
            //mesClaims.Add(infoNom);
            //mesClaims.Add(Role);

            //JwtSecurityToken Token = new JwtSecurityToken(

            //    issuer: _jwtOptions.Issuer,
            //    audience: _jwtOptions.Audience,
            //    claims: mesClaims,
            //    expires: DateTime.Now.AddSeconds(_jwtOptions.Expiration),
            //    signingCredentials: new SigningCredentials(laCle, SecurityAlgorithms.HmacSha256));

            //string TokenToSend = new JwtSecurityTokenHandler().WriteToken(Token);

            //return Ok(new { Nom = lm.nickname, Token = TokenToSend });

            #endregion
        }


        public void CreatePlayer(string nickname, string password, string email, DateTime birthdate, bool isAdmin)
        {
            string hashedPassword = PasswordHasher.HashPassword(password);


            Player newPlayer = new Player
            {
                Nickname = nickname,
                HashPwd = hashedPassword,
                Email = email,
                Birthdate = birthdate,
                IsAdmin = isAdmin
            };

            context.Players.Add(newPlayer);
            context.SaveChanges();  // à faire pour enregistrer l'entrée
        }

        

    }
}