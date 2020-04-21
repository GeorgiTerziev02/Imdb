namespace Imdb.Web.ViewModels.RecommendedMovies
{
    using System;
    using System.Linq;

    using AutoMapper;
    using Imdb.Data.Models;
    using Imdb.Services.Mapping;

    public class RecommendEntityViewModel : IMapFrom<Movie>, IHaveCustomMappings
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string Rating { get; set; }

        public DateTime? ReleaseDate { get; set; }

        public bool IsTvShow { get; set; }

        public string LanguageName { get; set; }

        public string DirectorName { get; set; }

        public string GeneralImageUrl { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Movie, RecommendEntityViewModel>()
                .ForMember(
                    x => x.Rating,
                    y => y.MapFrom(x => x.Votes.Average(z => z.Rating).ToString("f1")));
        }
    }
}
