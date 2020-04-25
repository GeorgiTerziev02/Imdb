namespace Imdb.Web.Areas.Administration.Controllers
{
    using System.Collections.Generic;
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
        private readonly IMoviesService moviesService;
        private readonly IDirectorsService directorsService;
        private readonly IActorsService actorsService;
        private readonly ICloudinaryService cloudinaryService;
        private readonly ILanguageService languageService;
        private readonly IGenresService genresService;

        public AdministrationController(
            IMoviesService moviesService,
            IDirectorsService directorsService,
            IActorsService actorsService,
            ICloudinaryService cloudinaryService,
            ILanguageService languageService,
            IGenresService genresService)
        {
            this.moviesService = moviesService;
            this.directorsService = directorsService;
            this.actorsService = actorsService;
            this.cloudinaryService = cloudinaryService;
            this.languageService = languageService;
            this.genresService = genresService;
        }

        public IActionResult Index()
        {
            return this.View();
        }

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

            var actorId = await this.actorsService.AddAsync(input.FirstName, input.LastName, input.Gender, input.Born, imageUrl, input.Description);

            return this.Redirect($"/Actors/{actorId}");
        }

        public IActionResult AddMovie()
        {
            var viewModel = new AddMovieInputViewModel()
            {
                Languages = this.languageService.GetAll<LanguageDropDownViewModel>(),
                Directors = this.directorsService.GetAll<DirectorDropDownViewModel>(),
            };
            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddMovie(AddMovieInputViewModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            var imageUrl = await this.cloudinaryService.UploudImageAsync(input.Image);
            if (imageUrl == null)
            {
                imageUrl = GlobalConstants.NoImagePicture;
            }

            input.GeneralImageUrl = imageUrl;
            var movieId = await this.moviesService.AddMovie<AddMovieInputViewModel>(input);

            return this.Redirect($"/Admin/Administration/AddActorsAndGenres/{movieId}");
        }

        public IActionResult AddTvShow()
        {
            var viewModel = new AddTvShowInputViewModel()
            {
                Languages = this.languageService.GetAll<LanguageDropDownViewModel>(),
                Directors = this.directorsService.GetAll<DirectorDropDownViewModel>(),
            };

            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddTvShow(AddTvShowInputViewModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            var imageUrl = await this.cloudinaryService.UploudImageAsync(input.Image);
            if (imageUrl == null)
            {
                imageUrl = GlobalConstants.NoImagePicture;
            }

            input.GeneralImageUrl = imageUrl;
            var tvshowId = await this.moviesService.AddTvShowAsync(
                input.Title,
                input.Description,
                input.Duration,
                input.ReleaseDate,
                input.EpisodesCount,
                input.LanguageId,
                input.DirectorId,
                input.GeneralImageUrl,
                input.Trailer);

            return this.Redirect($"/Admin/Administration/AddActorsAndGenres/{tvshowId}");
        }

        public IActionResult AddActorsAndGenres(string id)
        {
            var model = this.moviesService.GetById<AddActorsAndGenresViewModel>(id);
            model.AvailableGenres = this.genresService.GetAll<GenreDropDownViewModel>();
            model.AvailableActors = this.actorsService.GetAll<ActorsDropDownViewModel>();

            return this.View(model);
        }

        public IActionResult AddImages(string movieId)
        {
            var imagesModel = this.moviesService.GetById<AddImagesInputViewModel>(movieId);

            return this.View(imagesModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddImagesAsync(AddImagesInputViewModel input)
        {
            var imagesUrls = new List<string>();
            foreach (var image in input.Images)
            {
                var imageUrl = await this.cloudinaryService.UploudImageAsync(image);

                if (imageUrl != null)
                {
                    imagesUrls.Add(imageUrl);
                }
            }

            await this.moviesService.UploadImages(input.Id, imagesUrls);
            return this.Redirect($"/Movies/ById/{input.Id}");
        }

        public IActionResult Edit(string movieId)
        {
            if (!this.moviesService.IsMovieIdValid(movieId))
            {
                return this.BadRequest();
            }

            var movie = this.moviesService.GetMovieToEdit<EditMovieInputViewModel>(movieId);
            movie.Directors = this.directorsService.GetAll<DirectorDropDownViewModel>();
            movie.Languages = this.languageService.GetAll<LanguageDropDownViewModel>();

            return this.View(movie);
        }

        [HttpPost]
        public async Task<IActionResult> EditMovie(EditMovieInputViewModel editModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(editModel);
            }

            if (!this.moviesService.IsMovieIdValid(editModel.Id))
            {
                return this.BadRequest();
            }

            await this.moviesService.EditMovieAsync(
                editModel.Id,
                editModel.Title,
                editModel.Description,
                editModel.Gross,
                editModel.Budget,
                editModel.DirectorId,
                editModel.LanguageId,
                editModel.Duration,
                editModel.ReleaseDate,
                editModel.Trailer);

            return this.Redirect($"/Movies/ById/{editModel.Id}");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteById(string movieId)
        {
            if (!this.moviesService.IsMovieIdValid(movieId))
            {
                return this.BadRequest();
            }

            await this.moviesService.DeleteByIdAsync(movieId);

            return this.Redirect("/Movies/All");
        }
    }
}
