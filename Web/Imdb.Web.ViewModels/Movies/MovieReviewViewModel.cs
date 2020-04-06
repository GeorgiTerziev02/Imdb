using Imdb.Data.Models;
using Imdb.Services.Mapping;
using System;
using System.Collections.Generic;
using System.Text;

namespace Imdb.Web.ViewModels.Movies
{
    public class MovieReviewViewModel : IMapFrom<Review>
    {
        public string UserUsername { get; set; }

        public string Content { get; set; }
    }
}
