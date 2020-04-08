﻿namespace Imdb.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using Imdb.Data.Common.Models;

    public class Review : BaseDeletableModel<string>
    {
        public Review()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        [Required]
        public string UserId { get; set; }

        public ApplicationUser User { get; set; }

        [Required]
        public string Content { get; set; }

        public string MovieId { get; set; }

        public Movie Movie { get; set; }
    }
}
