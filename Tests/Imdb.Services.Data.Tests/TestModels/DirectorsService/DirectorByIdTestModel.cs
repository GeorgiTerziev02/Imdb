namespace Imdb.Services.Data.Tests.TestModels.DirectorsService
{
    using Imdb.Data.Models;
    using Imdb.Services.Mapping;

    public class DirectorByIdTestModel : IMapFrom<Director>
    {
        public string Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}
