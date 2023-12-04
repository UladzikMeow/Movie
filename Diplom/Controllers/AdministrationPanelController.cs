using Diplom.Data;
using Diplom.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Diplom.ViewModel;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace Diplom.Controllers
{
    [Authorize(Roles = "admin")]
    public class AdministrationPanelController : Controller
    {

        private readonly ILogger<AdministrationPanelController> _logger;
        private DiplomContext db;
        RoleManager<IdentityRole> _roleManager;
        UserManager<User> _userManager;
        IWebHostEnvironment _appEnvironment;
        public AdministrationPanelController(ILogger<AdministrationPanelController> logger, DiplomContext context, IWebHostEnvironment appEnvironment, RoleManager<IdentityRole> roleManager, UserManager<User> userManager)
        {
            _logger = logger;
            db = context;
            _appEnvironment = appEnvironment;
            _roleManager = roleManager;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            return View(await db.Movies.ToListAsync());
        }
        public async Task<IActionResult> CreateMovie()
        {
            return View(await db.Genres.ToListAsync());
        }

        [HttpPost]
        public async Task<IActionResult> CreateMovie(MovieViewModel UploadMovie, string[] genres)
        {
            ViewBag.Message = "GenrsId: ";
            foreach (var genresItem in genres)
            {
                ViewBag.Message += genresItem + " ";
            }
            Movie movie = new Movie { Description = UploadMovie.Description, Title = UploadMovie.Title };
            if (UploadMovie.MovieImg != null)
            {
                string path = "/imgFilms/" + UploadMovie.MovieImg.FileName;

                using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                {
                    await UploadMovie.MovieImg.CopyToAsync(fileStream);
                }
                movie.MovieImg = path;
            }
            if (genres.Length > 0)
            {
                foreach (var genreId in genres)
                {
                    movie.Genres.Add(db.Genres.FirstOrDefault(dbGenre => dbGenre.id == int.Parse(genreId)));
                }
            }
            db.Movies.Add(movie);

            db.SaveChanges();
            return View(await db.Genres.ToListAsync());
        }
        public async Task<IActionResult> CreateActor()
        {
            return View(await db.Movies.ToListAsync());
        }
        [HttpPost]
        public async Task<IActionResult> CreateActor(string Name, string[] Movies)
        {
            Actor actor = new Actor { Name = Name };
            if (Movies.Length > 0)
            {

                foreach (var MoviesId in Movies)
                {
                    actor.Movies.Add(db.Movies.FirstOrDefault(dbMovie => dbMovie.Id == int.Parse(MoviesId)));
                }
            }
            db.Actors.Add(actor);

            db.SaveChanges();
            return View(await db.Movies.ToListAsync());
        }

        public async Task<IActionResult> UserList()
        {
            return View(await db.Users.ToListAsync());
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            Movie? movie = await db.Movies.Include(m => m.Genres)
                .Include(m => m.Actors)
                .Include(m => m.Reviews)
                    .ThenInclude(r => r.User)
                .Include(m => m.Users)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (movie != null)
            {
                db.Movies.Remove(movie);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> EditRole(string userId)
        {

            User user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {

                var userRoles = await _userManager.GetRolesAsync(user);
                var allRoles = _roleManager.Roles.ToList();
                ChangeRoleViewModel model = new ChangeRoleViewModel
                {
                    UserId = user.Id,
                    UserEmail = user.Email,
                    UserRoles = userRoles,
                    AllRoles = allRoles
                };
                return View(model);
            }

            return NotFound();
        }
        [HttpPost]
        public async Task<IActionResult> EditRole(string userId, List<string> roles)
        {

            User user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {

                var userRoles = await _userManager.GetRolesAsync(user);

                var allRoles = _roleManager.Roles.ToList();
                var addedRoles = roles.Except(userRoles);

                var removedRoles = userRoles.Except(roles);

                await _userManager.AddToRolesAsync(user, addedRoles);

                await _userManager.RemoveFromRolesAsync(user, removedRoles);

                return RedirectToAction("UserList");
            }

            return NotFound();
        }
    }
}
