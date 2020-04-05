using Imdb.Data.Common.Models;
using Imdb.Data.Models.Enumerations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Imdb.Data.Models
{
    public class Director : BaseDeletableModel<string>
    {
        public Director()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Movies = new HashSet<Movie>();
            this.TvShows = new HashSet<TvShow>();
        }

        [Required]
        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        [Required]
        public string LastName { get; set; }

        public Gender Gender { get; set; }

        public DateTime? Born { get; set; }

        public string ImageUrl { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        public virtual IEnumerable<Movie> Movies { get; set; }

        public virtual IEnumerable<TvShow> TvShows { get; set; }
    }
}