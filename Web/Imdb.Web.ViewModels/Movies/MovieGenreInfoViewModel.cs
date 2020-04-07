namespace Imdb.Web.ViewModels.Movies
{
    using Imdb.Data.Models;
    using Imdb.Services.Mapping;

    public class MovieGenreInfoViewModel : IMapFrom<MovieGenre>
    {
        public string GenreId { get; set; }

        public string GenreName { get; set; }
    }
}
