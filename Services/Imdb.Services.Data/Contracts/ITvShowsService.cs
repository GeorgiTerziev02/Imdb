namespace Imdb.Services.Data.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ITvShowsService
    {
        Task<IEnumerable<T>> GetAll<T>(int skip, int take, string sorting);

        Task<int> GetCount();
    }
}
