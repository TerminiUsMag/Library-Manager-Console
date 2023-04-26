using LibraryManagerConsole.Infrastructure.Data.Models;
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
            Genres = new List<Genre>();
        }
        [Display(Name = "Book title")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "The field '{0}' is required")]
        [StringLength(50, MinimumLength = 4, ErrorMessage = "The field '{0}' must be between {2} and {1} characters")]
        public string Title { get; set; } = null!;
        [Display(Name = "Book author")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "The field '{0}' is required")]
        public Author Author { get; set; } = null!;
        [Display(Name = "Book genres")]
        public ICollection<Genre> Genres { get; set; } = null!;
        [Display(Name = "Book release date")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "The field '{0}' is required")]
        public DateTime DateOfRelease { get; set; }

        public void AddGenre(string genreName)
        {
            Genres.Add(new Genre
            {
                Name = genreName
            });
        }
        public void AddGenre(Genre genre)
        {
            Genres.Add(genre);
        }
    }
}
