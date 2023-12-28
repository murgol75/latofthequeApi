using lutoftheque.Entity.Models;

namespace lutoftheque.api.Services
{
    public class KeywordService
    {

        // déclaration du context lutoftheque dans la variable context. (_ devant context car private)
        private readonly lutofthequeContext _context;

        // Constructeur avec context de type lutofthequeContext.
        public KeywordService(lutofthequeContext context)
        {
            //le _context dans cette classe est égal au context ajouté en paramètre.
            this._context = context;
        }
        // Déclaration de la méthode GetKeywords qui retourne la liste de tous les objets Keyword.
        public List<Keyword> GetKeywords()
        {
            //On return tous les keyword qui sont dans le context DB, ToList() convertit le résultat en une liste d'objets Keyword

            return _context.Keywords.ToList();
        }
    }
}
