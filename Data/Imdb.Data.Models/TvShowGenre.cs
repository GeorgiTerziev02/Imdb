using Imdb.Data.Common.Models;
using System;

namespace Imdb.Data.Models
{
    public class TvShowGenre : BaseModel<string>
    {
        public TvShowGenre()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        public string TvShowId { get; set; }

        public TvShow TvShow { get; set; }

        public string GenreId { get; set; }

        public Genre Genre { get; set; }
    }
}