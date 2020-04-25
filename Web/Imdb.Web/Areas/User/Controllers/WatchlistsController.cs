﻿namespace Imdb.Web.Areas.User
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

            if (!(await this.moviesService.IsMovieIdValid(movieId)))
            {
                return this.BadRequest();
            }

            if (await this.watchlistsService.WatchlistMovieExists(userId, movieId))
            {
                return this.Redirect($"All?userId={userId}&page=1");
            }

            await this.watchlistsService.AddToWatchlistAsync(userId, movieId);

            return this.Redirect($"All?userId={userId}&page=1");
        }

        public async Task<IActionResult> Remove(string userId, string movieId)
        {
            if (this.User.FindFirstValue(ClaimTypes.NameIdentifier) != userId)
            {
                return this.BadRequest();
            }

            if (!(await this.moviesService.IsMovieIdValid(movieId)))
            {
                return this.BadRequest();
            }

            if (!(await this.watchlistsService.WatchlistMovieExists(userId, movieId)))
            {
                return this.BadRequest();
            }

            await this.watchlistsService.RemoveFromWatchlistAsync(userId, movieId);

            return this.RedirectToAction("All", new { userId });
        }

        public async Task<IActionResult> AllAsync(string userId, string sorting, int page = 1)
        {
            var count = await this.watchlistsService.GetCount(userId);
            if (page <= 0 || page > (((count - 1) / ItemsPerPage) + 1))
            {
                page = 1;
            }

            if (this.User.FindFirstValue(ClaimTypes.NameIdentifier) != userId)
            {
                return this.BadRequest();
            }

            var watchlist = new FullWatchlistViewModel()
            {
                Movies = await this.watchlistsService
                        .GetAll<WatchlistEntityViewModel>(
                        userId, (page - 1) * ItemsPerPage, ItemsPerPage, sorting),
            };

            this.ViewData["TitleSortParm"] = string.IsNullOrEmpty(sorting) ? "name_desc" : string.Empty;
            this.ViewData["ReleaseDateSortParm"] = sorting == "Date" ? "date_desc" : "Date";
            this.ViewData["RatingSortParm"] = sorting == "Rating" ? "rating_desc" : "Rating";

            var pagesCount = ((count - 1) / ItemsPerPage) + 1;
            watchlist.Id = userId;
            watchlist.CurrentPage = page;
            watchlist.PageCount = pagesCount;
            watchlist.CurrentSorting = sorting;

            return this.View(watchlist);
        }

        public IActionResult Tvshows()
        {
            return this.Json(string.Empty);
        }
    }
}
