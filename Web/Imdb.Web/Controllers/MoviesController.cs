using Imdb.Services.Data.Contracts;
using Imdb.Web.ViewModels.Movies;
using Microsoft.AspNetCore.Mvc;

namespace Imdb.Web.Controllers
{
    public class MoviesController : BaseController
    {
        private readonly IMoviesService moviesService;

        public MoviesController(IMoviesService moviesService)
        {
            this.moviesService = moviesService;
        }

        // TODO: Add paging
        public IActionResult All()
        {
            var allMovies = new ListAllMoviesViewModel
            {
                Movies = this.moviesService.GetAll<ListMovieViewModel>(),
            };

            return this.Json(allMovies);
        }

        public IActionResult ById(string id)
        {
            var movie = this.moviesService.GetById<MovieInfoViewModel>(id);
            return this.Json(movie);
        }
    }
}
