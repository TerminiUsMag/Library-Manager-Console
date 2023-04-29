using System.ComponentModel.DataAnnotations;

namespace LibraryManagerConsole.Core.Models
{
    /// <summary>
    /// Book without Id
    /// </summary>
    public class BookViewModel
    {
        public BookViewModel()
        {
            Genres = new List<GenreModel>();
        }
        [Display(Name = "Book title")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "The field '{0}' is required")]
        [StringLength(50, MinimumLength = 4, ErrorMessage = "The field '{0}' must be between {2} and {1} characters")]
        public string Title { get; set; } = null!;
        [Display(Name = "Book author")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "The field '{0}' is required")]
        public AuthorModel Author { get; set; } = null!;
        [Display(Name = "Book genres")]
        public IList<GenreModel> Genres { get; set; } = null!;
        [Display(Name = "Book release date")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "The field '{0}' is required")]
        public DateTime DateOfRelease { get; set; }


    }
}
