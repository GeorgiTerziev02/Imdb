namespace Imdb.Services.Data.Contracts
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Imdb.Data.Common.Repositories;
    using Imdb.Data.Models;
    using Imdb.Data.Models.Enumerations;
    using Imdb.Services.Mapping;
    using Microsoft.EntityFrameworkCore;

    public class DirectorsService : IDirectorsService
    {
        private readonly IDeletableEntityRepository<Director> directorsRepository;

        public DirectorsService(IDeletableEntityRepository<Director> directorsRepository)
        {
            this.directorsRepository = directorsRepository;
        }

        public async Task<string> AddAsync(string firstName, string lastName, Gender gender, DateTime? born, string imageUrl, string description)
        {
            var director = new Director()
            {
                FirstName = firstName,
                LastName = lastName,
                Gender = gender,
                Born = born,
                ImageUrl = imageUrl,
                Description = description,
            };

            await this.directorsRepository.AddAsync(director);
            await this.directorsRepository.SaveChangesAsync();

            return director.Id;
        }

        public async Task<IEnumerable<T>> GetAll<T>()
        {
            return await this.directorsRepository
                .AllAsNoTracking()
                .OrderBy(x => x.FirstName)
                .ThenBy(x => x.LastName)
                .To<T>()
                .ToListAsync();
        }

        public async Task<T> GetById<T>(string directorId)
        {
            return await this.directorsRepository
                .AllAsNoTracking()
                .Where(x => x.Id == directorId)
                .To<T>()
                .FirstOrDefaultAsync();
        }

        public async Task<string> GetId(string name)
        {
            return (await this.directorsRepository
                .All()
                .FirstOrDefaultAsync(x => (x.FirstName + " " + x.LastName) == name))?.Id;
        }
    }
}
