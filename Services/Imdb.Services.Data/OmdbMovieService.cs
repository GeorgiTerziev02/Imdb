namespace Imdb.Services.Data
{
    using System;
    using System.Threading.Tasks;

    using Imdb.Common;
    using Imdb.Data.Common.Repositories;
    using Imdb.Data.Models;
    using Imdb.Data.Models.Enumerations;
    using Imdb.Services.Data.Contracts;
    using Imdb.Web.ViewModels.Admin.Administration;

    public class OmdbMovieService : IOmdbMovieService
    {
        private readonly IDeletableEntityRepository<Movie> moviesRepository;
        private readonly ILanguageService languageService;
        private readonly IDirectorsService directorsService;

        public OmdbMovieService(
            IDeletableEntityRepository<Movie> moviesRepository,
            ILanguageService languageService,
            IDirectorsService directorsService)
        {
            this.moviesRepository = moviesRepository;
            this.languageService = languageService;
            this.directorsService = directorsService;
        }

        public async Task<string> AddMovieFromOmdb(OmdbDataProviderModel model)
        {
            var languageId = await this.languageService.GetId(model.Language.Split(", ")[0]);

            TimeSpan? duration = null;
            if (model.Runtime != null)
            {
                duration = TimeSpan.FromMinutes(double.Parse(model.Runtime.Split(" ")[0]));
            }

            bool isTvShow = model.Type == "series" ? true : false;

            var director = model.Director;
            if (director == null)
            {
                director = model.Writer;
            }

            var directorId = await this.directorsService.GetId(director);
            if (directorId == null)
            {
                var directorsNames = director.Split(" ");
                directorId = await this.directorsService.AddAsync(directorsNames[0], directorsNames[1], Gender.Male, DateTime.UtcNow, GlobalConstants.DefaulProfilePicture, "No info");
            }

            // TODO: make genres and actors to be seeded automatically
            var movie = new Movie()
            {
                Title = model.Title,
                Description = model.Plot,
                IsTvShow = isTvShow,
                Duration = duration,
                LanguageId = languageId.Value,
                DirectorId = directorId,
                ReleaseDate = DateTime.Parse(model.Released),
                GeneralImageUrl = model.Poster,
            };

            await this.moviesRepository.AddAsync(movie);
            await this.moviesRepository.SaveChangesAsync();

            return movie.Id;
        }
    }
}
