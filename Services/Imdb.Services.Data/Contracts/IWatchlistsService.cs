namespace Imdb.Services.Data.Contracts
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    public interface IWatchlistsService
    {
        Task AddToWatchlistAsync(string userId, string movieId);

        bool WatchlistMovieExists(string userId, string movieId);

        Task RemoveFromWatchlistAsync(string userId, string movieId);

        IEnumerable<T> GetMovies<T>(string userId);
    }
}
