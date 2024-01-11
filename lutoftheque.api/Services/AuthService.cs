using lutoftheque.bll.models;
using lutoftheque.bll.Services;
using lutoftheque.bll.Utils;
using lutoftheque.Entity.Models;

namespace lutoftheque.api.Services
{
    public class AuthService
    {
        private readonly lutofthequeContext context;
        private readonly AuthServiceBll authServiceBll;

        //Ce constructeur prend un paramètre context de type lutofthequeContext et l'assigne à la variable context de la classe.
        public AuthService(lutofthequeContext context)
        {
            this.context = context;
            this.authServiceBll = authServiceBll;
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
