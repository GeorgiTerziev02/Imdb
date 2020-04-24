namespace Imdb.Services.Data.Tests.TestModels.WatchlistService
{
    using Imdb.Data.Models;
    using Imdb.Services.Mapping;

    public class RecommendMovieTestModel : IMapFrom<Movie>
    {
        public string Title { get; set; }
    }
}
