using System.ComponentModel.DataAnnotations;

namespace Imdb.Web.ViewModels.Votes
{
    public class MovieVoteInputModel
    {
        [Required]
        public string MovieId { get; set; }

        [Range(0, 10)]
        [Required]
        public int Rating { get; set; }
    }
}
