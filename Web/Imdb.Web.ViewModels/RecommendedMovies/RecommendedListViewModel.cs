namespace Imdb.Web.ViewModels.RecommendedMovies
{
    using System.Collections.Generic;

    public class RecommendedListViewModel
    {
        public IEnumerable<RecommendEntityViewModel> Entities { get; set; }
    }
}
