namespace Imdb.Web.ViewModels.Genres
{
    using System.ComponentModel.DataAnnotations;

    public class RemoveGenreViewModel
    {
        [Required]
        public int Id { get; set; }
    }
}
