using lutoftheque.bll.models;
using lutoftheque.bll.Services;
using lutoftheque.Entity.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace lutoftheque.bll.Services
{
    public class WeightCalculate
    {
        private readonly lutofthequeContext context;

        public WeightCalculate(lutofthequeContext context)
        {
            this.context = context;
        }
 
        
        public List<KeywordWeight> CreateKeywordWeightList()
        {
            KeywordServiceBll keywordServiceBll = new KeywordServiceBll(context);

            List<string> keywords = keywordServiceBll.GetKeywordsName();

            List<KeywordWeight> keywordWeight = keywords
                .Select(keywordName => new KeywordWeight(keywordName))
                .ToList();

            return keywordWeight;
        }
        public List<ThemeWeight> CreateThemeWeightList()
        {
            ThemeServiceBll themeServiceBll = new ThemeServiceBll(context);

            List<string> themes = themeServiceBll.GetThemesName();

            List<ThemeWeight> themeWeight = themes
                .Select(themeName => new ThemeWeight(themeName))
                .ToList();

            return themeWeight;
        }

    }
}