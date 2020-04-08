namespace Imdb.Web.ViewModels.TvShows
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;
    using Imdb.Data.Models;
    using Imdb.Services.Mapping;

    public class TvShowInfoViewModel : IMapFrom<Movie>, IHaveCustomMappings
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public int? EpisodesCount { get; set; }

        public decimal? Budget { get; set; }

        public DateTime? ReleaseDate { get; set; }

        public string LanguageName { get; set; }

        public string DirectorName { get; set; }

        public Director Director { get; set; }

        public string GeneralImageUrl { get; set; }

        public double? Rating { get; set; }

        public virtual IEnumerable<TvShowGenreInfoViewModel> Genres { get; set; }

        public virtual IEnumerable<TvShowActorInfoViewModel> Actors { get; set; }

        public virtual IEnumerable<TvShowReviewInfoViewModel> Reviews { get; set; }

        // TODO:
        // public virtual IEnumerable<TvShowImage> Images { get; set; }
        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Movie, TvShowInfoViewModel>()
                .ForMember(x => x.DirectorName, y => y.MapFrom(x => x.Director.FirstName + x.Director.LastName))
                .ForMember(x => x.Rating, y => y.MapFrom(x => x.Votes.Average(z => z.Rating)));
        }
    }
}
