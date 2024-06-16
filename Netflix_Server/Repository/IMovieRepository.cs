using AutoMapper;
using Humanizer;
using Microsoft.EntityFrameworkCore;
using Netflix_Server.Models.Context;
using Netflix_Server.Models.MovieGroup;
using Netflix_Server.Models.MovieGroupDto;

namespace Netflix_Server.Repository
{
    public interface IMovieRepository
    {
        Task<MovieDto> AddMovie(MovieDto movie);
        Task<bool> RemoveMovieById(int id);
        Task<ICollection<MovieDto>> GetMovies();
        Task<ICollection<MovieDto>> GetMovies(FilterMovieDto filter);
        Task<MovieDto> GetMovieById(int id);
        Task<MovieDto> UpdateMovie(MovieDto movie);
    }
    public class MovieRepository : IMovieRepository
    {
        private readonly MovieContext _context;
        private readonly IMapper _mapper;
        private readonly IImageRepository _imageRep;

        public MovieRepository(MovieContext context, IMapper mapper, IImageRepository imageRep)
        {
            _context = context;
            _mapper = mapper;
            _imageRep = imageRep;
        }
        private async Task<Movie> ConvertFromDto(MovieDto dto)
        {
            var movie = _mapper.Map<Movie>(dto);
            movie.Id = default;

            if (dto.GenreIds != null && dto.GenreIds.Count > 0)
            {
                var genres = await _context.Genres.Where(g => dto.GenreIds.Contains(g.Id)).ToListAsync();
                movie.Genres = genres;
            }

            if (dto.ActorIds != null && dto.ActorIds.Count > 0)
            {
                var actors = await _context.Actors.Where(a => dto.ActorIds.Contains(a.Id)).ToListAsync();
                movie.Actors = actors;
            }

            movie.Remark = await _context.Remarks.FindAsync(dto.RemarkId) ?? throw new Exception("Remark id is not exists");
            movie.Rating = await _context.Ratings.FindAsync(dto.RatingId) ?? throw new Exception("Rating id is not exists");
            movie.Director = await _context.Directors.FindAsync(dto.DirectorId) ?? throw new Exception("Director id is not exists");
            movie.Company = await _context.Companies.FindAsync(dto.CompanyId) ?? throw new Exception("Company id is not exists");

            return movie;
        }
        public async Task<MovieDto> AddMovie(MovieDto dto)
        {
            var movie = await ConvertFromDto(dto);
            movie.MovieImages?.Clear();
            await _context.Movies.AddAsync(movie);
            await _context.SaveChangesAsync();

            if (dto.Images != null)
            {
                var images = await _imageRep.SaveNewImagesFromDto(dto);
                var movieImages = images.Select(i => new MovieImage { ImageId = i.Id, MovieId = movie.Id }).ToList();
                _context.MovieImages.AddRange(movieImages);
                await _context.SaveChangesAsync();
                movie.MovieImages = movieImages;
            }

            return _mapper.Map<MovieDto>(movie);
        }

