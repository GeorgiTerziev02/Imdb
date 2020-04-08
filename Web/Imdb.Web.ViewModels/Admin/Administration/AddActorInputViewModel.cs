﻿namespace Imdb.Web.ViewModels.Admin.Administration
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using Imdb.Data.Models;
    using Imdb.Data.Models.Enumerations;
    using Imdb.Services.Mapping;
    using Microsoft.AspNetCore.Http;

    public class AddActorInputViewModel : IMapTo<Actor>
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public Gender Gender { get; set; }

        [DataType(DataType.Date)]
        public DateTime? Born { get; set; }

        public IFormFile Image { get; set; }

        public string ImageUrl { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }
    }
}
