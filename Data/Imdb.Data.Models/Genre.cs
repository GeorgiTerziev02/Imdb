using Imdb.Data.Common.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Imdb.Data.Models
{
    public class Genre : BaseModel<string>
    {
        public Genre()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Movies = new HashSet<MovieGenre>();
            this.TvShows = new HashSet<TvShowGenre>();
        }

        [Required]
        public string Name { get; set; }

        public virtual IEnumerable<MovieGenre> Movies { get; set; }

        public virtual IEnumerable<TvShowGenre> TvShows { get; set; }
    }
}
