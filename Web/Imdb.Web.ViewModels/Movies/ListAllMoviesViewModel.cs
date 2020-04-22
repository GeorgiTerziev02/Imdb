namespace Imdb.Web.ViewModels.Movies
{
    using System.Collections.Generic;

    public class ListAllMoviesViewModel
    {
        public IEnumerable<ListMovieViewModel> Movies { get; set; }

        public int PageCount { get; set; }

        public int CurrentPage { get; set; }

        public string CurrentSorting { get; set; }
    }
}
