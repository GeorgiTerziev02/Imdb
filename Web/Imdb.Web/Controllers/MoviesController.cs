namespace Imdb.Web.Controllers
{
    using Imdb.Services.Data.Contracts;
    using Imdb.Web.ViewModels.Movies;
    using Microsoft.AspNetCore.Mvc;

    public class MoviesController : BaseController
    {
        private readonly IMoviesService moviesService;
        private const int ItemsPerPage = 5;

        public MoviesController(IMoviesService moviesService)
        {
            this.moviesService = moviesService;
        }

        public IActionResult All(int page = 1)
        {
            var count = this.moviesService.GetTotalCount();
            if (page <= 0 || page > count)
            {
                page = 1;
            }

            var allMovies = new ListAllMoviesViewModel
            {
                Movies = this.moviesService.GetAll<ListMovieViewModel>((page - 1) * ItemsPerPage, ItemsPerPage),
            };

            var pagesCount = ((count - 1) / ItemsPerPage) + 1;
            allMovies.PageCount = pagesCount;
            allMovies.CurrentPage = page;
            return this.View(allMovies);
        }

        public IActionResult ById(string id)
        {
            var movie = this.moviesService.GetById<MovieInfoViewModel>(id);

            if (movie == null)
            {
                // TODO: 404
                return this.BadRequest();
            }

            return this.View(movie);
        }
    }
}
