﻿namespace Netflix_Server.Models.MovieGroup
{
    public class MovieImage
    {
        public int MovieId { get; set; }
        public virtual Movie Movie { get; set; }

        public int ImageId { get; set; }
        public virtual Image Image { get; set; }
    }
}
