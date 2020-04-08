namespace Imdb.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text;

    using Imdb.Data.Common.Models;

    public class Vote : BaseModel<int>
    {
        [Required]
        [Range(0, 10)]
        public int Rating { get; set; }

        [Required]
        public string UserId { get; set; }

        public ApplicationUser User { get; set; }

        public string MovieId { get; set; }

        public Movie Movie { get; set; }
    }
}
