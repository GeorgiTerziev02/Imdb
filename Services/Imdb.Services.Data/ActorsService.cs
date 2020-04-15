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

    public class ActorsService : IActorsService
    {
        private readonly IDeletableEntityRepository<Actor> actorsRepository;

        public ActorsService(IDeletableEntityRepository<Actor> actorsRepository)
        {
            this.actorsRepository = actorsRepository;
        }

        public async Task AddAsync(string firstName, string lastName, Gender gender, DateTime? born, string imageUrl, string description)
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
        }

        public IEnumerable<T> GetAll<T>()
        {
            return this.actorsRepository.AllAsNoTracking().OrderBy(x => x.FirstName).ThenBy(x => x.LastName).To<T>().ToList();
        }

        public string GetName(string actorId)
        {
            var actor = this.actorsRepository.AllAsNoTracking().FirstOrDefault(x => x.Id == actorId);
            if (actor == null)
            {
                return null;
            }

            return actor.FirstName + " " + actor.LastName;
        }

        public bool IsActorIdValid(string actorId)
        {
            return this.actorsRepository.AllAsNoTracking().FirstOrDefault(x => x.Id == actorId) != null;
        }
    }
}
