﻿namespace Imdb.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;

    using Imdb.Data.Common.Repositories;
    using Imdb.Data.Models;
    using Imdb.Services.Data.Contracts;
    using Imdb.Services.Mapping;

    public class LanguageService : ILanguageService
    {
        private readonly IRepository<Language> languagesRepository;

        public LanguageService(IRepository<Language> languagesRepository)
        {
            this.languagesRepository = languagesRepository;
        }

        public IEnumerable<T> GetAll<T>()
        {
            return this.languagesRepository.AllAsNoTracking().OrderBy(x => x.Name).To<T>().ToList();
        }
    }
}
