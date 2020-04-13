namespace Imdb.Web.ViewModels.Review
{
    using System.ComponentModel.DataAnnotations;

    public class AddReviewInputViewModel
    {
        [Required]
        public string MovieId { get; set; }

        [Required]
        public string Content { get; set; }
    }
}
