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

        public async Task<IEnumerable<T>> GetAll<T>(string userId, int skip, int take, string sorting)
        {
            return sorting switch
            {
                "name_desc" => await this.watchlistRepository
                                   .All()
                                   .Where(x => x.UserId == userId)
                                   .OrderByDescending(x => x.Movie.Title)
                                   .Skip(skip)
                                   .Take(take)
                                   .To<T>()
                                   .ToListAsync(),
                "Date" => await this.watchlistRepository
                                   .All()
                                   .Where(x => x.UserId == userId)
                                   .OrderBy(x => x.Movie.ReleaseDate)
                                   .Skip(skip)
                                   .Take(take)
                                   .To<T>()
                                   .ToListAsync(),
                "date_desc" => await this.watchlistRepository
                                   .All()
                                   .Where(x => x.UserId == userId)
                                   .OrderByDescending(x => x.Movie.ReleaseDate)
                                   .Skip(skip)
                                   .Take(take)
                                   .To<T>()
                                   .ToListAsync(),
                "rating_desc" => await this.watchlistRepository
                                   .All()
                                   .Where(x => x.UserId == userId)
                                   .OrderByDescending(x => x.Movie.Votes.Average(x => x.Rating))
                                   .Skip(skip)
                                   .Take(take)
                                   .To<T>()
                                   .ToListAsync(),
                "Rating" => await this.watchlistRepository
                                   .All()
                                   .Where(x => x.UserId == userId)
                                   .OrderBy(x => x.Movie.Votes.Average(x => x.Rating))
                                   .Skip(skip)
                                   .Take(take)
                                   .To<T>()
                                   .ToListAsync(),
                _ => await this.watchlistRepository
                                   .All()
                                   .Where(x => x.UserId == userId)
                                   .OrderBy(x => x.Movie.Title)
                                   .Skip(skip)
                                   .Take(take)
                                   .To<T>()
                                   .ToListAsync(),
            };
        }

        public async Task RemoveFromWatchlistAsync(string userId, string movieId)
        {
            var userMoive = this.watchlistRepository
                .All()
                .FirstOrDefault(x => x.UserId == userId && x.MovieId == movieId);

            this.watchlistRepository.Delete(userMoive);
            await this.watchlistRepository.SaveChangesAsync();
        }

        public async Task<bool> WatchlistMovieExists(string userId, string movieId)
        {
            return await this.watchlistRepository
                .AllAsNoTracking()
                .AnyAsync(x => x.UserId == userId && x.MovieId == movieId);
        }

        public async Task<int> GetCount(string userId)
        {
            return await this.watchlistRepository
                .AllAsNoTracking()
                .Where(x => x.UserId == userId)
                .CountAsync();
        }

        public async Task<int> MostWatchedGenreId(string userId)
        {
            var genresIds = await this.watchlistRepository
                .AllAsNoTracking()
                .Where(x => x.UserId == userId)
                .SelectMany(x => x.Movie.Genres.Select(x => x.GenreId))
                .ToArrayAsync();

            var length = genresIds.Length;

            return MostFrequent(genresIds, length);
        }

        public async Task<IEnumerable<T>> Recommend<T>(string userId, int genreId, int count)
        {
            var notToRecommend = await this.watchlistRepository
                .AllAsNoTracking()
                .Where(x => x.UserId == userId)
                .Select(x => x.MovieId)
                .ToListAsync();

            return await this.moviesRepository
                .AllAsNoTracking()
                .Where(x => x.Genres.Any(y => y.GenreId == genreId))
                .Where(x => !notToRecommend.Contains(x.Id))
                .OrderByDescending(x => x.Votes.Average(y => y.Rating))
                .Take(count)
                .To<T>()
                .ToListAsync();
        }

        // TODO: Random recommend and has nothing random.... nice
        public async Task<IEnumerable<T>> RandomRecommend<T>(string userId, int count)
        {
            var notToRecommend = await this.watchlistRepository
                .AllAsNoTracking()
                .Where(x => x.UserId == userId)
                .Select(x => x.MovieId)
                .ToListAsync();

            return await this.moviesRepository
                .AllAsNoTracking()
                .Where(x => !notToRecommend.Contains(x.Id))
                .OrderByDescending(x => x.Votes.Average(y => y.Rating))
                .Take(count)
                .To<T>()
                .ToListAsync();
        }
    }
}
