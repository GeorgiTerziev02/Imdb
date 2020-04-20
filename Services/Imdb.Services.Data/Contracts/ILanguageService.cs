namespace Imdb.Services.Data.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ILanguageService
    {
        Task AddLanguage(string name);

        IEnumerable<T> GetAll<T>();
    }
}
