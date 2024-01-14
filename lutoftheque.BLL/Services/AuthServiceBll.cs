using lutoftheque.bll.models;
using lutoftheque.bll.Utils;
using lutoftheque.Entity.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lutoftheque.bll.Services
{
    public class AuthServiceBll
    {

        private readonly lutofthequeContext context;

        public AuthServiceBll(lutofthequeContext context)
        {
            this.context = context;
        }

        public void CreateUser(string nickname, string password, string email, DateTime birthdate)
        {
            string hashedPassword = PasswordHasher.HashPassword(password);

            // Création d'une nouvelle instance de Player avec les données fournies
            Player newPlayer = new Player
            {
                Nickname = nickname,
                HashPwd = hashedPassword,
                Email = email,
                Birthdate = birthdate,
                IsAdmin = false
            };






            //LoginModel newUser = new LoginModel { nickname = nickname, password = hashedPassword };
            context.Players.Add(newPlayer);  // faire un mappage
            context.SaveChanges();
        }

        public PlayerLightDto? GetPlayerInfo(string nickname)
        {
            // recuperation de l'Evenement avec la liste des joueurs participants et la liste des jeux disponibles (variable eventItem car event est un mot dédié sur Csharp
            Player? player = context.Players
                .FirstOrDefault(e => e.Nickname == nickname);

            // verification de la presence de l'évènement
            if (player == null)
            {
                return null;
            }

            // mappage de player pour le transformer en PlayerLightDto
        PlayerLightDto playerLight = new PlayerLightDto
            {
                PlayerId = player.PlayerId,
                Nickname = player.Nickname,
                Birthdate = player.Birthdate
            };

            return playerLight;
        }




    }
}





