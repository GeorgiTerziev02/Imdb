using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Imdb.Data.Models
{
    public class MovieImage
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string MovieId { get; set; }

        public Movie Movie { get; set; }

        [Required]
        public string ImageUrl { get; set; }
    }
}
