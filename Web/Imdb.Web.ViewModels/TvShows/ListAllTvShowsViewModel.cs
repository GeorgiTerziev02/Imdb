namespace Imdb.Web.ViewModels.TvShows
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class ListAllTvShowsViewModel
    {
        public IEnumerable<ListTvShowViewModel> TvShows { get; set; }
    }
}
