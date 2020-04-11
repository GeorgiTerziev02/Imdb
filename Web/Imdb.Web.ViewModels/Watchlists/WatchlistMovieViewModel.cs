namespace Imdb.Web.ViewModels.Watchlists
{
    using System;
    using System.Linq;

    using AutoMapper;

    using Imdb.Data.Models;
    using Imdb.Services.Mapping;

    public class WatchlistMovieViewModel : IMapFrom<UserMovie>, IHaveCustomMappings
    {
        public string MovieId { get; set; }

        public string MovieTitle { get; set; }

        public DateTime? MovieReleaseDate { get; set; }

        public string MovieGeneralImageUrl { get; set; }

        public double? Rating { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<UserMovie, WatchlistMovieViewModel>()
                .ForMember(x => x.Rating, y => y.MapFrom(x => x.Movie.Votes.Average(z => z.Rating)));
        }
    }
}
