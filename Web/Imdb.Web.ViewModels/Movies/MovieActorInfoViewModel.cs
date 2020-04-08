namespace Imdb.Web.ViewModels.Movies
{
    using AutoMapper;
    using Imdb.Data.Models;
    using Imdb.Services.Mapping;

    public class MovieActorInfoViewModel : IMapFrom<MovieActor>, IHaveCustomMappings
    {
        public string ActorId { get; set; }

        public string ActorName { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<MovieActor, MovieActorInfoViewModel>()
                .ForMember(x => x.ActorName, y => y.MapFrom(x => x.Actor.FirstName + x.Actor.LastName));
        }
    }
}
