using BookStoreWebApp.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Threading.Tasks;

namespace BookStoreWebApp.Data
{
    public class BookStoreContext : IdentityDbContext<AccountUser>
    {
        public BookStoreContext(DbContextOptions<BookStoreContext> options) : base(options)
        {

        }
        public DbSet<BooksModel> Books { get; set; }
        public DbSet<LanguageModel> Languages { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            //By default EF will create DB table with entity class name suffixed by 's'  i.e Product => Products
            modelBuilder.Entity<BooksModel>()
                 .Property(x => x.Price)
                 .HasPrecision(18, 2);
            //By default model Entityframework maps decimal to (18,2) column in database
            base.OnModelCreating(modelBuilder);
        }
    }
}
