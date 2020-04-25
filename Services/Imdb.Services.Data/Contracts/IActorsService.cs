namespace Imdb.Services.Data.Contracts
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Imdb.Data.Models.Enumerations;

    public interface IActorsService
    {
        Task<string> AddAsync(string firstName, string lastName, Gender gender, DateTime? born, string imageUrl, string description);

        Task<IEnumerable<T>> GetAll<T>();

        Task<IEnumerable<T>> GetAll<T>(int skip, int take);

        Task<bool> IsActorIdValid(string actorId);

        Task<string> GetName(string actorId);

        Task<T> GetById<T>(string actorId);

        Task<int> GetTotalCount();

        Task<IEnumerable<T>> GetBornToday<T>(int actorsBornToday);
    }
}
