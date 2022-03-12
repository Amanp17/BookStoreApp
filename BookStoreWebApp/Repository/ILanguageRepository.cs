using BookStoreWebApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookStoreWebApp.Repository
{
    public interface ILanguageRepository
    {
        Task<IEnumerable<LanguageModelDTO>> GetLanguage();
    }
}