using lutoftheque.bll.models;
using lutoftheque.bll.Utils;
using lutoftheque.Entity.Models;
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







        }
    }





