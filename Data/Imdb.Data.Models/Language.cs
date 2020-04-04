using Imdb.Data.Common.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Imdb.Data.Models
{
    public class Language : BaseModel<string>
    {
        public Language()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Movies = new HashSet<Movie>();
            this.TvShows = new HashSet<TvShow>();
        }

        [Required]
        public string Name { get; set; }

        public virtual IEnumerable<Movie> Movies { get; set; }

        public virtual IEnumerable<TvShow> TvShows { get; set; }
    }
}