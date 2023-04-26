using LibraryManagerConsole.Infrastructure.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace LibraryManagerConsole.Core.Models
{
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
        [Display(Name = "Book Author")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "The field '{0}' is required")]
        public Author Author { get; set; } = null!;
        public ICollection<Genre> Genres { get; set; } = null!;
        [Required]
        public DateTime DateOfRelease { get; set; }
        [Required]
        public decimal Rating { get; set; }

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
