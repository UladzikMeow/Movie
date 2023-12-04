namespace Diplom.Models
{
    public class Genre
    {
        public int id { get; set; }
        public string? Gener { get; set; }
        public virtual List<Movie> Movies { get; set; } = new List<Movie>();
    }
}
