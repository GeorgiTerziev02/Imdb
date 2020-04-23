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

        public IEnumerable<T> GetAll<T>(int skip, int take, string sorting)
        {
            var tvshows = this.tvshowsRepository
                    .All()
                    .Where(x => x.IsTvShow);

            tvshows = sorting switch
            {
                "name_desc" => tvshows.OrderByDescending(m => m.Title),
                "Date" => tvshows.OrderBy(m => m.ReleaseDate),
                "date_desc" => tvshows.OrderByDescending(m => m.ReleaseDate),
                "rating_desc" => tvshows.OrderByDescending(m => m.Votes.Average(x => x.Rating)),
                "Rating" => tvshows.OrderBy(m => m.Votes.Average(x => x.Rating)),
                _ => tvshows.OrderBy(m => m.Title),
            };

            return tvshows
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
