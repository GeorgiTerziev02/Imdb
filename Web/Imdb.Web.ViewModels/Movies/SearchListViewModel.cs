namespace Imdb.Web.ViewModels.Movies
{
    using System.Collections.Generic;

    public class SearchListViewModel
    {
        public IEnumerable<SearchMovieViewModel> Results { get; set; }
    }
}