        public async Task<MovieDto> GetMovieById(int id)
        {
            var movie = await _context.Movies
                .Include(m => m.MovieImages).ThenInclude(mi => mi.Image)
                .Include(m => m.Remark)
                .Include(m => m.Rating)
                .Include(m => m.Genres)
                .Include(m => m.Actors).ThenInclude(a => a.ActorImages).ThenInclude(ai => ai.Image)
                .Include(m => m.Director).ThenInclude(d => d.DirectorImages).ThenInclude(di => di.Image)
                .Include(m => m.Company).ThenInclude(c => c.CompanyImages).ThenInclude(ci => ci.Image)
                .FirstOrDefaultAsync(m => m.Id == id);

            return _mapper.Map<MovieDto>(movie);
        }
        public async Task<ICollection<MovieDto>> GetMovies()
        {
            var movies = await _context.Movies
                .Include(m => m.MovieImages).ThenInclude(mi => mi.Image)
                .Include(m => m.Remark)
                .Include(m => m.Rating)
                .Include(m => m.Genres)
                .Include(m => m.Actors).ThenInclude(a => a.ActorImages).ThenInclude(ai => ai.Image)
                .Include(m => m.Director).ThenInclude(d => d.DirectorImages).ThenInclude(di => di.Image)
                .Include(m => m.Company).ThenInclude(c => c.CompanyImages).ThenInclude(ci => ci.Image)
                .ToListAsync();

            return _mapper.Map<List<MovieDto>>(movies);
        }
        public async Task<ICollection<MovieDto>> GetMovies(FilterMovieDto filter)
        {
            var query = _context.Movies
                        .Include(m => m.Genres)
                        .Where(m => m.Title.ToLower().Contains(filter.SearchSubstring!.ToLower() ?? string.Empty));
            if (query.Count() == 0) return [];

            if (filter.GenreIds?.Any() ?? false)
            {
                query = query.Where(m => filter.GenreIds.Any(genreId => m.Genres.Select(g => g.Id).Contains(genreId)));
            }
            if (query.Count() == 0) return [];

            query?.OrderBy(m => m.Id)
                 .Skip((filter.PageIndex - 1) * filter.PageSize)
                 .Take(filter.PageSize)
                 .ToListAsync();

            if (query.Count() == 0) return [];
            var movies = await query
                .Include(m => m.MovieImages!)
                    .ThenInclude(a => a.Image)
                .Include(m => m.Remark)
                .Include(m => m.Rating)
                .Include(m => m.Genres)
                .Include(m => m.Actors)
                    .ThenInclude(a => a.ActorImages)
                        .ThenInclude(a => a.Image)
                .Include(m => m.Director)
                    .ThenInclude(a => a.DirectorImages)
                        .ThenInclude(a => a.Image)
                .Include(m => m.Company)
                    .ThenInclude(a => a.CompanyImages)
                        .ThenInclude(a => a.Image)
                 .ToListAsync();
            return movies.Select(m => _mapper.Map<MovieDto>(m)).ToList();
        }
        public async Task<bool> RemoveMovieById(int id)
        {
            var movie = await _context.Movies
                .Include(m => m.MovieImages)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (movie == null)
            {
                return false;
            }

            _context.Movies.Remove(movie);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<MovieDto> UpdateMovie(MovieDto dto)
        {
            // Find the existing movie in the database
            var existingMovie = await _context.Movies
                .Include(m => m.Genres)
                .Include(m => m.Actors)
                .Include(m => m.MovieImages)
                .FirstOrDefaultAsync(m => m.Id == dto.Id);

            if (existingMovie == null)
            {
                // Movie not found, return null 
                return null;
            }

            // Update properties of the existing movie with values from the DTO
            existingMovie.Title = dto.Title;
            existingMovie.Description = dto.Description;
            existingMovie.Key = dto.Key;
            existingMovie.StarRating = dto.StarRating;
            existingMovie.ReleaseDate = DateTime.Parse(dto.ReleaseDate);
            existingMovie.Runtime = dto.Runtime;

            // Update genres
            if (dto.GenreIds != null)
            {
                var existingGenreIds = existingMovie.Genres.Select(g => g.Id).ToList();
                var genresToAdd = dto.GenreIds.Where(id => !existingGenreIds.Contains(id)).ToList();
                var genresToRemove = existingMovie.Genres.Where(g => !dto.GenreIds.Contains(g.Id)).ToList();

                if (genresToRemove.Any())
                {
                    foreach (var genre in genresToRemove)
                    {
                        existingMovie.Genres.Remove(genre);
                    }
                }
                if (genresToAdd.Any())
                {
                    foreach (var genreId in genresToAdd)
                    {
                        var genre = await _context.Genres.FindAsync(genreId);
                        if (genre != null)
                        {
                            existingMovie.Genres.Add(genre);
                        }
                    }
                }
            }

            // Update actors
            if (dto.ActorIds != null)
            {
                var existingActorIds = existingMovie.Actors.Select(a => a.Id).ToList();
                var actorsToAdd = dto.ActorIds.Where(id => !existingActorIds.Contains(id)).ToList();
                var actorsToRemove = existingMovie.Actors.Where(a => !dto.ActorIds.Contains(a.Id)).ToList();

                if (actorsToRemove.Any())
                {
                    foreach (var actor in actorsToRemove)
                    {
                        existingMovie.Actors.Remove(actor);
                    }
                }
                if (actorsToAdd.Any())
                {
                    foreach (var actorId in actorsToAdd)
                    {
                        var actor = await _context.Actors.FindAsync(actorId);
                        if (actor != null)
                        {
                            existingMovie.Actors.Add(actor);
                        }
                    }
                }
            }

            // Update remark
            if (existingMovie.RemarkId != dto.RemarkId)
            {
                var remark = await _context.Remarks.FindAsync(dto.RemarkId);
                if (remark != null)
                {
                    existingMovie.Remark = remark;
                }
            }

            // Update rating
            if (existingMovie.RatingId != dto.RatingId)
            {
                var rating = await _context.Ratings.FindAsync(dto.RatingId);
                if (rating != null)
                {
                    existingMovie.Rating = rating;
                }
            }

            // Update director
            if (existingMovie.DirectorId != dto.DirectorId)
            {
                var director = await _context.Directors.FindAsync(dto.DirectorId);
                if (director != null)
                {
                    existingMovie.Director = director;
                }
            }

            // Update company
            if (existingMovie.CompanyId != dto.CompanyId)
            {
                var company = await _context.Companies.FindAsync(dto.CompanyId);
                if (company != null)
                {
                    existingMovie.Company = company;
                }
            }

            // Update images
            if (dto.Images?.Any() == true)
            {
                var imagesToReplace = dto.Images.Where(i => i.Id != null);
                foreach (var image in imagesToReplace)
                {
                    var img = await _context.Images.FirstAsync(x => x.Id == image.Id);
                    if (img != null)
                    {
                        img.Alt = image.Alt;
                        img.ImageUrl = image.ImageUrl;
                    }
                }
            }

            // Save changes to the database
            await _context.SaveChangesAsync();

            // Map the updated movie to MovieDto and return
            return _mapper.Map<MovieDto>(existingMovie);
        }
    }
}
