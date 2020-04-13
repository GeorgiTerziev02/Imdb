using System;
using System.Collections.Generic;
using System.Text;

namespace Imdb.Web.ViewModels.Search
{
    public class SearchListViewModel
    {
        public IEnumerable<SearchMovieViewModel> Results { get; set; }
    }
}
