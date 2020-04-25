namespace Imdb.Web.ViewModels.Admin.Administration
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using Imdb.Data.Models;
    using Imdb.Services.Mapping;

    public class EditMovieInputViewModel : IMapFrom<Movie>
    {
        [Required]
        public string Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Title { get; set; }

        [Required]
        [MaxLength(500)]
        public string Description { get; set; }

        [RegularExpression("^([0-1]?[0-9]|2[0-4]):([0-5][0-9])(:[0-5][0-9])?$", ErrorMessage = "Duration must be in format hh:mm/hh:mm:ss")]
        public string Duration { get; set; }

        public long? Gross { get; set; }

        public decimal? Budget { get; set; }

        [DataType(DataType.Date)]
        public DateTime? ReleaseDate { get; set; }

        [Display(Name = "Trailer url")]
        public string Trailer { get; set; }

        [Required]
        [Display(Name = "Language")]
        public int LanguageId { get; set; }

        [Required]
        [Display(Name = "Director")]
        public string DirectorId { get; set; }

        [NotMapped]
        public IEnumerable<DirectorDropDownViewModel> Directors { get; set; }

        [NotMapped]
        public IEnumerable<LanguageDropDownViewModel> Languages { get; set; }
    }
}
