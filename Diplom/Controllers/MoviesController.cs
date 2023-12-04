#nullable disable

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Diplom.Data;
using Diplom.Models;
using Microsoft.AspNetCore.Identity;
using Diplom.ViewModel;

namespace Diplom.Controllers
{
    public class MoviesController : Controller
    {
        private readonly DiplomContext _context;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public MoviesController(DiplomContext context, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }


        public async Task<IActionResult> Index()
        {
            return View(await _context.Movies.ToListAsync());
        }


        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movies
                .Include(m => m.Genres)
                .Include(m => m.Actors)
                .Include(m => m.Reviews)
                    .ThenInclude(r => r.User)
                .Include(m => m.Users)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (movie == null)
            {
                return NotFound();
            }
            MovieDetailsViewModel model = new MovieDetailsViewModel
            {
                Movie = movie,
            };
            if (User.Identity.IsAuthenticated)
            {
                User user = await _userManager.FindByNameAsync(User.Identity.Name);
                if (user != null)
                {
                    if (movie.Users.Contains(user))
                    {
                        var userRoles = await _userManager.GetRolesAsync(user);
                        var allRoles = _roleManager.Roles.ToList();
                        model.isAddedInViewList = true;
                        model.UserRoles = userRoles;
         
                        return View(model);
                    }
                    else
                    {
                        var userRoles = await _userManager.GetRolesAsync(user);
                        var allRoles = _roleManager.Roles.ToList();
                        model.isAddedInViewList = false;
                        model.UserRoles = userRoles;
                        return View(model);
                    }
                }
            }
            
            return View(model);
        }



        [HttpPost]
        public async Task<IActionResult> Details(string userName, int postRating, string textReview, int movieId)
        {
            var movie = await _context.Movies.FirstOrDefaultAsync(m => m.Id == movieId);
            User user = await _userManager.FindByNameAsync(userName);
            Review review = new Review { rating = postRating, Movie = movie, ReviewText = textReview, User = user };
            user.Review.Add(review);
            _context.Reviews.Add(review);
            _context.SaveChanges();
            movie = await _context.Movies
                .Include(m => m.Genres)
                .Include(m => m.Actors)
                .Include(m => m.Reviews)
                    .ThenInclude(r => r.User)
                .Include(m => m.Users)
            .FirstOrDefaultAsync(m => m.Id == movieId);
            MovieDetailsViewModel model = await CreateModel(movieId, user);
            return View(model);
        }



        public async Task<IActionResult> addToViewList(int movieId, string userName)
        {
            User user = await _userManager.FindByNameAsync(userName);
            Movie movie = await _context.Movies
            .FirstOrDefaultAsync(m => m.Id == movieId);
            user.ViewList.Add(movie);
            await _userManager.UpdateAsync(user);
            return RedirectToAction("Details", new { id = movieId });
        }


        [HttpGet]
        public IActionResult Serch(string Title)
        {
            List<Movie> serchedFilms = new List<Movie>();
            if (Title == null) return View(_context.Movies.ToList());
            foreach (Movie film in _context.Movies)
            {
                if (film.Title.Contains(Title))
                {
                    serchedFilms.Add(film);
                }
            }
            return View(serchedFilms);
        }

        [HttpPost]
        public IActionResult addReview()
        {

            return RedirectToAction("Movies/Details");
        }
        private async Task<MovieDetailsViewModel> CreateModel(int movieId,User user )
        {
            var model = new MovieDetailsViewModel();
            var movie = await _context.Movies
                .Include(m => m.Genres)
                .Include(m => m.Actors)
                .Include(m => m.Reviews)
                    .ThenInclude(r => r.User)
                .Include(m => m.Users)
            .FirstOrDefaultAsync(m => m.Id == movieId);
            model.Movie = movie;
            if (movie.Users.Contains(user))
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                var allRoles = _roleManager.Roles.ToList();
                model.isAddedInViewList = true;
                model.UserRoles = userRoles;
                return model;
            }
            else
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                var allRoles = _roleManager.Roles.ToList();
                model.isAddedInViewList = false;
                model.UserRoles = userRoles;
                return model;
            }
        }
    }
}
