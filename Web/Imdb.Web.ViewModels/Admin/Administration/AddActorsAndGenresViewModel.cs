using Imdb.Data.Models;
using Imdb.Services.Mapping;
using System;
using System.Collections.Generic;
using System.Text;

namespace Imdb.Web.ViewModels.Admin.Administration
{
    public class AddActorsAndGenresViewModel : IMapFrom<Movie>
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public IEnumerable<GenreInfoViewModel> Genres { get; set; }

        public IEnumerable<GenreDropDownViewModel> AvailableGenres { get; set; }

        public IEnumerable<ActorsDropDownViewModel> AvailableActors { get; set; }
    }
}
