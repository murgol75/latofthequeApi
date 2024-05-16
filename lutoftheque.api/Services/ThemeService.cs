using lutoftheque.Entity.Models;

namespace lutoftheque.api.Services
{
    public class ThemeService
    {
        private readonly lutofthequeContext _context;

        public ThemeService(lutofthequeContext context)
        {
            this._context = context;
        }
        public List<Theme> GetThemes()
        {
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