namespace Imdb.Web.ViewModels.Admin.Administration
{
    using System.ComponentModel.DataAnnotations;

    public class OmdbSearchViewModel
    {
        [Required]
        public string Title { get; set; }

        [Required]
        [Range(1840, 2100)]
        public int Year { get; set; }
    }
}
