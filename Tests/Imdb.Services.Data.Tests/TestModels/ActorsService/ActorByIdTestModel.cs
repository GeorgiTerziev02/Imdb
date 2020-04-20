namespace Imdb.Services.Data.Tests.TestModels.ActorsService
{
    using Imdb.Data.Models;
    using Imdb.Services.Mapping;

    public class ActorByIdTestModel : IMapFrom<Actor>
    {
        public string Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}
