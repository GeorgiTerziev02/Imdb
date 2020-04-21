namespace Imdb.Services.Data.Tests.TestModels.GenresServie
{
    using Imdb.Data.Models;
    using Imdb.Services.Mapping;

    public class GenreAllTestModel : IMapFrom<Genre>
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
