namespace Imdb.Web.ViewModels.TvShows
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using Imdb.Data.Models;
    using Imdb.Services.Mapping;

    public class TvShowReviewInfoViewModel : IMapFrom<Review>
    {
        public string UserUsername { get; set; }

        public string Content { get; set; }
    }
}
