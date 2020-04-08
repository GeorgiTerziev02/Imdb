namespace Imdb.Services.Data.Contracts
{
    using System.Collections.Generic;
    using System.Text;

    public interface ITvShowsService
    {
        T GetById<T>(string id);

        IEnumerable<T> GetAll<T>();
    }
}
