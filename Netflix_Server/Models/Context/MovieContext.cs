using Microsoft.EntityFrameworkCore;
using Netflix_Server.Models.MovieGroup;
using Netflix_Server.Models.SupportGroup;
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
                    // for actors 
                    //1
                    new Image {ImageUrl = "https://hips.hearstapps.com/hmg-prod/images/hlh100123feacover-010-1-650dd73053c63.jpg", Alt = "Chris Hemsworth"},
                    //2
                    new Image {ImageUrl = "https://encrypted-tbn1.gstatic.com/images?q=tbn:ANd9GcTaZlhnryd2kRyQlF-46pVxhsWAzqdydT3d0G3RJf-C41H3FU9t", Alt = "Adam Bessa"},
                    //3
                    new Image {ImageUrl = "https://encrypted-tbn3.gstatic.com/images?q=tbn:ANd9GcRIoTicvnmHhHcF3P2UciV9V36nBmhWPnKNti34EYHZOqzefGIo", Alt = "Golshifteh Farahani"},
                    

                    // for movies
                    //4
                    new Image {ImageUrl = "https://occ-0-2794-2219.1.nflxso.net/dnm/api/v6/E8vDc_W8CLv7-yMQu8KMEC7Rrr8/AAAABZ6ND88pZdT9v-gbiVfmMr8vowAHFfr8ujruNpYcF3Wv3LRAZtC9MRPHiYrREQWia2AAG29hUmfPvn73DDFyhcmjDCR0N47ivOvB.jpg?r=965", Alt= "Extraction II (2023)"},
                    //5
                    new Image {ImageUrl = "https://images.squarespace-cdn.com/content/v1/54fc3c46e4b07795aca279b5/19c6bfb9-6128-44e9-9e71-50bcb52b2114/hou-art-20230310-ltfs-header.jpeg", Alt= "Luther: The Fallen Sun (2023)"},

                    // for companies
                    //6
                    new Image {ImageUrl = "https://marketing.brain.com.ua/static/articles_description_ru/img_20240225182021.jpeg", Alt = "Netflix" },
                    //7
                    new Image {ImageUrl = "https://static.wikia.nocookie.net/marvelcinematicuniverse/images/3/36/Sony_pictures_logo.jpg/revision/latest?cb=20190708084018&path-prefix=ru", Alt = "Sony Pictures" },
                };
                Images.AddRange(images);
                SaveChanges();

                List<Actor> actors = new List<Actor>() {
                    new Actor { Name = "Chris Hemsworth" },      // 1
                    new Actor { Name = "Adam Bessa" },           // 2
                    new Actor { Name = "Golshifteh Farahani" },  // 3 

                    new Actor { Name = "Idris Elba" },    // 4
                    new Actor { Name = "Cynthia Erivo" }, // 5 
                    new Actor { Name = "Andy Serkis" },   // 6 
                };
                Actors.AddRange(actors);
                SaveChanges();

                List<ActorImage> actorImages = new List<ActorImage>()
                {
                    new ActorImage {ActorId = 1, ImageId = 1}, // 1
                    new ActorImage {ActorId = 2, ImageId = 2}, // 2
                    new ActorImage {ActorId = 3, ImageId = 3}, // 3

                    new ActorImage {ActorId = 4, ImageId = 1}, // 4
                    new ActorImage {ActorId = 5, ImageId = 2}, // 5
                    new ActorImage {ActorId = 6, ImageId = 3}, // 6
                };
                ActorImages.AddRange(actorImages);
                SaveChanges();

                List<Company> companies = new List<Company>()
                {
                    new Company {Name = "Netflix" }, // 1
                    new Company {Name = "Sony Pictures" }, // 2
                };
                Companies.AddRange(companies);

                List<CompanyImage> companyImages = new List<CompanyImage>()
                {
                    new CompanyImage {CompanyId = 1, ImageId = 1}, // 1
                    new CompanyImage {CompanyId = 2, ImageId = 2}, // 2
                };
                CompanyImages.AddRange(companyImages);

                List<Remark> remarks = new List<Remark>() {
                    new Remark { Name = "Leaving Soon" },    // 1
                    new Remark { Name = "Added Recently" },  // 2
                    new Remark { Name = "Trending Now" },    // 3
                    new Remark { Name = "Top Picks" },       // 4
                    new Remark { Name = "Popular" },         // 5
                    new Remark { Name = "New Release" },     // 6
                    new Remark { Name = "Staff Pick" },      // 7
                    new Remark { Name = "Award Winner" },    // 8
                    new Remark { Name = "Classic" },         // 9
                    new Remark { Name = "Fan Favorite" }     // 10
                };
                Remarks.AddRange(remarks);
                SaveChanges();

                List<Rating> ratings = new List<Rating>()
                {
                    new Rating { Name = "G" },          //1
                    new Rating { Name = "PG" },         //2
                    new Rating { Name = "PG-13" },      //3
                    new Rating { Name = "R" },          //4
                    new Rating { Name = "NC-17" },      //5
                    new Rating { Name = "Not Rated" },  //6
                    new Rating { Name = "Unrated" },    //7
                    new Rating { Name = "Approved" },   //8
                    new Rating { Name = "Passed" },     //9
                    new Rating { Name = "TV-G" },       //10
                    new Rating { Name = "TV-PG" },      //11
                    new Rating { Name = "TV-14" },      //12
                    new Rating { Name = "TV-MA" },      //13
                    new Rating { Name = "TV-Y" },       //14
                    new Rating { Name = "TV-Y7" },      //15
                    new Rating { Name = "TV-Y7-FV" }    //16
                };
                Ratings.AddRange(ratings);
                SaveChanges();

                List<Genre> genres = new List<Genre>()
                {
                    new Genre { Name = "Action" },      //1
                    new Genre { Name = "Comedy" },      //2
                    new Genre { Name = "Drama" },       //3
                    new Genre { Name = "Fantasy" },     //4
                    new Genre { Name = "Horror" },      //5
                    new Genre { Name = "Crime" },       //6
                    new Genre { Name = "Thriller" },    //7
                    new Genre { Name = "Mystery" },     //8
                    new Genre { Name = "Romance" },     //9
                    new Genre { Name = "Sci-Fi" },      //10
                    new Genre { Name = "Adventure" },   //11
                    new Genre { Name = "Animation" },   //12
                    new Genre { Name = "Biography" },   //13
                    new Genre { Name = "Documentary" }, //14
                    new Genre { Name = "Family" },      //15
                    new Genre { Name = "History" },     //16
                    new Genre { Name = "Musical" },     //17
                    new Genre { Name = "Sport" },       //18
                    new Genre { Name = "War" },         //19
                    new Genre { Name = "Western" }      //20 
                };
                Genres.AddRange(genres);
                SaveChanges();

                List<Director> directors = new List<Director>() {
                    new Director {Name = "Sam Hargrave"},   //1 
                    new Director {Name = "Jamie Payne" },   //2 
                };
                Directors.AddRange(directors);
                SaveChanges();

                List<DirectorImage> directorImages = new List<DirectorImage>()
                {
                    new DirectorImage {DirectorId = 1, ImageId = 1},
                    new DirectorImage {DirectorId = 2, ImageId = 2},
                };
                DirectorImages.AddRange(directorImages);
                SaveChanges();


                List<Movie> movies = new List<Movie>()
                {
                    new Movie {Title = "Extraction II (2023)", Description = "After barely surviving his grievous wounds from his mission in Dhaka, Bangladesh, Tyler Rake is back, and his team is ready to take on their next mission.",
                        Key = "Y274jZs5s7s", StarRating = "4", Runtime = 123, DirectorId = 1, CompanyId = 1, RatingId = 4, RemarkId = null, ReleaseDate = DateTime.Now,
                        Genres = new List<Genre>(){ genres[0], genres[5], genres[6] },
                        Actors = new List<Actor>(){ actors[0], actors[1], actors[2] },
                    },
                    new Movie {Title = "Luther: The Fallen Sun (2023)", Description = "Brilliant but disgraced detective John Luther breaks out of prison to hunt down a sadistic serial killer who is terrorising London.",
                        Key = "EGK5qtXuc1Q", StarRating = "5", Runtime = 129, DirectorId = 1, CompanyId = 2, RatingId = 1, RemarkId = 1, ReleaseDate = DateTime.Now,
                        Genres = new List<Genre>(){ genres[5], genres[2], genres[7] },
                        Actors = new List<Actor>(){ actors[3], actors[4], actors[5] },
                    },
                };
                Movies.AddRange(movies);
                SaveChanges();

                List<MovieImage> movieImages = new List<MovieImage>()
                {
                    new MovieImage {MovieId = 1, ImageId = 4},
                    new MovieImage {MovieId = 2, ImageId = 5},
                };
                MovieImages.AddRange(movieImages);
                SaveChanges();

                List<Faq> faqs = new List<Faq>
                {
                    new Faq { Title ="What is Netflix?", Answer = "Netflix is a streaming service that offers a wide variety of award-winning TV shows, movies, anime, documentaries, and more on thousands of internet-connected devices. \n\nYou can watch as much as you want, whenever you want without a single commercial – all for one low monthly price. There's always something new to discover and new TV shows and movies are added every week!"},
                    new Faq { Title ="How much does Netflix cost?", Answer = "Watch Netflix on your smartphone, tablet, Smart TV, laptop, or streaming device, all for one fixed monthly fee. Plans range from EUR 4.99 to EUR 9.99 a month. No extra costs, no contracts."},
                    new Faq { Title ="Where can I watch?", Answer = "Watch anywhere, anytime. Sign in with your Netflix account to watch instantly on the web at netflix.com from your personal computer or on any internet-connected device that offers the Netflix app, including smart TVs, smartphones, tablets, streaming media players and game consoles. \n\nYou can also download your favorite shows with the iOS, Android, or Windows 10 app. Use downloads to watch while you're on the go and without an internet connection. Take Netflix with you anywhere."},
                    new Faq { Title ="How do I cancel?", Answer = "Netflix is flexible. There are no pesky contracts and no commitments. You can easily cancel your account online in two clicks. There are no cancellation fees – start or stop your account anytime."},
                    new Faq { Title ="What can I watch on Netflix?", Answer = "Netflix has an extensive library of feature films, documentaries, TV shows, anime, award-winning Netflix originals, and more. Watch as much as you want, anytime you want."},
                    new Faq { Title ="Is Netflix good for kids?", Answer = "The Netflix Kids experience is included in your membership to give parents control while kids enjoy family-friendly TV shows and movies in their own space. \n\nKids profiles come with PIN-protected parental controls that let you restrict the maturity rating of content kids can watch and block specific titles you don’t want kids to see."},
                    new Faq { Title ="Why am I seeing this language?", Answer = "Your browser preferences determine the language shown here."},
                };
                Faqs.AddRange(faqs);
                SaveChanges();
            }
        }

        public DbSet<Faq> Faqs { get; set; }
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
