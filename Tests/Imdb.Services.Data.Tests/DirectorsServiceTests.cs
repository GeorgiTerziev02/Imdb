namespace Imdb.Services.Data.Tests
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Imdb.Data;
    using Imdb.Data.Models;
    using Imdb.Data.Models.Enumerations;
    using Imdb.Data.Repositories;
    using Imdb.Services.Data.Contracts;
    using Imdb.Services.Data.Tests.TestModels.DirectorsService;
    using Imdb.Services.Mapping;
    using Microsoft.EntityFrameworkCore;
    using Xunit;

    public class DirectorsServiceTests
    {
        private readonly DbContextOptionsBuilder<ApplicationDbContext> options;
        private readonly EfDeletableEntityRepository<Director> repository;
        private readonly DirectorsService service;

        public DirectorsServiceTests()
        {
            this.options = new DbContextOptionsBuilder<ApplicationDbContext>()
                    .UseInMemoryDatabase(Guid.NewGuid().ToString());
            this.repository = new EfDeletableEntityRepository<Director>(new ApplicationDbContext(this.options.Options));
            this.service = new DirectorsService(this.repository);
            AutoMapperConfig.RegisterMappings(typeof(DirectorByIdTestModel).Assembly);
            AutoMapperConfig.RegisterMappings(typeof(AllDirectorTestModel).Assembly);
        }

        [Fact]
        public async Task AddDirectorShouldWorkCorrectly()
        {
            var expectedCount = 1;

            await this.service.AddAsync("ewew", "rwrw", Gender.Male, DateTime.Now, "fsdfs", "fdff");
            var actualCount = await this.repository.AllAsNoTracking().CountAsync();

            Assert.Equal(expectedCount, actualCount);
        }

        [Fact]
        public async Task GetByIdShouldWorkCorrectly()
        {
            var expectedFirstName = "ewew";
            var expectedLastName = "rwrw";

            var id = await this.service.AddAsync("ewew", "rwrw", Gender.Male, DateTime.Now, "fsdfs", "fdff");
            var result = this.service.GetById<DirectorByIdTestModel>(id);

            Assert.Equal(id, result.Id);
            Assert.Equal(expectedFirstName, result.FirstName);
            Assert.Equal(expectedLastName, result.LastName);
        }

        [Theory]
        [InlineData("")]
        [InlineData("ff")]
        [InlineData(null)]
        public async Task GetByIdShouldReturnNull(string id)
        {
            await this.service.AddAsync("ewew", "rwrw", Gender.Male, DateTime.Now, "fsdfs", "fdff");

            var result = this.service.GetById<DirectorByIdTestModel>(id);

            Assert.Null(result);
        }

        [Fact]
        public async Task GetAllShouldWorkCorrectly()
        {
            var expectedCount = 4;

            for (int i = 0; i < 4; i++)
            {
                await this.service.AddAsync("ewew", "rwrw", Gender.Male, DateTime.Now, "fsdfs", "fdff");
            }

            var directors = this.service.GetAll<AllDirectorTestModel>();
            var actualCount = directors.Count();

            Assert.Equal(expectedCount, actualCount);
        }

        [Fact]
        public void GetAllShouldReturnEmptyList()
        {
            var directors = this.service.GetAll<AllDirectorTestModel>();
            Assert.Empty(directors);
        }

        [Fact]
        public async Task GetAllShouldOrderDirectors()
        {
            var expectedFirstName = "a";
            var expectedOtherFirstName = "b";
            await this.service.AddAsync("b", "b", Gender.Male, DateTime.UtcNow, "feee", "fffeeee");
            await this.service.AddAsync("a", "c", Gender.Male, DateTime.UtcNow, "feee", "fffeee");
            await this.service.AddAsync("a", "b", Gender.Male, DateTime.UtcNow, "feee", "fffeee");

            var directors = this.service.GetAll<AllDirectorTestModel>().ToList();

            Assert.Equal(expectedFirstName, directors[0].FirstName);
            Assert.Equal(expectedOtherFirstName, directors[0].LastName);
            Assert.Equal(expectedFirstName, directors[1].FirstName);
            Assert.Equal(expectedOtherFirstName, directors[2].FirstName);
        }
    }
}
