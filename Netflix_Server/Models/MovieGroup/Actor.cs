﻿namespace Netflix_Server.Models.MovieGroup
{
    public class Actor
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<ActorImage> ActorImages { get; set; }
        public virtual ICollection<Movie> Movies { get; set; }
    }
}
