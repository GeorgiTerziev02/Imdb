namespace Imdb.Web.Controllers
{
    using Imdb.Services.Data.Contracts;
    using Imdb.Web.ViewModels.Movies;
    using Microsoft.AspNetCore.Mvc;
    using System.Collections.Generic;
    using System.Security.Claims;

    public class MoviesController : BaseController
    {
        private readonly IMoviesService moviesService;
        private readonly IVotesService votesService;
        private const int ItemsPerPage = 5;

        public MoviesController(IMoviesService moviesService, IVotesService votesService)
        {
            this.moviesService = moviesService;
            this.votesService = votesService;
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

            if (this.User.Identity.IsAuthenticated)
            {
                var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
                movie.UserVote = this.votesService.GetUserRatingForMovie(userId, id);
            }
            else
            {
                movie.UserVote = null;
            }

            movie.PossibleVotes = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

            return this.View(movie);
        }
    }
}
