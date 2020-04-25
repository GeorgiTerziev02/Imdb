namespace Imdb.Web.Controllers
{
    using System.Threading.Tasks;

    using Imdb.Services.Data.Contracts;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("api/[controller]")]
    public class MoviesApiController : ControllerBase
    {
        private readonly IMoviesService moviesService;

        public MoviesApiController(IMoviesService moviesService)
        {
            this.moviesService = moviesService;
        }

        [HttpGet("search")]
        [Produces("application/json")]
        public async Task<IActionResult> SearchAsync(string word)
        {
            string term = this.HttpContext.Request.Query["term"].ToString();
            var suggestions = await this.moviesService.NamesSuggestion(term);
            return this.Ok(suggestions);
        }
    }
}
