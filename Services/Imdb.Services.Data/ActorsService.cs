namespace Imdb.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using Imdb.Data.Common.Repositories;
    using Imdb.Data.Models;
    using Imdb.Data.Models.Enumerations;
    using Imdb.Services.Data.Contracts;
    using Imdb.Services.Mapping;
    using Microsoft.EntityFrameworkCore;

    public class ActorsService : IActorsService
    {
        private readonly IDeletableEntityRepository<Actor> actorsRepository;

        public ActorsService(IDeletableEntityRepository<Actor> actorsRepository)
        {
            this.actorsRepository = actorsRepository;
        }

        public async Task<string> AddAsync(string firstName, string lastName, Gender gender, DateTime? born, string imageUrl, string description)
        {
            var actor = new Actor()
            {
                FirstName = firstName,
                LastName = lastName,
                Gender = gender,
                Born = born,
                ImageUrl = imageUrl,
                Description = description,
            };

            await this.actorsRepository.AddAsync(actor);
            await this.actorsRepository.SaveChangesAsync();

            return actor.Id;
        }

        public async Task<IEnumerable<T>> GetAll<T>()
        {
            return await this.actorsRepository
                .AllAsNoTracking()
                .OrderBy(x => x.FirstName)
                .ThenBy(x => x.LastName)
                .To<T>()
                .ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAll<T>(int skip, int take)
        {
            return await this.actorsRepository
                .AllAsNoTracking()
                .OrderBy(x => x.FirstName)
                .ThenBy(x => x.LastName)
                .Skip(skip)
                .Take(take)
                .To<T>()
                .ToListAsync();
        }

        public async Task<IEnumerable<T>> GetBornToday<T>(int actorsBornToday)
        {
            var result = await this.actorsRepository
                .AllAsNoTracking()
                .Where(x => x.Born.HasValue)
                .Where(x => x.Born.Value.Month == DateTime.UtcNow.Month)
                .Where(x => x.Born.Value.Day == DateTime.UtcNow.Day)
                .Take(actorsBornToday)
                .To<T>()
                .ToListAsync();

            return result;
        }

        public async Task<T> GetById<T>(string actorId)
        {
            return await this.actorsRepository
                .AllAsNoTracking()
                .Where(x => x.Id == actorId)
                .To<T>()
                .FirstOrDefaultAsync();
        }

        public async Task<string> GetName(string actorId)
        {
            var actor = await this.actorsRepository
                .AllAsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == actorId);

            if (actor == null)
            {
                return null;
            }

            return actor.FirstName + " " + actor.LastName;
        }

        public async Task<int> GetTotalCount()
        {
            return await this.actorsRepository
                .AllAsNoTracking()
                .CountAsync();
        }

        public async Task<bool> IsActorIdValid(string actorId)
        {
            return (await this.actorsRepository
                .AllAsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == actorId)) != null;
        }
    }
}
