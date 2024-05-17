using Microsoft.EntityFrameworkCore;
using Netflix_Server.Models.MovieGroup;
using Netflix_Server.Models.UserGroup;
using System.Reflection.Emit;

namespace Netflix_Server.Models.Context
{
    public class MovieContext : DbContext
    {
        // MAPPER || CONTROLLER EDIT

        public MovieContext(DbContextOptions<MovieContext> options)
            : base(options)
        {
            //Database.EnsureDeleted(); 
            if (Database.EnsureCreated())
            {
                List<PricingPlan> plans = new List<PricingPlan>()
                {
                    new PricingPlan
                    {
                        Name = "Premium",
                        Price = 9.99m, // Убедитесь, что цена указана без символа валюты
                        Period = "/ month",
                        Description = "4K (Ultra HD) + HDR",
                        Features = new List<Feature>
                        {
                            new Feature{Name = "Video and sound quality: Best"},
                            new Feature{Name = "Supported devices: TV, computer, mobile phone, tablet" },
                            new Feature{Name = "Devices your household can watch at the same time: 4" },
                            new Feature{Name = "Download devices: 6" }
                        }
                    },
                    new PricingPlan
                    {
                        Name = "Standard",
                        Price = 7.49m,
                        Period = "/ month",
                        Description = "1080p (Full HD)",
                        Features = new List<Feature>
                        {
                            new Feature{Name = "Video and sound quality: Great"},
                            new Feature{Name = "Supported devices: TV, computer, mobile phone, tablet" },
                            new Feature{Name = "Devices your household can watch at the same time: 2" },
                            new Feature{Name = "Download devices: 2" }
                        }
                    },
                    new PricingPlan
                    {
                        Name = "Basic",
                        Price = 4.99m,
                        Period = "/ month",
                        Description = "720p (HD)",
                        Features = new List<Feature>
                        {
                            new Feature{Name = "Video and sound quality: Good"},
                            new Feature{Name = "Supported devices: TV, computer, mobile phone, tablet" },
                            new Feature{Name = "Devices your household can watch at the same time: 1" },
                            new Feature{Name = "Download devices: 1" }
                        }
                    }
                };

                PricingPlans.AddRange(plans);
                SaveChanges();





                List<Image> images = new List<Image>() {
                    new Image {ImageUrl = "https://images.unsplash.com/photo-1467703834117-04386e3dadd8?q=80&w=2070&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D", Alt = "Image 1" },
                    new Image {ImageUrl = "https://images.unsplash.com/photo-1489447068241-b3490214e879?q=80&w=2070&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D", Alt = "Image 2" },
                    new Image {ImageUrl = "https://images.unsplash.com/photo-1492315622343-a50efe7c40a3?q=80&w=2014&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D", Alt = "Image 3" }
                };
                Images.AddRange(images);
                SaveChanges();

                List<Actor> actors = new List<Actor>() {
                    new Actor { Name = "Actor 1" },
                    new Actor { Name = "Actor 2" },
                    new Actor { Name = "Actor 3" }
                };
                Actors.AddRange(actors);
                SaveChanges();

                List<ActorImage> actorImages = new List<ActorImage>()
                {
                    new ActorImage {ActorId = 1, ImageId = 1},
                    new ActorImage {ActorId = 2, ImageId = 2},
                    new ActorImage {ActorId = 3, ImageId = 3}
                };
                ActorImages.AddRange(actorImages);
                SaveChanges();

                List<Company> companies = new List<Company>()
                {
                    new Company {Name = "Company 1" },
                    new Company {Name = "Company 2" },
                    new Company {Name = "Company 3" }
                };
                Companies.AddRange(companies);

                List<CompanyImage> companyImages = new List<CompanyImage>()
                {
                    new CompanyImage {CompanyId = 1, ImageId = 1},
                    new CompanyImage {CompanyId = 2, ImageId = 2},
                    new CompanyImage {CompanyId = 3, ImageId = 3}
                };
                CompanyImages.AddRange(companyImages);

                List<Remark> remarks = new List<Remark>() {
                    new Remark { Name = "Good" },
                    new Remark { Name = "Average" },
                    new Remark { Name = "Poor" }
                };
                Remarks.AddRange(remarks);
                SaveChanges();

                List<Rating> ratings = new List<Rating>()
                {
                    new Rating {Name = "G" },
                    new Rating {Name = "PG" },
                    new Rating {Name = "PG-13" },
                    new Rating {Name = "R" },
                    new Rating {Name = "NC-17" }
                };
                Ratings.AddRange(ratings);
                SaveChanges();

                List<Genre> genres = new List<Genre>()
                {
                    new Genre {Name = "Action" },
                    new Genre {Name = "Comedy" },
                    new Genre {Name = "Drama" },
                    new Genre {Name = "Fantasy" },
                    new Genre {Name = "Horror" }
                };
                Genres.AddRange(genres);
                SaveChanges();

                List<Director> directors = new List<Director>() {
                    new Director {Name = "Director 1"},
                    new Director {Name = "Director 2" },
                    new Director {Name = "Director 3" }
                };
                Directors.AddRange(directors);
                SaveChanges();

                List<DirectorImage> directorImages = new List<DirectorImage>()
                {
                    new DirectorImage {DirectorId = 1, ImageId = 1},
                    new DirectorImage {DirectorId = 2, ImageId = 2},
                    new DirectorImage {DirectorId = 3, ImageId = 3}
                };
                DirectorImages.AddRange(directorImages);
                SaveChanges();


                List<Movie> movies = new List<Movie>()
                {
                    new Movie {Title = "Movie 1", Description = "Description 1", Key = "?v=bvC_0foemLY", StarRating = "5", Runtime = 120, DirectorId = 1, CompanyId = 1, RatingId = 1, RemarkId = 1 },
                    new Movie {Title = "Movie 2", Description = "Description 2", Key = "?v=bvC_0foemLY", StarRating = "4", Runtime = 90, DirectorId = 2, CompanyId = 2, RatingId = 2, RemarkId = 2 }
                };
                Movies.AddRange(movies);
                SaveChanges();
                List<MovieImage> movieImages = new List<MovieImage>()
                {
                    new MovieImage {MovieId = 1, ImageId = 1},
                    new MovieImage {MovieId = 2, ImageId = 2},
                };
                MovieImages.AddRange(movieImages);
                SaveChanges();

            }
        }

