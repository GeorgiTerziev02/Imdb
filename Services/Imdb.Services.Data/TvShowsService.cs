namespace Imdb.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Imdb.Data.Common.Repositories;
    using Imdb.Data.Models;
    using Imdb.Services.Data.Contracts;
    using Imdb.Services.Mapping;

    public class TvShowsService : ITvShowsService
    {
        private readonly IDeletableEntityRepository<Movie> tvshowsRepository;

        public TvShowsService(IDeletableEntityRepository<Movie> tvshowsRepository)
        {
            this.tvshowsRepository = tvshowsRepository;
        }

        public IEnumerable<T> GetAll<T>(int skip, int take)
        {
            return this.tvshowsRepository
                .All()
                .Where(x => x.IsTvShow)
                .Skip(skip)
                .Take(take)
                .To<T>()
                .ToList();
        }

        public int GetCount()
        {
            return this.tvshowsRepository
                .AllAsNoTracking()
                .Where(x => x.IsTvShow)
                .Count();
        }
    }
}
