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
            Genres = new List<GenreModel>();
        }
        [Display(Name = "Book title")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "The field '{0}' is required")]
        [StringLength(50, MinimumLength = 4, ErrorMessage = "The field '{0}' must be between {2} and {1} characters")]
        public string Title { get; set; } = null!;
        [Display(Name = "Book author")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "The field '{0}' is required")]
        public Author Author { get; set; } = null!;
        [Display(Name = "Book genres")]
        public ICollection<GenreModel> Genres { get; set; } = null!;
        [Display(Name = "Book release date")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "The field '{0}' is required")]
        public DateTime DateOfRelease { get; set; }

        public void AddGenre(string genreName)
        {
            Genres.Add(new GenreModel
            {
                Name = genreName
            });
        }
        public void AddGenre(GenreModel genre)
        {
            Genres.Add(genre);
        }
        public void RemoveGenre(string genreName)
        {
            GenreModel? genreModel = Genres.Where(g => g.Name == genreName).FirstOrDefault();
            if (genreModel == null)
            {
                Console.WriteLine($"No such genre in '{this.Title}' book");
                return;
            }
            var genre = new GenreModel
            {
                Name = genreModel.Name,
                Books = genreModel.Books,
                Id = genreModel.Id,
            };
            Genres.Remove(genre);
        }
    }
}
