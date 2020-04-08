﻿namespace Imdb.Services.Data.Contracts
{
    using System;
    using System.Threading.Tasks;

    using Imdb.Data.Models.Enumerations;

    public interface IActorsService
    {
        Task AddAsync(string firstName, string lastName, Gender gender, DateTime? born, string imageUrl, string description);
    }
}
