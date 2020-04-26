namespace Imdb.Services.Data.Contracts
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IMoviesService
    {
        Task<IEnumerable<T>> GetTopMovies<T>(int count);

        Task<string> AddMovieFromOmdb(string title, string description, string type, TimeSpan? duration, int languageId, DateTime? releaseDate, string imgUrl);

        Task<string> AddMovie<T>(T model);

        Task<T> GetById<T>(string id);

        Task<IEnumerable<T>> GetAll<T>(int page, int itemsPerPage, string sorting);

        Task<bool> IsMovieIdValid(string movieId);

        Task<int> GetTotalCount();

        Task<IEnumerable<T>> Find<T>(string name);

        Task<bool> ContainsActor(string movieId, string actorId);

        Task<bool> ContainsTitle(string title);

        Task AddActorAsync(string movieId, string actorId);

        Task<IEnumerable<string>> NamesSuggestion(string name);

        Task UploadImages(string movieId, IEnumerable<string> imageUrls);

        Task RemoveMovieActor(int id);

        Task<string> AddTvShowAsync(
            string title,
            string description,
            TimeSpan? duration,
            DateTime? releaseDate,
            int? episodesCount,
            int languageId,
            string directorId,
            string generalImageUrl,
            string trailer);

        Task<IEnumerable<T>> GetTop<T>(int count);

        Task<IEnumerable<T>> GetByGenreId<T>(int genreId);

        Task DeleteByIdAsync(string movieId);

        Task<T> GetMovieToEdit<T>(string movieId);

        Task EditMovieAsync(string id, string title, string description, long? gross, decimal? budget, string directorId, int languageId, string duration, DateTime? releaseDate, string trailer);
    }
}
