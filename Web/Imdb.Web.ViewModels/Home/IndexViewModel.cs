using System.Collections.Generic;

namespace Imdb.Web.ViewModels.Home
{
    public class IndexViewModel
    {
        public IEnumerable<TopMovieViewModel> TopMovies { get; set; }
    }
}
