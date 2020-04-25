namespace Imdb.Web.Controllers
{
    using System.Threading.Tasks;

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

        public async Task<IActionResult> All(int page = 1)
        {
            var count = await this.actorsService.GetTotalCount();
            if (page <= 0 || page > (((count - 1) / ActorsPerPage) + 1))
            {
                page = 1;
            }

            var actors = new AllActorsListViewModel()
            {
                Actors = await this.actorsService
                    .GetAll<ActorViewModel>((page - 1) * ActorsPerPage, ActorsPerPage),
            };

            var pagesCount = ((count - 1) / ActorsPerPage) + 1;
            actors.PageCount = pagesCount;
            actors.CurrentPage = page;
            return this.View(actors);
        }

        public async Task<IActionResult> ById(string id)
        {
            var actor = await this.actorsService.GetById<ActorByIdViewModel>(id);

            if (actor == null)
            {
                return this.NotFound();
            }

            return this.View(actor);
        }
    }
}
