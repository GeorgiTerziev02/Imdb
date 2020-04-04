using Imdb.Data.Common.Models;
using System;

namespace Imdb.Data.Models
{
    public class UserTvShow
    {
        public string UserId { get; set; }

        public ApplicationUser User { get; set; }

        public string TvShowId { get; set; }

        public TvShow TvShow { get; set; }
    }
}