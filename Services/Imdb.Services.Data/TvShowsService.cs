﻿namespace Imdb.Services.Data
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

        public IEnumerable<T> GetAll<T>()
        {
            return this.tvshowsRepository.All().Where(x => x.IsTvShow).To<T>().ToList();
        }

        public T GetById<T>(string id)
        {
            return this.tvshowsRepository.All().Where(x => x.IsTvShow).Where(x => x.Id == id).To<T>().FirstOrDefault();
        }
    }
}
