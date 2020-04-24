namespace Imdb.Web.ViewModels.Votes
{
    using System.ComponentModel.DataAnnotations;

    public class MovieVoteInputModel
    {
        [Required]
        public string MovieId { get; set; }

        [Range(0, 10)]
        public int Rating { get; set; }
    }
}
