using BookStoreWebApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookStoreWebApp.Repository
{
    public interface IBookRepository
    {
        Task<int> AddBookDetails(BookModelDTO booksModel);
        Task<List<BookModelDTO>> GetAllBook();
        Task<BookModelDTO> GetByID(int id);
    }
}