using System.Collections.Generic;
using System.Text;

namespace Imdb.Services.Data.Contracts
{
    public interface ITvShowsService
    {
        T GetById<T>(string id);

        IEnumerable<T> GetAll<T>();
    }
}
