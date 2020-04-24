namespace Imdb.Web.ViewModels.Movies
{
    using System.Collections.Generic;

    public class ListMoviesViewModel
    {
        public string GenreName { get; set; }

        public IEnumerable<MovieByGenreViewModel> Movies { get; set; }
    }
}
