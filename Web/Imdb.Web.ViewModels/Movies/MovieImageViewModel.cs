namespace Imdb.Web.ViewModels.Movies
{
    using Imdb.Data.Models;
    using Imdb.Services.Mapping;

    public class MovieImageViewModel : IMapFrom<MovieImage>
    {
        public string ImageUrl { get; set; }
    }
}
