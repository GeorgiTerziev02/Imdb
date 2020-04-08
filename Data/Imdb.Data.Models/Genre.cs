namespace Imdb.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Imdb.Data.Common.Models;

    public class Genre : BaseModel<int>
    {
        public Genre()
        {
            this.Movies = new HashSet<MovieGenre>();
        }

        [Required]
        public string Name { get; set; }

        public virtual IEnumerable<MovieGenre> Movies { get; set; }
    }
}
