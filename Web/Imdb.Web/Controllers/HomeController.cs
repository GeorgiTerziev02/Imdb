namespace Imdb.Web.Controllers
{
    using System.Diagnostics;

    using Imdb.Web.ViewModels;
    using Imdb.Web.ViewModels.Home;
    using Microsoft.AspNetCore.Mvc;

    public class HomeController : BaseController
    {
        public IActionResult Index()
        {
            IndexViewModel topMovie = new IndexViewModel
            {
                Name = "TopMovie",
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
