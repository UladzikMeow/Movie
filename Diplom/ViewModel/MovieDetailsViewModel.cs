using Diplom.Models;

namespace Diplom.ViewModel
{
    public class MovieDetailsViewModel
    {
        public bool isAddedInViewList { get; set; }
        public Movie? Movie { get; set; }
        public IList<string>? UserRoles { get; set; }

    }
}
