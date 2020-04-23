namespace Imdb.Services.Data.Tests.TestModels.MoviesService
{
    using Imdb.Data.Models;
    using Imdb.Services.Mapping;

    public class GetAllMovieTestModel : IMapFrom<Movie>
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public bool IsTvShow { get; set; }
    }
}
