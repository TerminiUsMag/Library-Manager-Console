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

        public virtual DbSet<Book>? Books { get; set; }
        public virtual DbSet<Author>? Authors { get; set; }
        public virtual DbSet<Genre>? Genres { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=.;Database=Library;User Id=sa;Password=None124578;MultipleActiveResultSets=true;TrustServerCertificate=True;");
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
