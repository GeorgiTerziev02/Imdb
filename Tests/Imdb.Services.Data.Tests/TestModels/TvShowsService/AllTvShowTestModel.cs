namespace Imdb.Services.Data.Tests.TestModels.TvShowsService
{
    using Imdb.Data.Models;
    using Imdb.Services.Mapping;

    public class AllTvShowTestModel : IMapFrom<Movie>
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public bool IsTvShow { get; set; }

        public double Rating { get; set; }
    }
}
