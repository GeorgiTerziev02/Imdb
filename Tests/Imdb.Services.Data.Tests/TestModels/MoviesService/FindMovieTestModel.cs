namespace Imdb.Services.Data.Tests.TestModels.MoviesService
{
    using Imdb.Data.Models;
    using Imdb.Services.Mapping;

    public class FindMovieTestModel : IMapFrom<Movie>
    {
        public string Title { get; set; }
    }
}
