namespace Imdb.Services.Data.Tests.TestModels.ActorsService
{
    using Imdb.Data.Models;
    using Imdb.Services.Mapping;

    public class AllActorViewModel : IMapFrom<Actor>
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}
