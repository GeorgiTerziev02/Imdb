namespace Imdb.Web.ViewModels.Watchlists
{
    using System.Collections.Generic;

    public class FullWatchlistViewModel
    {
        public IEnumerable<WatchlistMovieViewModel> Movies { get; set; }
    }
}
