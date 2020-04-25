﻿namespace Imdb.Web.Controllers
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
            if (await this.genresService.MovieContainsGenre(input.GenreId, input.MovieId))
            {
                return this.BadRequest();
            }

            await this.genresService.AddGenreToMovie(input.GenreId, input.MovieId);
            var response = new GenreMovieResponseModel()
            {
                GenreName = await this.genresService.GetGenreName(input.GenreId),
            };

            return response;
        }

        [HttpDelete]
        [Authorize]
        public async Task<ActionResult<RemoveMovieGenreResponseModel>> Delete(RemoveGenreViewModel model)
        {
            var id = await this.genresService.RemoveGenreFromMovie(model.Id);
            if (id == null)
            {
                return this.BadRequest();
            }

            var response = new RemoveMovieGenreResponseModel()
            {
                Id = id.Value,
            };

            return response;
        }
    }
}
