namespace Imdb.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Imdb.Data.Common.Repositories;
    using Imdb.Data.Models;
    using Imdb.Services.Data.Contracts;
    using Imdb.Services.Mapping;

    public class WatchlistService : IWatchlistsService
    {
        private readonly IRepository<UserMovie> watchlistRepository;

        public WatchlistService(IRepository<UserMovie> watchlistRepository)
        {
            this.watchlistRepository = watchlistRepository;
        }

        public async Task AddToWatchlist(string userId, string movieId)
        {
            var userMovie = new UserMovie()
            {
                UserId = userId,
                MovieId = movieId,
            };

            await this.watchlistRepository.AddAsync(userMovie);
            await this.watchlistRepository.SaveChangesAsync();
        }

        public IEnumerable<T> GetFullWatchlist<T>(string userId)
        {
            return this.watchlistRepository.All().To<T>().ToList();
        }

        public IEnumerable<T> GetMovies<T>(string userId)
        {
            return this.watchlistRepository.All().Where(x => x.UserId == userId).To<T>().ToList();
        }

        public IEnumerable<T> TvShows<T>(string userId)
        {
            return this.watchlistRepository.All().Where(x => x.UserId == userId && x.Movie.IsTvShow).To<T>().ToList();
        }
    }
}
