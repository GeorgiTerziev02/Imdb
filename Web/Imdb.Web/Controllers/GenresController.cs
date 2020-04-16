namespace Imdb.Web.Controllers
{
    using System.Threading.Tasks;

    using Imdb.Services.Data.Contracts;
    using Imdb.Web.ViewModels.Genres;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("api/[controller]")]
    public class GenresController : ControllerBase
    {
        private readonly IGenresService genresService;

        public GenresController(IGenresService genresService)
        {
            this.genresService = genresService;
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<GenreMovieResponseModel>> Post(GenreMovieInputModel input)
        {
            if (this.genresService.MovieContainsGenre(input.GenreId, input.MovieId))
            {
                return this.BadRequest();
            }

            await this.genresService.AddGenreToMovie(input.GenreId, input.MovieId);
            var response = new GenreMovieResponseModel()
            {
                GenreName = this.genresService.GetGenreName(input.GenreId),
            };
            return response;
        }
    }
}
