namespace Imdb.Web.ViewModels.Directors
{
    using System.Linq;

    using AutoMapper;
    using Imdb.Data.Models;
    using Imdb.Services.Mapping;

    public class DirectorsMovieViewModel : IMapFrom<Movie>, IHaveCustomMappings
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string Rating { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Movie, DirectorsMovieViewModel>()
                .ForMember(x => x.Rating, y => y.MapFrom(x => x.Votes.Average(x => x.Rating).ToString("f1")));
        }
    }
}
