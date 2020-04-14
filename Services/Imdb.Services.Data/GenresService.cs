namespace Imdb.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Imdb.Data.Common.Repositories;
    using Imdb.Data.Models;
    using Imdb.Services.Data.Contracts;
    using Imdb.Services.Mapping;

    public class GenresService : IGenresService
    {
        private readonly IRepository<Genre> genresRepository;
        private readonly IRepository<MovieGenre> movieGenreRepository;

        public GenresService(IRepository<Genre> genresRepository, IRepository<MovieGenre> movieGenreRepository)
        {
            this.genresRepository = genresRepository;
            this.movieGenreRepository = movieGenreRepository;
        }

        public IEnumerable<T> GetAll<T>()
        {
            return this.genresRepository.AllAsNoTracking().OrderBy(x => x.Name).To<T>().ToList();
        }

        public string GetGenreName(int genreId)
        {
            return this.genresRepository.AllAsNoTracking().FirstOrDefault(x => x.Id == genreId)?.Name;
        }

        public async Task AddGenreToMovie(int genreId, string movieId)
        {
            var model = new MovieGenre()
            {
                GenreId = genreId,
                MovieId = movieId,
            };

            await this.movieGenreRepository.AddAsync(model);
            await this.movieGenreRepository.SaveChangesAsync();
        }

        public bool MovieContainsGenre(int genreId, string movieId)
        {
            return this.movieGenreRepository.AllAsNoTracking().FirstOrDefault(x => x.GenreId == genreId && x.MovieId == movieId) != null;
        }
    }
}
