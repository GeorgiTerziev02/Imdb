namespace Imdb.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    public class BaseController : Controller
    {
        protected const int ItemsPerPage = 5;
        protected const int TopMoviesCount = 3;
        protected const int ActorsPerPage = 10;
    }
}
