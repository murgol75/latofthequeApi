﻿using lutoftheque.api.Exceptions;
using lutoftheque.api.Models;
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
        new Claim(JwtRegisteredClaimNames.Sub, user.PlayerId.ToString()), // ou l'identifiant unique approprié
        new Claim("userId", user.PlayerId.ToString()),
        new Claim(ClaimTypes.Name, user.Nickname),
        new Claim(ClaimTypes.Role, user.IsAdmin ? "Admin" : "User") 
    };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SigningKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                _jwtOptions.Issuer,
                _jwtOptions.Audience,
                claims : claims,
                expires: DateTime.Now.AddSeconds(_jwtOptions.Expiration),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);

            
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

            #region Verification de l'existance d'un joueur avec le même pseudo

            var existingPlayer = context.Players.FirstOrDefault(p => p.Nickname == nickname);
            if (existingPlayer != null)
            {
                throw new DuplicateNicknameException("Ce Pseudo est déjà utilisé");
            }

            #endregion




            context.Players.Add(newPlayer);
            context.SaveChanges();  // à faire pour enregistrer l'entrée

            #region initialisation de toutes les valeurs Keyword à 3
            var keywordIds = context.Keywords.Select(k => k.KeywordId).ToList(); // Récupération de tous les KeywordId

            foreach (var keywordId in keywordIds)
            {
                PlayerKeyword newPlayerKeyword = new PlayerKeyword
                {
                    FkKeywordId = keywordId,
                    FkPlayerId = newPlayer.PlayerId, // Utilisez l'ID du joueur nouvellement créé
                    KeywordNote = 3
                };
                context.PlayerKeywords.Add(newPlayerKeyword);
            }
            context.SaveChanges(); // Enregistrement des nouvelles entrées Player_Keyword
            #endregion

            #region initialisation de toutes les valeurs theme à 3
            var themeIds = context.Themes.Select(t => t.ThemeId).ToList();

            foreach (var themeId in themeIds)
            {
                PlayerTheme newPlayerTheme = new PlayerTheme
                {
                    FkThemeId = themeId,
                    FkPlayerId = newPlayer.PlayerId, // Utilisez l'ID du joueur nouvellement créé
                    ThemeNote = 3
                };
                context.PlayerThemes.Add(newPlayerTheme);
            }
            context.SaveChanges(); // Enregistrement des nouvelles entrées Player_Keyword
            #endregion

        }

    }
}
