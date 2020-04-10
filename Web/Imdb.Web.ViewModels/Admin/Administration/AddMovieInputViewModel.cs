namespace Imdb.Web.ViewModels.Admin.Administration
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using Imdb.Data.Models;
    using Imdb.Services.Mapping;
    using Microsoft.AspNetCore.Http;

    public class AddMovieInputViewModel : IMapTo<Movie>
    {
        [Required]
        [MaxLength(100)]
        public string Title { get; set; }

        [Required]
        [MaxLength(500)]
        public string Description { get; set; }

        public TimeSpan? Duration { get; set; }

        public long? Gross { get; set; }

        public decimal? Budget { get; set; }

        public DateTime ReleaseDate { get; set; }

        public IFormFile Image { get; set; }

        public string GeneralImageUrl { get; set; }

        public string TrailerUrl { get; set; }

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
