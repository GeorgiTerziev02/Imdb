namespace Imdb.Web.ViewModels.Movies
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;
    using Imdb.Data.Models;
    using Imdb.Services.Mapping;

    public class MovieInfoViewModel : IMapFrom<Movie>, IHaveCustomMappings
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public TimeSpan? Duration { get; set; }

        public long? Gross { get; set; }

        public decimal? Budget { get; set; }

        public DateTime? ReleaseDate { get; set; }

        public string LanguageName { get; set; }

        public string DirectorId { get; set; }

        public string DirectorName { get; set; }

        public string GeneralImageUrl { get; set; }

        public double? Rating { get; set; }

        public virtual IEnumerable<MovieGenreInfoViewModel> Genres { get; set; }

        public virtual IEnumerable<MovieActorsInfoViewModel> Actors { get; set; }

        public virtual IEnumerable<MovieReviewViewModel> Reviews { get; set; }

        // TODO: public virtual IEnumerable<MovieImage> MovieImages { get; set; }
        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Movie, MovieInfoViewModel>()
                .ForMember(x => x.DirectorName, y => y.MapFrom(x => x.Director.FirstName + x.Director.LastName))
                .ForMember(x => x.Rating, y => y.MapFrom(x => x.Votes.Average(z => z.Rating)));
        }
    }
}
