using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryManagerConsole.Infrastructure.Data.Models
{
    public class Author
    {
        [Key]
        [Column("AuthorId")]
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; } = null!;
        [MaxLength(50)]
        [Required]
        public string MiddleName { get; set; } = null!;
        [MaxLength(50)]
        [Required]
        public string LastName { get; set; } = null!;

    }
}