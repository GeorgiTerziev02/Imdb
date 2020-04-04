namespace Imdb.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Imdb.Data.Common.Models;

    public class TvShow : BaseDeletableModel<string>
    {
        public TvShow()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Actors = new HashSet<TvShowActor>();
            this.Reviews = new HashSet<Review>();
            this.Genres = new HashSet<TvShowGenre>();
            this.UsersWatchlists = new HashSet<UserTvShow>();
            this.Images = new HashSet<TvShowImage>();
        }

        [Required]
        [MaxLength(100)]
        public string Title { get; set; }

        [Required]
        [MaxLength(500)]
        public string Description { get; set; }

        [Range(0, 100000)]
        public int EpisodesCount { get; set; }

        [Required]
        public decimal Budget { get; set; }

        [Required]
        [Range(0, 10)]
        public decimal Rating { get; set; }

        public DateTime ReleaseDate { get; set; }

        [Required]
        public int LanguageId { get; set; }

        public Language Language { get; set; }

        [Required]
        public string DirectorId { get; set; }

        public Director Director { get; set; }

        public virtual IEnumerable<TvShowGenre> Genres { get; set; }

        public virtual IEnumerable<TvShowActor> Actors { get; set; }

        public virtual IEnumerable<Review> Reviews { get; set; }

        public virtual IEnumerable<UserTvShow> UsersWatchlists { get; set; }

        public virtual IEnumerable<TvShowImage> Images { get; set; }
    }
}
