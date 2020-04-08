namespace Imdb.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Imdb.Data.Common.Models;
    using Imdb.Data.Models.Enumerations;

    public class Actor : BaseDeletableModel<string>
    {
        public Actor()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Movies = new HashSet<MovieActor>();
        }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public Gender Gender { get; set; }

        public DateTime? Born { get; set; }

        public string ImageUrl { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        public virtual IEnumerable<MovieActor> Movies { get; set; }
    }
}
