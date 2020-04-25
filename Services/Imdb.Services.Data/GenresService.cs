namespace Imdb.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Imdb.Data.Common.Repositories;
    using Imdb.Data.Models;
    using Imdb.Services.Data.Contracts;
    using Imdb.Services.Mapping;
    using Microsoft.EntityFrameworkCore;

    public class GenresService : IGenresService
    {
        private readonly IRepository<Genre> genresRepository;
        private readonly IRepository<MovieGenre> movieGenreRepository;

        public GenresService(IRepository<Genre> genresRepository, IRepository<MovieGenre> movieGenreRepository)
        {
            this.genresRepository = genresRepository;
            this.movieGenreRepository = movieGenreRepository;
        }

        public async Task<IEnumerable<T>> GetAll<T>()
        {
            return await this.genresRepository
                .AllAsNoTracking()
                .OrderBy(x => x.Name)
                .To<T>()
                .ToListAsync();
        }

        public async Task<string> GetGenreName(int genreId)
        {
            return (await this.genresRepository
                .AllAsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == genreId))?.Name;
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

        public async Task<bool> MovieContainsGenre(int genreId, string movieId)
        {
            return (await this.movieGenreRepository
                .AllAsNoTracking()
                .FirstOrDefaultAsync(x => x.GenreId == genreId && x.MovieId == movieId)) != null;
        }

        public async Task<int?> RemoveGenreFromMovie(int id)
        {
            var movieGenre = this.movieGenreRepository
                .All()
                .FirstOrDefault(x => x.Id == id);

            if (movieGenre == null)
            {
                return null;
            }

            this.movieGenreRepository.Delete(movieGenre);
            await this.movieGenreRepository.SaveChangesAsync();

            return id;
        }
    }
}
