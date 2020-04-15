namespace Imdb.Services.Data.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IMoviesService
    {
        IEnumerable<T> GetTopMovies<T>(int count);

        // Task AddMovie(string title, string description, TimeSpan duration, long gross, decimal budget, int languageId, DateTime releaseDate)
        Task<string> AddMovie<T>(T model);

        T GetById<T>(string id);

        IEnumerable<T> GetAll<T>(int page, int itemsPerPage);

        bool IsMovieIdValid(string movieId);

        int GetTotalCount();

        IEnumerable<T> Find<T>(string name);

        bool ContainsActor(string movieId, string actorId);

        Task AddActorAsync(string movieId, string actorId);
    }
}
