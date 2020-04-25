namespace Imdb.Web.Controllers
{
    using System.Threading.Tasks;

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

        public async Task<IActionResult> ById(string directorId)
        {
            var director = await this.directorsService.GetById<DirectorInfoViewModel>(directorId);

            if (director == null)
            {
                return this.BadRequest();
            }

            return this.View(director);
        }
    }
}
