namespace Imdb.Services.Data.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ILanguageService
    {
        Task AddLanguage(string name);

        Task<int?> GetId(string name);

        Task<IEnumerable<T>> GetAll<T>();
    }
}
