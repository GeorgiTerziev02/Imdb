﻿namespace Imdb.Services.Data.Contracts
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    public interface IMoviesService
    {
        IEnumerable<T> GetTopMovies<T>(int count);

        // Task AddMovie(string title, string description, TimeSpan duration, long gross, decimal budget, int languageId, DateTime releaseDate)
        Task AddMovie<T>(T model);

        T GetById<T>(string id);

        IEnumerable<T> GetAll<T>();

        bool IsMovieIdValid(string movieId);
    }
}
