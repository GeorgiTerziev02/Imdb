namespace Imdb.Services.Data.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IWatchlistsService
    {
        Task AddToWatchlistAsync(string userId, string movieId);

        bool WatchlistMovieExists(string userId, string movieId);

        Task RemoveFromWatchlistAsync(string userId, string movieId);

        IEnumerable<T> GetAll<T>(string userId, int skip, int take, string sorting);

        int GetCount(string userId);

        int MostWatchedGenreId(string userId);

        IEnumerable<T> Recommend<T>(string userId, int genreId, int count);

        IEnumerable<T> RandomRecommend<T>(string userId, int count);
    }
}
