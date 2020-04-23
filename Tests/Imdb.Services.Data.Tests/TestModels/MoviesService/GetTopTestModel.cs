namespace Imdb.Services.Data.Tests.TestModels.MoviesService
{
    using Imdb.Data.Models;
    using Imdb.Services.Mapping;

    public class GetTopTestModel : IMapFrom<Movie>
    {
        public string Title { get; set; }

        public bool IsTvShow { get; set; }
    }
}
