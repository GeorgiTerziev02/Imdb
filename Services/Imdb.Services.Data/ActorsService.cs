using Imdb.Data.Common.Repositories;
using Imdb.Data.Models;
using Imdb.Data.Models.Enumerations;
using Imdb.Services.Data.Contracts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Imdb.Services.Data
{
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
    }
}
