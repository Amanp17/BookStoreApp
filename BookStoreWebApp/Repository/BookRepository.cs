using BookStoreWebApp.Data;
using Microsoft.EntityFrameworkCore;
using BookStoreWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStoreWebApp.Repository
{
    public class BookRepository : IBookRepository
    {
        private readonly BookStoreContext _context;

        public BookRepository(BookStoreContext context)
        {
            _context = context;
        }
        public async Task<List<BookModelDTO>> GetAllBook()
        {
            var books = await _context.Books.Select(book => new BookModelDTO
            {
                Id = book.Id,
                Name = book.Name,
                Title = book.Title,
                AuthorName = book.AuthorName,
                Description = book.Description,
                Category = book.Category,
                PageNo = book.PageNo,
                Price = book.Price,
                LanguageId = book.LanguageId,
                Language = book.Language.Language,
                FilePath = book.CoverImageURL
            }).ToListAsync();

            return books;
        }

        public async Task<BookModelDTO> GetByID(int id)
        {
            return await _context.Books
                .Where(x => x.Id == id)
                .Select(book => new BookModelDTO()
                {
                    Id = book.Id,
                    Name = book.Name,
                    Title = book.Title,
                    AuthorName = book.AuthorName,
                    Description = book.Description,
                    Category = book.Category,
                    PageNo = book.PageNo,
                    Price = book.Price,
                    LanguageId = book.LanguageId,
                    Language = book.Language.Language,
                    FilePath = book.CoverImageURL,
                    BookPdfPath = book.BookPdfURL
                    //For Uploading Multiple Images
                    //GalleryList = book.Gallery.Select(g => new BookGallery()
                    //{
                    //    ID = g.ID,
                    //    Name = g.Name,
                    //    URL = g.URL
                    //}).ToList()
                }).FirstOrDefaultAsync();
        }

        public async Task<int> AddBookDetails(BookModelDTO booksModel)
        {
            var databook = new BooksModel()
            {
                Name = booksModel.Name,
                Title = booksModel.Title,
                AuthorName = booksModel.AuthorName,
                Description = booksModel.Description,
                Category = booksModel.Category,
                PageNo = booksModel.PageNo.HasValue ? booksModel.PageNo : 0,
                Price = booksModel.Price.HasValue ? booksModel.Price : 0,
                LanguageId = booksModel.LanguageId.HasValue ? booksModel.LanguageId : 0,
                CoverImageURL = booksModel.FilePath,
                BookPdfURL = booksModel.BookPdfPath
            };

            //For Adding Multiple Photos
            //databook.Gallery = new List<BookGallery>();
            //foreach (var file in booksModel.GalleryList)
            //{
            //    databook.Gallery.Add(new BookGallery()
            //    {
            //        Name = file.Name,
            //        URL=file.URL
            //    });
            //}


            await _context.Books.AddAsync(databook);
            await _context.SaveChangesAsync();
            return databook.Id;
        }
    }
}
