using lutoftheque.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lutoftheque.bll.Services
{
    public class KeywordServiceBll
    {
        private readonly lutofthequeContext _context;

        public KeywordServiceBll(lutofthequeContext context)
        {
            _context = context;
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