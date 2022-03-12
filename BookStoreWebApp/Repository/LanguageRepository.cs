using BookStoreWebApp.Data;
using BookStoreWebApp.Models;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace BookStoreWebApp.Repository
{

    public class LanguageRepository : ILanguageRepository
    {
        private readonly BookStoreContext _context = null;

        public LanguageRepository(BookStoreContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<LanguageModelDTO>> GetLanguage()
        {
            return await _context.Languages.Select(Language => new LanguageModelDTO()
            {
                ID = Language.ID,
                Language = Language.Language
            }).ToListAsync();
        }
    }
}
