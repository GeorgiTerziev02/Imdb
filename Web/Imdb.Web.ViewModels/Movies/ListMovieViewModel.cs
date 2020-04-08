namespace Imdb.Web.ViewModels.Movies
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using AutoMapper;
    using Imdb.Data.Models;
    using Imdb.Services.Mapping;

    public class ListMovieViewModel : IMapFrom<Movie>, IHaveCustomMappings
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public DateTime? ReleaseDate { get; set; }

        public string GeneralImageUrl { get; set; }

        public double? Rating { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Movie, ListMovieViewModel>()
                .ForMember(x => x.Rating, y => y.MapFrom(x => x.Votes.Average(z => z.Rating)));
        }
    }
}
