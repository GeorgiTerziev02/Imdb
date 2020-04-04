﻿namespace Imdb.Data.Models
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
        }

        [Required]
        [MaxLength(100)]
        public string Title { get; set; }

        [Required]
        [MaxLength(500)]
        public string Description { get; set; }

        [Required]
        public TimeSpan Duration { get; set; }

        public long Gross { get; set; }

        [Required]
        public decimal Budget { get; set; }

        [Required]
        [Range(0, 10)]
        public decimal Rating { get; set; }

        public DateTime ReleaseDate { get; set; }

        [Required]
        public string LanguageId { get; set; }

        public Language Language { get; set; }

        [Required]
        public string DirectorId { get; set; }

        public Director Director { get; set; }

        public virtual IEnumerable<MovieGenre> Genres { get; set; }

        public virtual IEnumerable<MovieActor> Actors { get; set; }

        public virtual IEnumerable<Review> Reviews { get; set; }

        public virtual IEnumerable<UserMovie> UsersWatchlists { get; set; }
    }
}
