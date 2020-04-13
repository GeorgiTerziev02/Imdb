using Imdb.Services.Data.Contracts;
using Imdb.Web.ViewModels.Search;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Imdb.Web.Controllers
{
    public class SearchController : Controller
    {
        private readonly IMoviesService moviesService;

        public SearchController(IMoviesService moviesService)
        {
            this.moviesService = moviesService;
        }

        // TODO: pagination
        public IActionResult Results(string name)
        {
            var movies = new SearchListViewModel()
            {
                Results = this.moviesService.Find<SearchMovieViewModel>(name),
            };

            return this.Json(movies);
        }
    }
}
