namespace Imdb.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    public class WatchlistsController : BaseController
    {
        public async Task<IActionResult> AddToWatchlist()
        {
            return this.Json(string.Empty);
        }

        public IActionResult Movies(string id)
        {
            return this.Json(string.Empty);
        }

        public IActionResult Tvshows(string id)
        {
            return this.Json(string.Empty);
        }
    }
}
