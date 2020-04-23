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
        private readonly IDeletableEntityRepository<Movie> moviesRepository;

        public WatchlistService(
            IRepository<UserMovie> watchlistRepository,
            IDeletableEntityRepository<Movie> moviesRepository)
        {
            this.watchlistRepository = watchlistRepository;
            this.moviesRepository = moviesRepository;
        }

        public static int MostFrequent(int[] arr, int length)
        {
            Dictionary<int, int> kvp = new Dictionary<int, int>();

            for (int i = 0; i < length; i++)
            {
                int key = arr[i];
                if (kvp.ContainsKey(key))
                {
                    int freq = kvp[key];
                    freq++;
                    kvp[key] = freq;
                }
                else
                {
                    kvp.Add(key, 1);
                }
            }

            int minCount = 0, result = -1;

            foreach (KeyValuePair<int, int> pair in kvp)
            {
                if (minCount < pair.Value)
                {
                    result = pair.Key;
                    minCount = pair.Value;
                }
            }

            return result;
        }

        public async Task AddToWatchlistAsync(string userId, string movieId)
        {
            var userMovie = new UserMovie()
            {
                UserId = userId,
                MovieId = movieId,
            };

            await this.watchlistRepository.AddAsync(userMovie);
            await this.watchlistRepository.SaveChangesAsync();
        }

        public IEnumerable<T> GetAll<T>(string userId, int skip, int take, string sorting)
        {
            var watchlist = this.watchlistRepository
                .All()
                .Where(x => x.UserId == userId);

            switch (sorting)
            {
                case "name_desc":
                    watchlist = watchlist.OrderByDescending(m => m.Movie.Title);
                    break;
                case "Date":
                    watchlist = watchlist.OrderBy(m => m.Movie.ReleaseDate);
                    break;
                case "date_desc":
                    watchlist = watchlist.OrderByDescending(m => m.Movie.ReleaseDate);
                    break;
                case "rating_desc":
                    watchlist = watchlist.OrderByDescending(m => m.Movie.Votes.Average(x => x.Rating));
                    break;
                case "Rating":
                    watchlist = watchlist.OrderBy(m => m.Movie.Votes.Average(x => x.Rating));
                    break;
                default:
                    watchlist = watchlist.OrderBy(m => m.Movie.Title);
                    break;
            }

            return watchlist
                .Skip(skip)
                .Take(take)
                .To<T>()
                .ToList();
        }

        public async Task RemoveFromWatchlistAsync(string userId, string movieId)
        {
            var userMoive = this.watchlistRepository
                .All()
                .FirstOrDefault(x => x.UserId == userId && x.MovieId == movieId);

            this.watchlistRepository.Delete(userMoive);
            await this.watchlistRepository.SaveChangesAsync();
        }

        public bool WatchlistMovieExists(string userId, string movieId)
        {
            return this.watchlistRepository
                .AllAsNoTracking()
                .Any(x => x.UserId == userId && x.MovieId == movieId);
        }

        public IEnumerable<T> TvShows<T>(string userId)
        {
            return this.watchlistRepository
                .All()
                .Where(x => x.UserId == userId && x.Movie.IsTvShow)
                .To<T>()
                .ToList();
        }

        public int GetCount(string userId)
        {
            return this.watchlistRepository
                .AllAsNoTracking()
                .Where(x => x.UserId == userId)
                .Count();
        }

        public int MostWatchedGenreId(string userId)
        {
            var genresIds = this.watchlistRepository
                .AllAsNoTracking()
                .Where(x => x.UserId == userId)
                .SelectMany(x => x.Movie.Genres.Select(x => x.GenreId))
                .ToArray();

            var length = genresIds.Length;

            return MostFrequent(genresIds, length);
        }

        public IEnumerable<T> Recommend<T>(string userId, int genreId, int count)
        {
            var notToRecommend = this.watchlistRepository
                .AllAsNoTracking()
                .Where(x => x.UserId == userId)
                .Select(x => x.MovieId)
                .ToList();

            return this.moviesRepository
                .AllAsNoTracking()
                .Where(x => x.Genres.Any(y => y.GenreId == genreId))
                .Where(x => !notToRecommend.Contains(x.Id))
                .OrderByDescending(x => x.Votes.Average(y => y.Rating))
                .Take(count)
                .To<T>()
                .ToList();
        }

        // TODO: Random recommend and has nothing random.... nice
        public IEnumerable<T> RandomRecommend<T>(string userId, int count)
        {
            var notToRecommend = this.watchlistRepository
                .AllAsNoTracking()
                .Where(x => x.UserId == userId)
                .Select(x => x.MovieId)
                .ToList();

            return this.moviesRepository
                .AllAsNoTracking()
                .Where(x => !notToRecommend.Contains(x.Id))
                .OrderByDescending(x => x.Votes.Average(y => y.Rating))
                .Take(count)
                .To<T>()
                .ToList();
        }
    }
}
