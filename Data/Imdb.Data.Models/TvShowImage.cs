using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Imdb.Data.Models
{
    public class TvShowImage
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string ImageUrl { get; set; }

        [Required]
        public string TvShowId { get; set; }

        public TvShow TvShow { get; set; }
    }
}
