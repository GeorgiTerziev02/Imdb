using System;
using System.Collections.Generic;
using System.Text;

namespace Imdb.Web.ViewModels.TvShows
{
    public class ListAllTvShowsViewModel
    {
        public IEnumerable<ListTvShowViewModel> TvShows { get; set; }
    }
}
