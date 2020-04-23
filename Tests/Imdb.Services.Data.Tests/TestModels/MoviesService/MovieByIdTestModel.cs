namespace Imdb.Services.Data.Tests.TestModels.MoviesService
{
    using Imdb.Data.Models;
    using Imdb.Services.Mapping;

    public class MovieByIdTestModel : IMapFrom<Movie>
    {
        public string Id { get; set; }
    }
}
