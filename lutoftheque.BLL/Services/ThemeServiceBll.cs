using lutoftheque.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lutoftheque.bll.Services
{
    public class ThemeServiceBll
    {
        private readonly lutofthequeContext _context;

        // Constructeur avec context de type lutofthequeContext.
        public ThemeServiceBll(lutofthequeContext context)
        {
            //le _context dans cette classe est égal au context ajouté en paramètre.
            this._context = context;
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
