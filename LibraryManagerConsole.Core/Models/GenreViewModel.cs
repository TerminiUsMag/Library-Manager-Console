using LibraryManagerConsole.Infrastructure.Data.Models;
using System.ComponentModel.DataAnnotations;

namespace LibraryManagerConsole.Core.Models
{
    /// <summary>
    /// Genre model without Id
    /// </summary>
    public class GenreViewModel
    {
        public GenreViewModel()
        {
            Books = new List<Book>();
        }
        [Display(Name = "Genre name")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "The field '{0}' is required")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "The field '{0}' must be between {2} and {1} characters")]
        public string Name { get; set; } = null!;
        [Display(Name = "Books")]
        public ICollection<Book> Books { get; set; }
    }
}
