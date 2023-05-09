using LibraryManagerConsole.Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagerConsole.Infrastructure.Data
{
    public class LibraryContext : DbContext
    {
        public LibraryContext()
        {
        }
        public LibraryContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Genre> Genres { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=localhost;Database=Library;User Id=sa;Password=Db12345678;MultipleActiveResultSets=true;TrustServerCertificate=True;");
            }
        }
        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    base.OnModelCreating(modelBuilder);
        //}
    }
}
