namespace Imdb.Services.Data.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IGenresService
    {
        Task<IEnumerable<T>> GetAll<T>();

        Task<bool> MovieContainsGenre(int genreId, string movieId);

        Task AddGenreToMovie(int genreId, string movieId);

        Task<string> GetGenreName(int genreId);

        Task<int?> RemoveGenreFromMovie(int id);
    }
}
