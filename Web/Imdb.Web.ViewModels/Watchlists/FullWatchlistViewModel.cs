namespace Imdb.Web.ViewModels.Watchlists
{
    using System.Collections.Generic;

    public class FullWatchlistViewModel
    {
        public IEnumerable<WatchlistEntityViewModel> Movies { get; set; }

        public string Id { get; set; }

        public int CurrentPage { get; set; }

        public int PageCount { get; set; }

        public string CurrentSorting { get; set; }
    }
}
