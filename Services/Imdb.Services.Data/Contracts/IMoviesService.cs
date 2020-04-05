using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Imdb.Services.Data.Contracts
{
    public interface IMoviesService
    {
        IEnumerable<T> GetTop5Movies<T>();

        //Task AddMovie(string title, string description, TimeSpan duration, long gross, decimal budget, int languageId, DateTime releaseDate)

        Task AddMovie<T>(T model);
    }
}
