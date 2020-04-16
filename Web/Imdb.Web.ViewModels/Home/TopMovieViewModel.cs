namespace Imdb.Web.ViewModels.Home
{
    using System.Linq;
    using System.Net;
    using System.Text.RegularExpressions;

    using AutoMapper;
    using Imdb.Data.Models;
    using Imdb.Services.Mapping;

    public class TopMovieViewModel : IMapFrom<Movie>, IHaveCustomMappings
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public double Rating { get; set; }

        public string ShortDescription
        {
            get
            {
                var content = WebUtility.HtmlDecode(Regex.Replace(this.Description, @"<[^>]+>", string.Empty));
                return content.Length > 25
                        ? content.Substring(0, 25) + "..."
                        : content;
            }
        }

        public string GeneralImageUrl { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Movie, TopMovieViewModel>()
                .ForMember(x => x.Rating, y => y.MapFrom(x => x.Votes.Average(x => x.Rating)));
        }
    }
}
