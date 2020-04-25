﻿namespace Imdb.Services.Data.Contracts
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IMoviesService
    {
        IEnumerable<T> GetTopMovies<T>(int count);

        // Task AddMovie(string title, string description, TimeSpan duration, long gross, decimal budget, int languageId, DateTime releaseDate)
        Task<string> AddMovie<T>(T model);

        T GetById<T>(string id);

        IEnumerable<T> GetAll<T>(int page, int itemsPerPage, string sorting);

        bool IsMovieIdValid(string movieId);

        int GetTotalCount();

        IEnumerable<T> Find<T>(string name);

        bool ContainsActor(string movieId, string actorId);

        Task AddActorAsync(string movieId, string actorId);

        IEnumerable<string> NamesSuggestion(string name);

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

        IEnumerable<T> GetTop<T>(int count);

        IEnumerable<T> GetByGenreId<T>(int genreId);

        Task DeleteByIdAsync(string movieId);

        T GetMovieToEdit<T>(string movieId);

        Task EditMovieAsync(string id, string title, string description, long? gross, decimal? budget, string directorId, int languageId, string duration, DateTime? releaseDate, string trailer);
    }
}
