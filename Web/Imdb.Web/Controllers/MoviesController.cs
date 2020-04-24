namespace Imdb.Web.Controllers
{
    using System.Security.Claims;

    using Imdb.Services.Data.Contracts;
    using Imdb.Web.ViewModels.Movies;
    using Microsoft.AspNetCore.Mvc;

    public class MoviesController : BaseController
    {
        private readonly IMoviesService moviesService;
        private readonly IVotesService votesService;
        private readonly IGenresService genresService;

        public MoviesController(IMoviesService moviesService, IVotesService votesService, IGenresService genresService)
        {
            this.moviesService = moviesService;
            this.votesService = votesService;
            this.genresService = genresService;
        }

        public IActionResult All(string sorting, int page = 1)
        {
            var count = this.moviesService.GetTotalCount();
            if (page <= 0 || page > (((count - 1) / ItemsPerPage) + 1))
            {
                page = 1;
            }

            var allMovies = new ListAllMoviesViewModel
            {
                Movies = this.moviesService
                       .GetAll<ListMovieViewModel>((page - 1) * ItemsPerPage, ItemsPerPage, sorting),
            };

            this.ViewData["TitleSortParm"] = string.IsNullOrEmpty(sorting) ? "name_desc" : string.Empty;
            this.ViewData["ReleaseDateSortParm"] = sorting == "Date" ? "date_desc" : "Date";
            this.ViewData["RatingSortParm"] = sorting == "Rating" ? "rating_desc" : "Rating";

            var pagesCount = ((count - 1) / ItemsPerPage) + 1;
            allMovies.PageCount = pagesCount;
            allMovies.CurrentPage = page;
            allMovies.CurrentSorting = sorting;

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

            // TODO: Add user review picture
            movie.PossibleVotes = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

            return this.View(movie);
        }

        public IActionResult ByGenre(int genreId)
        {
            var genreName = this.genresService.GetGenreName(genreId);

            if (genreName == null)
            {
                return this.BadRequest();
            }

            var movies = new ListMoviesViewModel()
            {
                GenreName = genreName,
                Movies = this.moviesService.GetByGenreId<MovieByGenreViewModel>(genreId),
            };

            return this.View(movies);
        }

        public IActionResult Results(string movieTitle)
        {
            if (string.IsNullOrWhiteSpace(movieTitle))
            {
                return this.Redirect("/");
            }

            var movies = new SearchListViewModel()
            {
                Results = this.moviesService.Find<SearchMovieViewModel>(movieTitle),
            };

            return this.View(movies);
        }
    }
}
