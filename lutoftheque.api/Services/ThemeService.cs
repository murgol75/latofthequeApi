using lutoftheque.Entity.Models;

namespace lutoftheque.api.Services
{
    public class ThemeService
    {
        private readonly lutofthequeContext _context;

        // Constructeur avec context de type lutofthequeContext.
        public ThemeService(lutofthequeContext context)
        {
            //le _context dans cette classe est égal au context ajouté en paramètre.
            this._context = context;
        }
        // Déclaration de la méthode GetKeywords qui retourne la liste de tous les objets Keyword.
        public List<Theme> GetThemes()
        {
            //On return tous les keyword qui sont dans le context DB, ToList() convertit le résultat en une liste d'objets Keyword

            return _context.Themes.ToList();
        }

        public List<string> GetThemesName()
        {
            return _context
                .Themes
                .Select(k => k.ThemeName)
                .ToList();
        }
    }
}

