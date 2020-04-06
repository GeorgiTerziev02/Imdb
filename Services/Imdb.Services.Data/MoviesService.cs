using Imdb.Data.Common.Repositories;
using Imdb.Data.Models;
using Imdb.Services.Data.Contracts;
using Imdb.Services.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Imdb.Services.Data
{
    public class MoviesService : IMoviesService
    {
        private readonly IDeletableEntityRepository<Movie> moviesRepository;

        public MoviesService(IDeletableEntityRepository<Movie> moviesRepository)
        {
            this.moviesRepository = moviesRepository;
        }

        public async Task AddMovie<T>(T model)
        {
            var newMovie = AutoMapperConfig.MapperInstance.Map<Movie>(model);
            await this.moviesRepository.AddAsync(newMovie);
            await this.moviesRepository.SaveChangesAsync();
        }

        public IEnumerable<T> GetAll<T>()
        {
            return this.moviesRepository.All().To<T>().ToList();
        }

        public T GetById<T>(string id)
        {
            return this.moviesRepository.All().Where(x => x.Id == id).To<T>().FirstOrDefault();
        }

        public IEnumerable<T> GetTop5Movies<T>()
        {
            return this.moviesRepository.All().OrderByDescending(x => x.Votes.Average(y => y.Rating)).To<T>().ToList();
        }
    }
}
