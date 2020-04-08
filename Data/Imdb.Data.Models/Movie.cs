namespace Imdb.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Imdb.Data.Common.Models;

    public class Movie : BaseDeletableModel<string>
    {
        public Movie()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Actors = new HashSet<MovieActor>();
            this.Reviews = new HashSet<Review>();
            this.Genres = new HashSet<MovieGenre>();
            this.UsersWatchlists = new HashSet<UserMovie>();
            this.MovieImages = new HashSet<MovieImage>();
            this.Votes = new HashSet<Vote>();
            this.EpisodesCount = 1;
        }

        [Required]
        [MaxLength(100)]
        public string Title { get; set; }

        [Required]
        [MaxLength(500)]
        public string Description { get; set; }

        public TimeSpan? Duration { get; set; }

        public long? Gross { get; set; }

        public decimal? Budget { get; set; }

        public DateTime? ReleaseDate { get; set; }

        public bool IsTvShow { get; set; }

        [Range(0, 100000)]
        public int? EpisodesCount { get; set; }

        [Required]
        public int LanguageId { get; set; }

        public Language Language { get; set; }

        [Required]
        public string DirectorId { get; set; }

        public Director Director { get; set; }

        public string GeneralImageUrl { get; set; }

        public string Trailer { get; set; }

        public virtual IEnumerable<MovieGenre> Genres { get; set; }

        public virtual IEnumerable<MovieActor> Actors { get; set; }

        public virtual IEnumerable<Review> Reviews { get; set; }

        public virtual IEnumerable<UserMovie> UsersWatchlists { get; set; }

        public virtual IEnumerable<MovieImage> MovieImages { get; set; }

        public virtual IEnumerable<Vote> Votes { get; set; }
    }
}
