using BookStoreWebApp.CustomValidation;
using BookStoreWebApp.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookStoreWebApp.Models
{
    public class BookModelDTO
    {
        public int Id { get; set; }
        [Display(Name = "Book Name")]
        [Required(ErrorMessage = "Required")]
        public string Name { get; set; }
        [Display(Name = "Author Name")]
        [Required(ErrorMessage = "Required")]
        public string AuthorName { get; set; }
        [Display(Name = "Book Title")]
        [Required(ErrorMessage = "Required")]
        public string Title { get; set; }
        [Display(Name = "Book Description")]
        [Required(ErrorMessage = "Required")]
        public string Description { get; set; }
        [Display(Name = "Book Category")]
        [Required(ErrorMessage = "Required")]
        public string Category { get; set; }
        [Display(Name = "Total Pages")]
        [Required(ErrorMessage = "Required")]
        public int? PageNo { get; set; }
        [Display(Name = "Book Price")]
        [Required(ErrorMessage = "Required")]
        public decimal? Price { get; set; }
        [Required(ErrorMessage = "Required")]
        public int? LanguageId { get; set; }
        public string Language { get; set; }
        [Display(Name = "Cover Image")]
        [SingleFileValidate(new string[]{".jpg",".png",".jpeg"})]
        [Required(ErrorMessage ="Please Upload Cover Image")]
        public IFormFile CoverPhoto { get; set; }
        public string FilePath { get; set; }
        [SingleFileValidate(new string[] { ".pdf" })]
        [Required(ErrorMessage ="Please Upload PDF File")]
        public IFormFile BookPdf { get; set; }
        public string BookPdfPath { get; set; }

        //[Display(Name ="Other Images")]
        //[MultipleFileValidate(new string[] { ".jpg", ".png", ".jpeg" })]
        //[Required(ErrorMessage = "Please Upload Other Images also")]
        //public IFormFileCollection GalleryImage { get; set; }
        //public List<BookGallery> GalleryList { get; set; }
    }
    
}