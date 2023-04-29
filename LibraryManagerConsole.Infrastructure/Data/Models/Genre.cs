using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryManagerConsole.Infrastructure.Data.Models
{
    public class Genre
    {
        public Genre()
        {
            Books = new List<Book>();
        }
        [Key]
        [Column("GenreId")]
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = null!;
        public ICollection<Book> Books { get; set; }
        public override string ToString()
        {
            return this.Name;
        }

    }
}