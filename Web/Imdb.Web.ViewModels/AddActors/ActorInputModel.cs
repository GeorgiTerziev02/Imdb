namespace Imdb.Web.ViewModels.AddActors
{
    using System.ComponentModel.DataAnnotations;

    public class ActorInputModel
    {
        [Required]
        public string MovieId { get; set; }

        [Required]
        public string ActorId { get; set; }
    }
}
