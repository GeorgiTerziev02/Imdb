using System;
using System.ComponentModel.DataAnnotations;

namespace Imdb.Web.ViewModels.Admin.Administration
{
    public class AddMovieInputViewModel
    {
        [Required]
        [MaxLength(100)]
        public string Title { get; set; }

        [Required]
        [MaxLength(500)]
        public string Description { get; set; }

        [Required]
        public TimeSpan Duration { get; set; }

        public long Gross { get; set; }

        [Required]
        public decimal Budget { get; set; }

        [Required]
        [Range(0, 10)]
        public decimal Rating { get; set; }

        public DateTime ReleaseDate { get; set; }

        [Required]
        public int LanguageId { get; set; }

        [Required]
        public string DirectorId { get; set; }

        //public Director Director { get; set; }
    }
}
