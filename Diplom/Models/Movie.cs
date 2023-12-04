using System.ComponentModel.DataAnnotations;

namespace Diplom.Models
{
    public class Movie
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? MovieImg { get; set; }
        public string? Description { get; set; }

        [Display(Name = "Release Date")]
        [DataType(DataType.Date)]
        public DateTime ReleaseDate { get; set; }
        public virtual List<Review>? Reviews { get; set; } = new List<Review>();
        public virtual List<User> Users { get; set; } = new List<User>();
        public virtual List<Actor> Actors { get; set; } = new List<Actor>();
        public virtual List<Genre> Genres { get; set; } = new List<Genre>();
    }
}
