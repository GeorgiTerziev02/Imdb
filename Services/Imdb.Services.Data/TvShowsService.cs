namespace Imdb.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Imdb.Data.Common.Repositories;
    using Imdb.Data.Models;
    using Imdb.Services.Data.Contracts;
    using Imdb.Services.Mapping;
    using Microsoft.EntityFrameworkCore;

    public class TvShowsService : ITvShowsService
    {
        private readonly IDeletableEntityRepository<Movie> tvshowsRepository;

        public TvShowsService(IDeletableEntityRepository<Movie> tvshowsRepository)
        {
            this.tvshowsRepository = tvshowsRepository;
        }

        public async Task<IEnumerable<T>> GetAll<T>(int skip, int take, string sorting)
        {
            return sorting switch
            {
                "name_desc" => await this.tvshowsRepository
                                   .All()
                                   .Where(x => x.IsTvShow)
                                   .OrderByDescending(x => x.Title)
                                   .Skip(skip)
                                   .Take(take)
                                   .To<T>()
                                   .ToListAsync(),
                "Date" => await this.tvshowsRepository
                                   .All()
                                   .Where(x => x.IsTvShow)
                                   .OrderBy(x => x.ReleaseDate)
                                   .Skip(skip)
                                   .Take(take)
                                   .To<T>()
                                   .ToListAsync(),
                "date_desc" => await this.tvshowsRepository
                                   .All()
                                   .Where(x => x.IsTvShow)
                                   .OrderByDescending(x => x.ReleaseDate)
                                   .Skip(skip)
                                   .Take(take)
                                   .To<T>()
                                   .ToListAsync(),
                "rating_desc" => await this.tvshowsRepository
                                   .All()
                                   .Where(x => x.IsTvShow)
                                   .OrderByDescending(x => x.Votes.Average(x => x.Rating))
                                   .Skip(skip)
                                   .Take(take)
                                   .To<T>()
                                   .ToListAsync(),
                "Rating" => await this.tvshowsRepository
                                   .All()
                                   .Where(x => x.IsTvShow)
                                   .OrderBy(x => x.Votes.Average(x => x.Rating))
                                   .Skip(skip)
                                   .Take(take)
                                   .To<T>()
                                   .ToListAsync(),
                _ => await this.tvshowsRepository
                                   .All()
                                   .Where(x => x.IsTvShow)
                                   .OrderBy(x => x.Title)
                                   .Skip(skip)
                                   .Take(take)
                                   .To<T>()
                                   .ToListAsync(),
            };
        }

        public async Task<int> GetCount()
        {
            return await this.tvshowsRepository
                .AllAsNoTracking()
                .Where(x => x.IsTvShow)
                .CountAsync();
        }
    }
}
