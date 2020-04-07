using Imdb.Data.Models;
using Imdb.Services.Mapping;
using System;
using System.Collections.Generic;
using System.Text;

namespace Imdb.Web.ViewModels.TvShows
{
    public class TvShowReviewInfoViewModel : IMapFrom<Review>
    {
        public string UserUsername { get; set; }

        public string Content { get; set; }
    }
}
