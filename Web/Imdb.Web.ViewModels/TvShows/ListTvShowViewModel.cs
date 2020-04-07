namespace Imdb.Web.ViewModels.TvShows
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using Imdb.Data.Models;
    using Imdb.Services.Mapping;

    public class ListTvShowViewModel : IMapFrom<TvShow>
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public int? EpisodesCount { get; set; }

        public string GeneralImageUrl { get; set; }

        public double? Rating { get; set; }
    }
}
