using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookStoreWebApp.Data
{
    public class LanguageModel
    {
        [Key]
        public int ID { get; set; }
        public string Language { get; set; }
        public ICollection<BooksModel> Books { get; set; }
    }
}
