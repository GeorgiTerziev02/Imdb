﻿namespace Imdb.Services.Data.Contracts
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Imdb.Data.Models.Enumerations;

    public interface IDirectorsService
    {
        Task<string> AddAsync(string firstName, string lastName, Gender gender, DateTime? born, string imageUrl, string description);

        Task<IEnumerable<T>> GetAll<T>();

        Task<T> GetById<T>(string directorId);

        Task<string> GetId(string name);
    }
}
