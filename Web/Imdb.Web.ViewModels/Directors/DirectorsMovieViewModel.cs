namespace Imdb.Web.ViewModels.Directors
{
    using Imdb.Data.Models;
    using Imdb.Services.Mapping;

    public class DirectorsMovieViewModel : IMapFrom<Movie>
    {
        public string Id { get; set; }

        public string Title { get; set; }
    }
}
