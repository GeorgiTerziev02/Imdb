namespace Imdb.Web.Areas.Administration.Controllers
{
    using System.Threading.Tasks;

    using Imdb.Common;
    using Imdb.Services.Data.Contracts;
    using Imdb.Web.Controllers;
    using Imdb.Web.ViewModels.Admin.Administration;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
    [Area("Admin")]
    public class AdministrationController : BaseController
    {
        private readonly IDirectorsService directorsService;
        private readonly IActorsService actorsService;
        private readonly ICloudinaryService cloudinaryService;

        public AdministrationController(IDirectorsService directorsService, IActorsService actorsService, ICloudinaryService cloudinaryService)
        {
            this.directorsService = directorsService;
            this.actorsService = actorsService;
            this.cloudinaryService = cloudinaryService;
        }

        public IActionResult Index()
        {
            return this.View();
        }

        // TODO: Try to do with one code
        public IActionResult AddDirector()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> AddDirector(AddDirectorInputViewModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            var imageUrl = await this.cloudinaryService.UploudImageAsync(input.Image);
            if (imageUrl == null)
            {
                imageUrl = GlobalConstants.DefaulProfilePicture;
            }

            await this.directorsService.AddAsync(input.FirstName, input.LastName, input.Gender, input.Born, imageUrl, input.Description);

            return this.Redirect("/Home/Index");
        }

        public IActionResult AddActor()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> AddActor(AddActorInputViewModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            var imageUrl = await this.cloudinaryService.UploudImageAsync(input.Image);
            if (imageUrl == null)
            {
                imageUrl = GlobalConstants.DefaulProfilePicture;
            }

            await this.actorsService.AddAsync(input.FirstName, input.LastName, input.Gender, input.Born, imageUrl, input.Description);

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
