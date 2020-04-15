namespace Imdb.Web.ViewModels.Actors
{
    using Imdb.Data.Models;
    using Imdb.Services.Mapping;

    public class MovieOfActorViewModel : IMapFrom<MovieActor>
    {
        public string MovieId { get; set; }

        public string MovieTitle { get; set; }
    }
}
