namespace Imdb.Services.Data.Contracts
{
    using System.Threading.Tasks;

    public interface IMovieDataProviderService
    {
        Task<T> GetByTitleAsync<T>(string title, int year)
            where T : class;
    }
}
