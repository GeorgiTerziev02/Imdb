namespace Imdb.Web.ViewModels.Actors
{
    using System.Linq;

    using AutoMapper;
    using Imdb.Data.Models;
    using Imdb.Services.Mapping;

    public class MovieOfActorViewModel : IMapFrom<MovieActor>, IHaveCustomMappings
    {
        public string MovieId { get; set; }

        public string MovieTitle { get; set; }

        public double MovieRating { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<MovieActor, MovieOfActorViewModel>()
                .ForMember(x => x.MovieRating, y => y.MapFrom(x => x.Movie.Votes.Average(z => z.Rating)));
        }
    }
}
