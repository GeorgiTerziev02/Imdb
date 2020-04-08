namespace Imdb.Web.ViewModels.Home
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using Imdb.Data.Models;
    using Imdb.Services.Mapping;

    public class TopMovieViewModel : IMapFrom<Movie>
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string GeneralImageUrl { get; set; }
    }
}
