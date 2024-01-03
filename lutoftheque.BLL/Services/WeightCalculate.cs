using lutoftheque.api.Services;
using lutoftheque.bll.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using lutoftheque.api.Services;


namespace lutoftheque.bll.Services
{
    public class WeightCalculate
    {
        //private readonly lutofthequeContext context;
        private readonly EventService _eventService;
        private readonly PlayerService _playerService;
        private readonly KeywordService _keywordService;
        private readonly ThemeService _themeService;

        public List<KeywordWeight> CreateKeywordWeightList()
        { 
        List<string> keywords = _keywordService.GetKeywordsName(); // Récupère les noms des mots-clés
            List<KeywordWeight> keywordWeight = keywords
                .Select(keywordName => new KeywordWeight(keywordName))
                .ToList();

            return keywordWeight;
        }
        public List<ThemeWeight> CreateThemeWeightList()
        {
            List<string> themes = _themeService.GetThemesName(); // Récupère les noms des mots-clés
            List<ThemeWeight> themeWeight = themes
                .Select(themeName => new ThemeWeight(themeName))
                .ToList();

            return themeWeight;
        }

    }
}
