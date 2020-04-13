namespace Imdb.Web.ViewModels.Movies
{
    using System;

    using Ganss.XSS;
    using Imdb.Data.Models;
    using Imdb.Services.Mapping;

    public class MovieReviewViewModel : IMapFrom<Review>
    {
        public string Id { get; set; }

        public DateTime CreatedOn { get; set; }

        public string UserId { get; set; }

        public string UserUsername { get; set; }

        public int UserReviewsCount { get; set; }

        public DateTime UserCreatedOn { get; set; }

        public string Content { get; set; }

        public string SanitizedContent => new HtmlSanitizer().Sanitize(this.Content);
    }
}
