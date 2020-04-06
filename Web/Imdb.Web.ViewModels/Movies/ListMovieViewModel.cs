using Imdb.Data.Models;
using Imdb.Services.Mapping;
using System;
using System.Collections.Generic;
using System.Text;

namespace Imdb.Web.ViewModels.Movies
{
    public class ListMovieViewModel : IMapFrom<Movie>
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public DateTime? ReleaseDate { get; set; }

        public string GeneralImageUrl { get; set; }

        public double? Rating { get; set; }
    }
}
