namespace Imdb.Web.ViewModels.Directors
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Imdb.Data.Models;
    using Imdb.Data.Models.Enumerations;
    using Imdb.Services.Mapping;

    public class DirectorInfoViewModel : IMapFrom<Director>
    {
        public string Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public Gender Gender { get; set; }

        public DateTime? Born { get; set; }

        public string ImageUrl { get; set; }

        public string Description { get; set; }

        [Display(Name = "Movies and TvShows")]
        public virtual IEnumerable<DirectorsMovieViewModel> Movies { get; set; }
    }
}
