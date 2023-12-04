using Diplom.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace Diplom.Data
{
    public class DiplomContext : IdentityDbContext<User>
    {
        public DbSet<Movie> Movies { get; set; } = null!;
        public DbSet<Actor> Actors { get; set; } = null!;
        public DbSet<Genre> Genres { get; set; } = null!;
        public DbSet<Review> Reviews { get; set; } = null!;
        public DiplomContext(DbContextOptions<DiplomContext> options) : base(options)
        {
           // Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Genre>().HasData(
                new Genre[]
            {
                new Genre { id = 1, Gener = "Adventure"},
                new Genre { id = 2, Gener = "Animation"},
                new Genre { id = 3, Gener = "Children"},
                new Genre { id = 4, Gener = "Comedy"},
                new Genre { id = 5, Gener = "Fantasy"},
                new Genre { id = 6, Gener = "Romance"},
                new Genre { id = 7, Gener = "Drama"},
                new Genre { id = 8, Gener = "Thriller"},
                new Genre { id = 9, Gener = "Horror"},
                new Genre { id = 10, Gener = "Mystery"},
                new Genre { id = 11, Gener = "Crime"},
                new Genre { id = 12, Gener = "Sci-Fi"},

            });
            base.OnModelCreating(modelBuilder);
        }


    }
}
