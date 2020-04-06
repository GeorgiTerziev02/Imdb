using Imdb.Data.Models;
using Imdb.Services.Mapping;
using System;
using System.Collections.Generic;
using System.Text;

namespace Imdb.Web.ViewModels.Movies
{
    public class MovieGenreInfoViewModel : IMapFrom<MovieGenre>
    {
        public string GenreId { get; set; }

        public string GenreName { get; set; }
    }
}
