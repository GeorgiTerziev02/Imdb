namespace Imdb.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Imdb.Services.Data.Contracts;
    using Imdb.Services.Mapping;
    using OMDbApiNet;
    using OMDbApiNet.Model;

    public class OmdbService : IMovieDataProviderService
    {
        private readonly AsyncOmdbClient omdb;

        public OmdbService(AsyncOmdbClient omdb)
        {
            this.omdb = omdb;
        }

        public async Task<T> GetByTitleAsync<T>(string title, int year)
            where T : class
        {
            Item item;

            // weird api...
            try
            {
                item = await this.omdb.GetItemByTitleAsync(title, year);
            }
            catch (Exception)
            {
                return null;
            }

            var items = new List<Item>();
            items.Add(item);
            return items.AsQueryable().To<T>().FirstOrDefault();
        }
    }
}
