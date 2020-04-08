namespace Imdb.Services.Data.Contracts
{
    using System;
    using System.Threading.Tasks;

    using Imdb.Data.Common.Repositories;
    using Imdb.Data.Models;
    using Imdb.Data.Models.Enumerations;

    public class DirectorsService : IDirectorsService
    {
        private readonly IDeletableEntityRepository<Director> directorsRepository;

        public DirectorsService(IDeletableEntityRepository<Director> directorsRepository)
        {
            this.directorsRepository = directorsRepository;
        }

        public async Task AddAsync(string firstName, string lastName, Gender gender, DateTime? born, string imageUrl, string description)
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
        }
    }
}
