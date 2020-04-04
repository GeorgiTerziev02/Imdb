using Imdb.Data.Common.Models;
using Imdb.Data.Models.Enumerations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Imdb.Data.Models
{
    public class Actor : BaseDeletableModel<string>
    {

        public Actor()
        {
            this.Id = Guid.NewGuid().ToString();
            this.TvShows = new HashSet<TvShowActor>();
            this.Movies = new HashSet<MovieActor>();
        }

        [Required]
        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        [Required]
        public string LastName { get; set; }

        public Gender Gender { get; set; }

        public DateTime? Born { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        public virtual IEnumerable<TvShowActor> TvShows { get; set; }

        public virtual IEnumerable<MovieActor> Movies { get; set; }
    }
}