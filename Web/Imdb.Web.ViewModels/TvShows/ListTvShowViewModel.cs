namespace Imdb.Web.ViewModels.TvShows
{
    using System.Linq;

    using AutoMapper;
    using Imdb.Data.Models;
    using Imdb.Services.Mapping;

    public class ListTvShowViewModel : IMapFrom<Movie>, IHaveCustomMappings
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public int? EpisodesCount { get; set; }

        public string GeneralImageUrl { get; set; }

        public string Rating { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Movie, ListTvShowViewModel>()
                .ForMember(x => x.Rating, y => y.MapFrom(x => x.Votes.Average(z => z.Rating).ToString("f1")));
        }
    }
}
