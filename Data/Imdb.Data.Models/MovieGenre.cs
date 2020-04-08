namespace Imdb.Data.Models
{
    using System;

    using Imdb.Data.Common.Models;

    public class MovieGenre : BaseModel<int>
    {
        public string MovieId { get; set; }

        public Movie Movie { get; set; }

        public int GenreId { get; set; }

        public Genre Genre { get; set; }
    }
}
