namespace Imdb.Web.Controllers
{
    using System.Threading.Tasks;

    using Imdb.Services.Data.Contracts;
    using Imdb.Web.ViewModels.AddActors;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("api/[controller]")]
    public class AddActorsController : ControllerBase
    {
        private readonly IMoviesService moviesService;
        private readonly IActorsService actorsService;

        public AddActorsController(IMoviesService moviesService, IActorsService actorsService)
        {
            this.moviesService = moviesService;
            this.actorsService = actorsService;
        }

        // TODO: try out httpDelete
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<ActorResponseModel>> Post(ActorInputModel input)
        {
            if (!this.moviesService.IsMovieIdValid(input.MovieId))
            {
                return this.BadRequest();
            }

            if (!this.actorsService.IsActorIdValid(input.ActorId))
            {
                return this.BadRequest();
            }

            if (this.moviesService.ContainsActor(input.MovieId, input.ActorId))
            {
                return this.BadRequest();
            }

            await this.moviesService.AddActorAsync(input.MovieId, input.ActorId);
            var actorName = this.actorsService.GetName(input.ActorId);
            var response = new ActorResponseModel()
            {
                Name = actorName,
            };

            return response;
        }
    }
}
