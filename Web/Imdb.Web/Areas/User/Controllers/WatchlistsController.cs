namespace Imdb.Web.Areas.User
{
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Imdb.Services.Data.Contracts;
    using Imdb.Web.Controllers;
    using Imdb.Web.ViewModels.Watchlists;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    [Area("User")]
    public class WatchlistsController : BaseController
    {
        private readonly IMoviesService moviesService;
        private readonly IWatchlistsService watchlistsService;

        public WatchlistsController(IMoviesService moviesService, IWatchlistsService watchlistsService)
        {
            this.moviesService = moviesService;
            this.watchlistsService = watchlistsService;
        }

        public async Task<IActionResult> Add(string userId, string movieId)
        {
            if (this.User.FindFirstValue(ClaimTypes.NameIdentifier) != userId)
            {
                return this.BadRequest();
            }

            if (!this.moviesService.IsMovieIdValid(movieId))
            {
                return this.BadRequest();
            }

            if (this.watchlistsService.WatchlistMovieExists(userId, movieId))
            {
                return this.Redirect($"Movies?userId={userId}");
            }

            await this.watchlistsService.AddToWatchlistAsync(userId, movieId);

            return this.Redirect($"Movies?userId={userId}");
        }

        public async Task<IActionResult> Remove(string userId, string movieId)
        {
            if (this.User.FindFirstValue(ClaimTypes.NameIdentifier) != userId)
            {
                return this.BadRequest();
            }

            if (!this.moviesService.IsMovieIdValid(movieId))
            {
                return this.BadRequest();
            }

            if (!this.watchlistsService.WatchlistMovieExists(userId, movieId))
            {
                return this.BadRequest();
            }

            await this.watchlistsService.RemoveFromWatchlistAsync(userId, movieId);

            return this.RedirectToAction("Movies", new { userId });
        }

        public IActionResult Movies(string userId)
        {
            if (this.User.FindFirstValue(ClaimTypes.NameIdentifier) != userId)
            {
                return this.BadRequest();
            }

            var watchlist = new FullWatchlistViewModel()
            {
                Movies = this.watchlistsService.GetMovies<WatchlistMovieViewModel>(userId),
            };

            return this.Json(watchlist);
        }

        public IActionResult Tvshows()
        {
            return this.Json(string.Empty);
        }
    }
}
