namespace Imdb.Services.Data.Contracts
{
    using System.Collections.Generic;
    using System.Text;

    public interface ITvShowsService
    {
        IEnumerable<T> GetAll<T>();
    }
}
