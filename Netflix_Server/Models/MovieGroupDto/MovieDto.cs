namespace Netflix_Server.Models.MovieGroupDto
{
    public class MovieDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public ICollection<ImageDto>? Images { get; set; }
        public string Description { get; set; }
        public int RemarkId { get; set; }

     
        public RemarkDto? Remark { get; set; }
        public string Key { get; set; }
        public string StarRating { get; set; }
        public int RatingId { get; set; }
        public RatingDto? Rating { get; set; }
        public string ReleaseDate { get; set; }
        public ICollection<int> GenreIds { get; set; }
        public ICollection<GenreDto>? Genres { get; set; }
        public int Runtime { get; set; }
        public ICollection<int> ActorIds { get; set; }
        public ICollection<ActorDto>? Actors { get; set; }
        public int DirectorId { get; set; }
        public DirectorDto? Director { get; set; }
        public int CompanyId { get; set; }
        public CompanyDto? Company { get; set; }
    }
}
