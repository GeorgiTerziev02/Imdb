using Imdb.Data.Common.Models;
using System;

namespace Imdb.Data.Models
{
    public class MovieGenre : BaseModel<string>
    {
        public MovieGenre()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        public string MovieId { get; set; }

        public Movie Movie { get; set; }

        public string GenreId { get; set; }

        public Genre Genre { get; set; }
    }
}