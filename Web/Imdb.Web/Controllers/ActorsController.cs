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

        public IActionResult All(int page = 1)
        {
            var count = this.actorsService.GetTotalCount();
            if (page <= 0 || page > count)
            {
                page = 1;
            }

            var actors = new AllActorsListViewModel()
            {
                Actors = this.actorsService.GetAll<ActorViewModel>((page - 1) * ActorsPerPage, ActorsPerPage),
            };

            var pagesCount = ((count - 1) / ActorsPerPage) + 1;
            actors.PageCount = pagesCount;
            actors.CurrentPage = page;
            return this.View(actors);
        }

        public IActionResult ById(string id)
        {
            var actor = this.actorsService.GetById<ActorByIdViewModel>(id);

            return this.View(actor);
        }
    }
}
