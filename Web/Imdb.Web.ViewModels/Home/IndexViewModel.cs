namespace Imdb.Web.ViewModels.Home
{
    using System.Collections.Generic;

    public class IndexViewModel
    {
        public IEnumerable<TopMovieViewModel> TopMovies { get; set; }

        public IEnumerable<BornTodayActorViewModel> BornToday { get; set; }
    }
}
