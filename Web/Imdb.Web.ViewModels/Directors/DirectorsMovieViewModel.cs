namespace Imdb.Web.ViewModels.Directors
{
    using AutoMapper;
    using Imdb.Data.Models;
    using Imdb.Services.Mapping;
    using System.Linq;

    public class DirectorsMovieViewModel : IMapFrom<Movie>, IHaveCustomMappings
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public double Rating { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Movie, DirectorsMovieViewModel>()
                .ForMember(x => x.Rating, y => y.MapFrom(x => x.Votes.Average(x => x.Rating)));
        }
    }
}
