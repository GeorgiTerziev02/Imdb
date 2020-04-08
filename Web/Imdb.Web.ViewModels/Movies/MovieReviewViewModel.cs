namespace Imdb.Web.ViewModels.Movies
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using Imdb.Data.Models;
    using Imdb.Services.Mapping;

    public class MovieReviewViewModel : IMapFrom<Review>
    {
        public string UserUsername { get; set; }

        public string Content { get; set; }
    }
}
