using BookStoreWebApp.Data;
using BookStoreWebApp.Models;
using BookStoreWebApp.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BookStoreWebApp.Controllers
{
    public class BookController : Controller
    {
        private readonly IBookRepository _bookRepository=null;
        private readonly ILanguageRepository _languageRepository=null;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public BookController(IBookRepository bookRepository, ILanguageRepository languageRepository,IWebHostEnvironment webHostEnvironment)
        {
            _bookRepository = bookRepository;
            _languageRepository = languageRepository;
            _webHostEnvironment = webHostEnvironment;
        }
        public async Task<IActionResult> Index()
        {
            var book = await _bookRepository.GetAllBook();
            return View(book);
        }
        [Route("{controller}/{id:int:min(1)}")]
        public async Task<IActionResult> Details(int id)
        {
            var data = await _bookRepository.GetByID(id);
            if(data != null)
            {
                return View(data);
            }
            return NotFound();
        }

        [Authorize]            //This Will Only Allow Signed In User to access this Action Method
        public IActionResult AddBookForm(bool isSuccess = false, int bookid=0)
        {
            //ViewBag.Language =  new SelectList(await _languageRepository.GetLanguage(), "ID", "Language");
            ViewBag.IsSuccess = isSuccess;
            ViewBag.BookId = bookid;
            return View();
        }

        [Authorize]
        public async Task<IActionResult> AddBookData(BookModelDTO bookModel)
        {
           
            if (ModelState.IsValid)
            {
                if (bookModel.CoverPhoto != null)
                {
                    //This will Store the Folder Path Where We have to Store the Image
                    string folder = "images/Books/CoverPhoto/";
                    bookModel.FilePath = await UploadFile(folder, bookModel.CoverPhoto);
                }
                if (bookModel.BookPdf != null)
                {
                    //This will Store the Folder Path Where We have to Store the pdf
                    string folder = "Bookpdf/";
                    bookModel.BookPdfPath = await UploadFile(folder, bookModel.BookPdf);
                }
            }

            if (ModelState.IsValid)
            {
                
                int id = await _bookRepository.AddBookDetails(bookModel);
                if (id > 0)
                {
                    return RedirectToAction(nameof(AddBookForm), new { isSuccess = true, bookid = id });
                }
            }

            //ViewBag.Language = new SelectList(await _languageRepository.GetLanguage(), "ID", "Language");
            return View(nameof(AddBookForm));
        }
        private async Task<string> UploadFile(string folderPath,IFormFile file)
        {
            folderPath += Guid.NewGuid().ToString() + "_" + file.FileName;
            //While working with server the images will be store on the Storage fo for that configuration is done
            string serverFolder = Path.Combine(_webHostEnvironment.WebRootPath, folderPath);
            await file.CopyToAsync(new FileStream(serverFolder, FileMode.Create));
            return "/" + folderPath;
        }
    }
}
