namespace Imdb.Web.ViewModels.Movies
{
    using System.Collections.Generic;

    public class ListAllMoviesViewModel
    {
        public IEnumerable<ListMovieViewModel> Movies { get; set; }
    }
}
