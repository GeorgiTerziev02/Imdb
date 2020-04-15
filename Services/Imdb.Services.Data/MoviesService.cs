namespace Imdb.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Imdb.Data.Common.Repositories;
    using Imdb.Data.Models;
    using Imdb.Services.Data.Contracts;
    using Imdb.Services.Mapping;
    using Microsoft.EntityFrameworkCore;

    public class MoviesService : IMoviesService
    {
        private readonly IDeletableEntityRepository<Movie> moviesRepository;
        private readonly IRepository<MovieActor> movieActorsRepository;

        public MoviesService(IDeletableEntityRepository<Movie> moviesRepository, IRepository<MovieActor> movieActorsRepository)
        {
            this.moviesRepository = moviesRepository;
            this.movieActorsRepository = movieActorsRepository;
        }

        public async Task AddActorAsync(string movieId, string actorId)
        {
            var model = new MovieActor()
            {
                ActorId = actorId,
                MovieId = movieId,
            };

            await this.movieActorsRepository.AddAsync(model);
            await this.movieActorsRepository.SaveChangesAsync();
        }

        public async Task<string> AddMovie<T>(T model)
        {
            var newMovie = AutoMapperConfig.MapperInstance.Map<Movie>(model);
            newMovie.IsTvShow = false;
            await this.moviesRepository.AddAsync(newMovie);
            await this.moviesRepository.SaveChangesAsync();

            return newMovie.Id;
        }

        public bool ContainsActor(string movieId, string actorId)
        {
            return this.movieActorsRepository.AllAsNoTracking()
                .Where(x => x.ActorId == actorId && x.MovieId == movieId).FirstOrDefault() != null;
        }

        public IEnumerable<T> Find<T>(string name)
        {
            return this.moviesRepository.AllAsNoTracking().Where(x => x.Title.StartsWith(name)).To<T>().ToList();
        }

        public IEnumerable<T> GetAll<T>(int skip, int itemsPerPage)
        {
            return this.moviesRepository.All().Skip(skip).Take(itemsPerPage).OrderBy(x => x.Title).To<T>().ToList();
        }

        public T GetById<T>(string id)
        {
            return this.moviesRepository.All().Where(x => x.Id == id).To<T>().FirstOrDefault();
        }

        public IEnumerable<T> GetTopMovies<T>(int count)
        {
            return this.moviesRepository.All().OrderByDescending(x => x.Votes.Average(y => y.Rating)).ThenByDescending(x => x.Votes.Count()).Take(count).To<T>().ToList();
        }

        public int GetTotalCount()
        {
            return this.moviesRepository.AllAsNoTracking().Count();
        }

        public bool IsMovieIdValid(string movieId)
        {
            return this.moviesRepository.AllAsNoTracking().Any(x => x.Id == movieId);
        }
    }
}
