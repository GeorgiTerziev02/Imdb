using Imdb.Data.Models;
using Imdb.Services.Mapping;
using System;
using System.Collections.Generic;
using System.Text;

namespace Imdb.Web.ViewModels.Home
{
    public class TopMovieViewModel : IMapFrom<Movie>
    {
        public string Title { get; set; }

        public decimal Rating { get; set; }

        public string GeneralImageUrl { get; set; }
    }
}
