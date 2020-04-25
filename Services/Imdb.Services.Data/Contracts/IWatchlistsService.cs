namespace Imdb.Services.Data.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IWatchlistsService
    {
        Task AddToWatchlistAsync(string userId, string movieId);

        Task<bool> WatchlistMovieExists(string userId, string movieId);

        Task RemoveFromWatchlistAsync(string userId, string movieId);

        Task<IEnumerable<T>> GetAll<T>(string userId, int skip, int take, string sorting);

        Task<int> GetCount(string userId);

        Task<int> MostWatchedGenreId(string userId);

        Task<IEnumerable<T>> Recommend<T>(string userId, int genreId, int count);

        Task<IEnumerable<T>> RandomRecommend<T>(string userId, int count);
    }
}
