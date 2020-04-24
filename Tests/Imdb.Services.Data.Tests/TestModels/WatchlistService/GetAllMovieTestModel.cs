namespace Imdb.Services.Data.Tests.TestModels.WatchlistService
{
    using Imdb.Data.Models;
    using Imdb.Services.Mapping;

    public class GetAllMovieTestModel : IMapFrom<UserMovie>
    {
        public string MovieTitle { get; set; }
    }
}
