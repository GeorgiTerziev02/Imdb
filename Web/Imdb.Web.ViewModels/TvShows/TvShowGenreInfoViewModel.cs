namespace Imdb.Web.ViewModels.TvShows
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using Imdb.Data.Models;
    using Imdb.Services.Mapping;

    public class TvShowGenreInfoViewModel : IMapFrom<MovieGenre>
    {
        public string GenreId { get; set; }

        public string GenreName { get; set; }
    }
}
