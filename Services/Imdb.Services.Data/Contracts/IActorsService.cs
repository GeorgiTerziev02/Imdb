namespace Imdb.Services.Data.Contracts
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Imdb.Data.Models.Enumerations;

    public interface IActorsService
    {
        Task<string> AddAsync(string firstName, string lastName, Gender gender, DateTime? born, string imageUrl, string description);

        IEnumerable<T> GetAll<T>();

        IEnumerable<T> GetAll<T>(int skip, int take);

        bool IsActorIdValid(string actorId);

        string GetName(string actorId);

        T GetById<T>(string actorId);

        int GetTotalCount();

        IEnumerable<T> GetBornToday<T>(int actorsBornToday);
    }
}
