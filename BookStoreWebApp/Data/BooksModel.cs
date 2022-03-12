using BookStoreWebApp.CustomValidation;
using BookStoreWebApp.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BookStoreWebApp.Data
{
    public class BooksModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string AuthorName { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public int? PageNo { get; set; }
        public decimal? Price { get; set; }
        [ForeignKey("LanguageID")]
        public int? LanguageId { get; set; }
        public string CoverImageURL { get; set; }
        public string BookPdfURL { get; set; }
        public LanguageModel Language { get; set; }
    }
}
