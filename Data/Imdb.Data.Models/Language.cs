using Imdb.Data.Common.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Imdb.Data.Models
{
    public class Language
    {
        public Language()
        {
            this.Movies = new HashSet<Movie>();
            this.TvShows = new HashSet<TvShow>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public virtual IEnumerable<Movie> Movies { get; set; }

        public virtual IEnumerable<TvShow> TvShows { get; set; }
    }
}