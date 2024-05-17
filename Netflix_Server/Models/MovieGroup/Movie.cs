using Humanizer.Localisation;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System.IO;

namespace Netflix_Server.Models.MovieGroup
{
    public class Movie
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public virtual ICollection<MovieImage>? MovieImages { get; set; }
        public string Description { get; set; }
        public int RemarkId { get; set; }

        public virtual Remark Remark { get; set; }
        public string Key { get; set; }
        public string StarRating { get; set; }
        public int RatingId { get; set; }
        public virtual Rating Rating { get; set; }
        public DateOnly? ReleaseDate { get; set; }
        public virtual ICollection<Genre> Genres { get; set; }
        public int Runtime { get; set; }
        public virtual ICollection<Actor> Actors { get; set; }
        public int DirectorId { get; set; }
        public virtual Director Director { get; set; }
        public int CompanyId { get; set; }
        public virtual Company Company { get; set; }

        public Movie()
        {
            MovieImages = new List<MovieImage>();
            Genres = new List<Genre>();
            Actors = new List<Actor>();
        }
    }
}
