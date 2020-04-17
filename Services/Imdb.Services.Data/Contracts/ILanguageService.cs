namespace Imdb.Services.Data.Contracts
{
    using System.Collections.Generic;

    public interface ILanguageService
    {
        IEnumerable<T> GetAll<T>();
    }
}
