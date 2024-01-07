using lutoftheque.Entity.Models;

namespace lutoftheque.api.Services
{
    public class KeywordService
    {

        private readonly lutofthequeContext _context;

        public KeywordService(lutofthequeContext context)
        {
            this._context = context;
        }
        // Déclaration de la méthode GetKeywords qui retourne la liste de tous les objets Keyword.
        public List<Keyword> GetKeywords()
        {
            //On return tous les keyword qui sont dans le context DB, ToList() convertit le résultat en une liste d'objets Keyword
            return _context.Keywords.ToList();
        }

        public List<string> GetKeywordsName()
        {
            return _context
                .Keywords
                .Select(k => k.KeywordName)
                .ToList();
        }
    }
}