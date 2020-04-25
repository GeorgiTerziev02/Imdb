namespace Imdb.Web.ViewModels.Genres
{
    using System.ComponentModel.DataAnnotations;

    public class GenreMovieInputModel
    {
        [Required]
        public int GenreId { get; set; }

        [Required]
        public string MovieId { get; set; }
    }
}
