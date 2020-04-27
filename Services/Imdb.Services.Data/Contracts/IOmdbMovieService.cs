namespace Imdb.Services.Data.Contracts
{
    using System.Threading.Tasks;

    using Imdb.Web.ViewModels.Admin.Administration;

    public interface IOmdbMovieService
    {
        Task<string> AddMovieFromOmdb(OmdbDataProviderModel model);
    }
}
