namespace Imdb.Web.Controllers
{
    using Imdb.Services.Data.Contracts;
    using Imdb.Web.ViewModels.Actors;
    using Microsoft.AspNetCore.Mvc;

    public class ActorsController : BaseController
    {
        private readonly IActorsService actorsService;

        public ActorsController(IActorsService actorsService)
        {
            this.actorsService = actorsService;
        }

        public IActionResult All()
        {
            var actors = new AllActorsListViewModel()
            {
                Actors = this.actorsService.GetAll<ActorViewModel>(),
            };

            return this.Json(actors);
        }

        public IActionResult ById(string id)
        {
            var actor = this.actorsService.GetById<ActorByIdViewModel>(id);

            return this.Json(actor);
        }
    }
}
