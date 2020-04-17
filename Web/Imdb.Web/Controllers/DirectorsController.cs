namespace Imdb.Web.Controllers
{
    using Imdb.Services.Data.Contracts;
    using Imdb.Web.ViewModels.Directors;
    using Microsoft.AspNetCore.Mvc;

    public class DirectorsController : BaseController
    {
        private readonly IDirectorsService directorsService;

        public DirectorsController(IDirectorsService directorsService)
        {
            this.directorsService = directorsService;
        }

        public IActionResult ById(string directorId)
        {
            var director = this.directorsService.GetById<DirectorInfoViewModel>(directorId);

            if (director == null)
            {
                return this.BadRequest();
            }

            return this.View(director);
        }
    }
}
