namespace Diplom.Models
{
    public class Review
    {
        public int id { get; set; }
        public int rating { get; set; }
        public string? ReviewText { get; set; }
        public virtual User? User { get; set; }
        public virtual Movie? Movie { get; set; }
    }
}
