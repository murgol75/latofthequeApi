using lutoftheque.Entity.Models;

namespace lutoftheque.api.Services
{
    public class GameService
    {
        private readonly lutofthequeContext _lutofthequeContext;
        public GameService (lutofthequeContext _lutofthequeContext)
        {
            this._lutofthequeContext = _lutofthequeContext;
        }
        public List<Game> GetGames ()
        {
            return _lutofthequeContext.Games.ToList (); // correspond à "select * from Games"
        }

    }
}
