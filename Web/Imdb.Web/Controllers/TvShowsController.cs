namespace Imdb.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

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

        public IActionResult All()
        {
            var tvshows = new ListAllTvShowsViewModel()
            {
                TvShows = this.tvshowsService.GetAll<ListTvShowViewModel>(),
            };
            return this.Json(tvshows);
        }
    }
}
