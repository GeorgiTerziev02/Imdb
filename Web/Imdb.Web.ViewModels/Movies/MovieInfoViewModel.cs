namespace Imdb.Web.ViewModels.Movies
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
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

        [Display(Name = "Release date")]
        public DateTime? ReleaseDate { get; set; }

        [Display(Name = "Language")]
        public string LanguageName { get; set; }

        public string DirectorId { get; set; }

        [Display(Name = "Director's name")]
        public string DirectorName { get; set; }

        [Display(Name = "Image")]
        public string GeneralImageUrl { get; set; }

        public string Trailer { get; set; }

        public double? Rating { get; set; }

        public virtual IEnumerable<MovieGenreInfoViewModel> Genres { get; set; }

        public virtual IEnumerable<MovieActorInfoViewModel> Actors { get; set; }

        public virtual IEnumerable<MovieReviewViewModel> Reviews { get; set; }

        public int VotesCount { get; set; }

        public IEnumerable<int> PossibleVotes { get; set; }

        // TODO: public virtual IEnumerable<MovieImage> MovieImages { get; set; }
        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Movie, MovieInfoViewModel>()
                .ForMember(x => x.DirectorName, y => y.MapFrom(x => x.Director.FirstName + " " + x.Director.LastName))
                .ForMember(x => x.Rating, y => y.MapFrom(x => x.Votes.Average(z => z.Rating)))
                .ForMember(x => x.VotesCount, y => y.MapFrom(x => x.Votes.Count()));
        }
    }
}
