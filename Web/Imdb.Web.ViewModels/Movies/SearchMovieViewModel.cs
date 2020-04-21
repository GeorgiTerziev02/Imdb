namespace Imdb.Web.ViewModels.Movies
{
    using Imdb.Data.Models;
    using Imdb.Services.Mapping;

    public class SearchMovieViewModel : IMapFrom<Movie>
    {
        public string GeneralImageUrl { get; set; }

        public bool IsTvShow { get; set; }

        public string Id { get; set; }

        public string Title { get; set; }
    }
}
