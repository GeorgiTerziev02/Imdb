namespace Imdb.Web.Controllers
{
    using System.Diagnostics;
    using System.Threading.Tasks;

    using Imdb.Services.Data.Contracts;
    using Imdb.Web.ViewModels;
    using Imdb.Web.ViewModels.Home;
    using Microsoft.AspNetCore.Mvc;

    public class HomeController : BaseController
    {
        private readonly IMoviesService moviesService;
        private readonly IActorsService actorsService;

        public HomeController(IMoviesService moviesService, IActorsService actorsService)
        {
            this.moviesService = moviesService;
            this.actorsService = actorsService;
        }

        public async Task<IActionResult> Index()
        {
            IndexViewModel topMovie = new IndexViewModel
            {
                TopMovies = await this.moviesService
                                .GetTopMovies<TopMovieViewModel>(TopMoviesCount),
                BornToday = await this.actorsService
                                .GetBornToday<BornTodayActorViewModel>(ActorsBornToday),
            };
            return this.View(topMovie);
        }

        public IActionResult Privacy()
        {
            return this.View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return this.View(
                new ErrorViewModel { RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier });
        }
    }
}
