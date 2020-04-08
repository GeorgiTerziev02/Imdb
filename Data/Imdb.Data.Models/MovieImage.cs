namespace Imdb.Data.Models
{
    using System.ComponentModel.DataAnnotations;

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
