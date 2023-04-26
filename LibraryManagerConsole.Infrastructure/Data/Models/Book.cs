using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryManagerConsole.Infrastructure.Data.Models
{
    public class Book
    {
        public Book()
        {
            Genres = new List<Genre>();
        }
        [Key]
        [Column("BookId")]
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Title { get; set; } = null!;
        [Required]
        [ForeignKey(nameof(Author))]
        public int AuthorId { get; set; }
        public Author Author { get; set; } = null!;
        public ICollection<Genre> Genres { get; set; } = null!;
        [Required]
        public DateTime DateOfRelease { get; set; }
        [Required]
        public decimal Rating { get; set; }
    }
}
