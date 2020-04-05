using System.Collections.Generic;

namespace Imdb.Web.ViewModels.Home
{
    public class IndexViewModel
    {
        public string Name { get; set; }

        public IEnumerable<TopMovieViewModel> TopMovies { get; set; }
    }
}
