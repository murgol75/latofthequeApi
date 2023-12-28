using lutoftheque.Entity.Models;
using System.Reflection.Metadata;
using System.Text;

namespace lutoftheque.api.Services
{
    public class GameService
    {
        //private readonly lutofthequeContext context; : Déclare une variable privée context de type lutofthequeContext.Le mot-clé readonly indique que cette variable ne peut être assignée qu'au moment de la création de l'objet GameService et pas après.
        private readonly lutofthequeContext context;

        //Ce constructeur prend un paramètre context de type lutofthequeContext et l'assigne à la variable context de la classe.
        public GameService (lutofthequeContext context)
        {
            //this.context = context; : this est utilisé pour faire la distinction entre le paramètre context et la variable de classe context.
            this.context = context;
         }
        // public List<Game> GetGames() : Déclare une méthode publique GetGames qui retourne une liste d'objets Game.
        public List<Game> GetGames ()
        {
            //return context.Games.ToList(); : Cette ligne retourne la liste de tous les jeux.context.Games fait référence à la table Games dans la base de données, et ToList() convertit le résultat en une liste d'objets Game
            return context.Games.ToList (); // correspond à "select * from Games"
        }

    }
}