        public DbSet<Movie> Movies { get; set; }
        public DbSet<MovieImage> MovieImages { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Remark> Remarks { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Actor> Actors { get; set; }
        public DbSet<ActorImage> ActorImages { get; set; }
        public DbSet<Director> Directors { get; set; }
        public DbSet<DirectorImage> DirectorImages { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<CompanyImage> CompanyImages { get; set; }



        public DbSet<User> Users { get; set; }
        public DbSet<Feature> Features { get; set; }
        public DbSet<PricingPlan> PricingPlans { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure relationships and delete behaviors
            modelBuilder.Entity<Movie>()
                .HasMany(m => m.Genres)
                .WithMany(g => g.Movies)
                .UsingEntity(j => j.ToTable("MovieGenres"));

            modelBuilder.Entity<Movie>()
                .HasMany(m => m.Actors)
                .WithMany(a => a.Movies)
                .UsingEntity(j => j.ToTable("MovieActors"));

            modelBuilder.Entity<Movie>()
                .HasOne(m => m.Director)
                .WithMany(d => d.Movies)
                .HasForeignKey(m => m.DirectorId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Movie>()
                .HasOne(m => m.Company)
                .WithMany(c => c.Movies)
                .HasForeignKey(m => m.CompanyId)
                .OnDelete(DeleteBehavior.Restrict);

            // ACTOR IMAGE
            modelBuilder.Entity<ActorImage>()
                    .HasKey(ai => new { ai.ActorId, ai.ImageId });

            modelBuilder.Entity<ActorImage>()
                .HasOne(ai => ai.Actor)
                .WithMany(a => a.ActorImages)
                .HasForeignKey(ai => ai.ActorId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ActorImage>()
                .HasOne(ai => ai.Image)
                .WithMany()
                .HasForeignKey(ai => ai.ImageId)
                .OnDelete(DeleteBehavior.Restrict);

            // COMPANY IMAGE
            modelBuilder.Entity<CompanyImage>()
                   .HasKey(ai => new { ai.CompanyId, ai.ImageId });

            modelBuilder.Entity<CompanyImage>()
                .HasOne(ai => ai.Company)
                .WithMany(a => a.CompanyImages)
                .HasForeignKey(ai => ai.CompanyId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CompanyImage>()
                .HasOne(ai => ai.Image)
                .WithMany()
                .HasForeignKey(ai => ai.ImageId)
                .OnDelete(DeleteBehavior.Restrict);

            // DIRECTOR IMAGE
            modelBuilder.Entity<DirectorImage>()
                .HasKey(ai => new { ai.DirectorId, ai.ImageId });

            modelBuilder.Entity<DirectorImage>()
                .HasOne(ai => ai.Director)
                .WithMany(a => a.DirectorImages)
                .HasForeignKey(ai => ai.DirectorId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<DirectorImage>()
                .HasOne(ai => ai.Image)
                .WithMany()
                .HasForeignKey(ai => ai.ImageId)
                .OnDelete(DeleteBehavior.Restrict);

            // MOVIE IMAGE
            modelBuilder.Entity<MovieImage>()
                .HasKey(ai => new { ai.MovieId, ai.ImageId });

            modelBuilder.Entity<MovieImage>()
                .HasOne(ai => ai.Movie)
                .WithMany(a => a.MovieImages)
                .HasForeignKey(ai => ai.MovieId)
                .OnDelete(DeleteBehavior.Cascade); // Changed to Cascade

            modelBuilder.Entity<MovieImage>()
                .HasOne(ai => ai.Image)
                .WithMany()
                .HasForeignKey(ai => ai.ImageId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
