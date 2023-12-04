using Microsoft.AspNetCore.Identity;

namespace Diplom.Models
{
    public class User : IdentityUser
    {
        public virtual List<Review>? Review { get; set; } = new List<Review>();
        public virtual List<Movie>? ViewList { get; set; } = new List<Movie>();
    }
}
