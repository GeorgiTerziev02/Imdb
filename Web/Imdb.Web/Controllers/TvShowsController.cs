namespace Imdb.Web.Controllers
{
    using Imdb.Services.Data.Contracts;
    using Imdb.Web.ViewModels.TvShows;
    using Microsoft.AspNetCore.Mvc;

    public class TvShowsController : BaseController
    {
        private readonly ITvShowsService tvshowsService;

        public TvShowsController(ITvShowsService tvshowsService)
        {
            this.tvshowsService = tvshowsService;
        }

        public IActionResult All(int page = 1)
        {
            var count = this.tvshowsService.GetCount();
            if (page <= 0 || page > (((count - 1) / ItemsPerPage) + 1))
            {
                page = 1;
            }

            var tvshows = new ListAllTvShowsViewModel()
            {
                TvShows = this.tvshowsService
                            .GetAll<ListTvShowViewModel>((page - 1) * ItemsPerPage, ItemsPerPage),
            };

            var pagesCount = ((count - 1) / ItemsPerPage) + 1;
            tvshows.PageCount = pagesCount;
            tvshows.CurrentPage = page;
            return this.View(tvshows);
        }
    }
}
