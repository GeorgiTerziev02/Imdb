namespace Imdb.Services.Data.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IGenresService
    {
        IEnumerable<T> GetAll<T>();

        bool MovieContainsGenre(int genreId, string movieId);

        Task AddGenreToMovie(int genreId, string movieId);

        string GetGenreName(int genreId);
    }
}
