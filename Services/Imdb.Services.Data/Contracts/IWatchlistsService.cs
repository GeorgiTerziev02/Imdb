namespace Imdb.Services.Data.Contracts
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    public interface IWatchlistsService
    {
        Task AddToWatchlist(string userId, string movieId);

        IEnumerable<T> GetMovies<T>(string userId);

        IEnumerable<T> TvShows<T>(string userId);

        IEnumerable<T> GetFullWatchlist<T>(string userId);
    }
}
