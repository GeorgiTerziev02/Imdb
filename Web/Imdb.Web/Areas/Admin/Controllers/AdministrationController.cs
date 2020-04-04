namespace Imdb.Web.Areas.Administration.Controllers
{
    using Imdb.Common;
    using Imdb.Web.Controllers;
    using Imdb.Web.ViewModels.Admin.Administration;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
    [Area("Admin")]
    public class AdministrationController : BaseController
    {
        public AdministrationController()
        {
        }

        public IActionResult AddDirector()
        {
            return this.Ok();
        }

        [HttpPost]
        public IActionResult AddDirector(AddDirectorInputViewModel input)
        {
            return this.Redirect("/Home/Index");
        }

        public IActionResult AddActor()
        {
            return this.View();
        }

        [HttpPost]
        public IActionResult AddActor(AddActorInputViewModel input)
        {
            return this.Redirect("/Home/Index");
        }

        public IActionResult AddMovie()
        {
            return this.View();
        }

        [HttpPost]
        public IActionResult AddMovie(AddMovieInputViewModel input)
        {
            return this.Redirect("/Home/Index");
        }

        public IActionResult AddTvShow()
        {
            return this.View();
        }

        [HttpPost]
        public IActionResult AddTvShow(AddTvShowInputViewModel input)
        {
            return this.Redirect("/Home/Index");
        }
    }
}
