using System.Collections.Generic;
using System.Text;

namespace Imdb.Services.Data.Contracts
{
    public interface ILanguageService
    {
        IEnumerable<T> GetAll<T>();
    }
}
