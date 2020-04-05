using Imdb.Data.Common.Repositories;
using Imdb.Data.Models;
using Imdb.Data.Models.Enumerations;
using System;
using System.Threading.Tasks;

namespace Imdb.Services.Data.Contracts
{
    public class DirectorsService : IDirectorsService
    {
        private readonly IDeletableEntityRepository<Director> directorsRepository;

        public DirectorsService(IDeletableEntityRepository<Director> directorsRepository)
        {
            this.directorsRepository = directorsRepository;
        }

        public async Task AddAsync(string firstName, string middleName, string lastName, Gender gender, DateTime? born, string imageUrl, string description)
        {
            var director = new Director()
            {
                FirstName = firstName,
                MiddleName = middleName,
                LastName = lastName,
                Gender = gender,
                Born = born,
                ImageUrl = imageUrl,
                Description = description,
            };

            await this.directorsRepository.AddAsync(director);
            await this.directorsRepository.SaveChangesAsync();
        }
    }
}
