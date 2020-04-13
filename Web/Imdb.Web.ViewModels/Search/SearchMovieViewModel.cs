namespace Imdb.Web.ViewModels.Search
{
    using Imdb.Data.Models;
    using Imdb.Services.Mapping;

    public class SearchMovieViewModel : IMapFrom<Movie>
    {
        public string Id { get; set; }

        public string Title { get; set; }
    }
}
