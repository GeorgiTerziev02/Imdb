﻿namespace Imdb.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Imdb.Data.Common.Repositories;
    using Imdb.Data.Models;
    using Imdb.Services.Data.Contracts;
    using Imdb.Services.Mapping;
    using Microsoft.EntityFrameworkCore;

    public class LanguageService : ILanguageService
    {
        private readonly IRepository<Language> languagesRepository;

        public LanguageService(IRepository<Language> languagesRepository)
        {
            this.languagesRepository = languagesRepository;
        }

        public async Task AddLanguage(string name)
        {
            var lang = new Language()
            {
                Name = name,
            };

            await this.languagesRepository.AddAsync(lang);
            await this.languagesRepository.SaveChangesAsync();
        }

        public async Task<int?> GetId(string name)
        {
            return (await this.languagesRepository
                .AllAsNoTracking()
                .FirstOrDefaultAsync(x => x.Name == name))?.Id;
        }

        public async Task<IEnumerable<T>> GetAll<T>()
        {
            return await this.languagesRepository
                .AllAsNoTracking()
                .OrderBy(x => x.Name)
                .To<T>()
                .ToListAsync();
        }
    }
}
