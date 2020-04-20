namespace Imdb.Services.Data.Tests
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Imdb.Data;
    using Imdb.Data.Models;
    using Imdb.Data.Models.Enumerations;
    using Imdb.Data.Repositories;
    using Imdb.Services.Data.Tests.TestModels.ActorsService;
    using Imdb.Services.Mapping;
    using Microsoft.EntityFrameworkCore;
    using Xunit;

    public class ActorsServiceTests
    {
        private DbContextOptionsBuilder<ApplicationDbContext> options;
        private EfDeletableEntityRepository<Actor> repository;
        private ActorsService service;

        public ActorsServiceTests()
        {
            this.options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            this.repository = new EfDeletableEntityRepository<Actor>(new ApplicationDbContext(this.options.Options));
            this.service = new ActorsService(this.repository);
            AutoMapperConfig.RegisterMappings(typeof(AllActorViewModel).Assembly);
            AutoMapperConfig.RegisterMappings(typeof(ActorByIdTestModel).Assembly);
        }

        [Fact]
        public async Task AddActorShouldWorkCorrectly()
        {
            var expected = 1;

            await this.service.AddAsync("Random", "Random", Gender.Male, DateTime.UtcNow, "Random", "Random");
            var actual = await this.repository.AllAsNoTracking().CountAsync();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task GetTotalCountShouldWorkCorrectly()
        {
            var expected = 100;

            for (int i = 0; i < expected; i++)
            {
                await this.service.AddAsync("Random", "Random", Gender.Male, DateTime.UtcNow, "Random", "Random");
            }

            var actual = this.service.GetTotalCount();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetTotalCountShouldReturnZero()
        {
            var expected = 0;

            var actual = this.service.GetTotalCount();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task GetNameShouldWorkCorrectly()
        {
            var expected = "Adam Adams";

            var actorId = await this.service.AddAsync("Adam", "Adams", Gender.Male, DateTime.UtcNow, "Random", "Random");
            var actual = this.service.GetName(actorId);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetNameShouldReturnNull()
        {
            var result = this.service.GetName("1");

            Assert.Null(result);
        }

        [Fact]
        public async Task IsActorValidShouldReturnTrue()
        {
            string id = string.Empty;
            for (int i = 0; i < 100; i++)
            {
                if (i == 55)
                {
                    id = await this.service.AddAsync("Adam", "Adams", Gender.Male, DateTime.UtcNow, "Random", "Random");
                }
                else
                {
                    await this.service.AddAsync("Adam", "Adams", Gender.Male, DateTime.UtcNow, "Random", "Random");
                }
            }

            var result = this.service.IsActorIdValid(id);
            Assert.True(result);
        }

        [Theory]
        [InlineData("")]
        [InlineData("1")]
        [InlineData(null)]
        public async Task IsActorValidShouldReturnFalse(string id)
        {
            for (int i = 0; i < 5; i++)
            {
                await this.service.AddAsync("Adam", "Adams", Gender.Male, DateTime.UtcNow, "Random", "Random");
            }

            var result = this.service.IsActorIdValid(id);

            Assert.False(result);
        }

        [Fact]
        public async Task GetActorByIdShouldWorkCorrectly()
        {
            string expectedFirstName = "Adam";
            string expectedLastName = "Adams";

            var id = await this.service.AddAsync("Adam", "Adams", Gender.Male, DateTime.UtcNow, "Random", "Random");
            var result = this.service.GetById<ActorByIdTestModel>(id);

            Assert.Equal(id, result.Id);
            Assert.Equal(expectedFirstName, result.FirstName);
            Assert.Equal(expectedLastName, result.LastName);
        }

        [Theory]
        [InlineData("")]
        [InlineData("1")]
        [InlineData(null)]
        public void GetActorByIdShouldReturnNull(string id)
        {
            var result = this.service.GetById<ActorByIdTestModel>(id);

            Assert.Null(result);
        }

        [Fact]
        public async Task GetAllShouldWorkCorrectly()
        {
            var expected = 3;
            await this.service.AddAsync("1", "1", Gender.Male, DateTime.UtcNow, "f", "fff");
            await this.service.AddAsync("1", "1", Gender.Male, DateTime.UtcNow, "f", "fff");
            await this.service.AddAsync("1", "1", Gender.Male, DateTime.UtcNow, "f", "fff");
            var actors = this.service.GetAll<AllActorViewModel>().ToList();

            Assert.Equal(expected, actors.Count);
        }

        [Fact]
        public void GetAllShouldReturnNull()
        {
            var actors = this.service.GetAll<AllActorViewModel>();

            Assert.Empty(actors);
        }

        [Fact]
        public async Task GetAllShouldOrder()
        {
            var expectedFirstName = "a";
            var expectedOtherFirstName = "b";
            await this.service.AddAsync("b", "b", Gender.Male, DateTime.UtcNow, "f", "fff");
            await this.service.AddAsync("a", "c", Gender.Male, DateTime.UtcNow, "f", "fff");
            await this.service.AddAsync("a", "b", Gender.Male, DateTime.UtcNow, "f", "fff");

            var actors = this.service.GetAll<AllActorViewModel>().ToList();

            Assert.Equal(expectedFirstName, actors[0].FirstName);
            Assert.Equal(expectedOtherFirstName, actors[0].LastName);
            Assert.Equal(expectedFirstName, actors[1].FirstName);
            Assert.Equal(expectedOtherFirstName, actors[2].FirstName);
        }

        [Fact]
        public async Task GetAllWithParamShouldWorkCorrectly()
        {
            var expectedCount = 5;

            for (int i = 1; i <= 11; i++)
            {
                await this.service.AddAsync($"a", $"a", Gender.Male, DateTime.UtcNow, "f", "fff");
            }

            var actors = this.service.GetAll<AllActorViewModel>(5, 5).ToList();

            Assert.Equal(expectedCount, actors.Count());
        }

        [Fact]
        public async Task GetAllShouldReturnLessThanTaken()
        {
            var expectedCount = 1;

            for (int i = 1; i <= 11; i++)
            {
                await this.service.AddAsync($"a", $"a", Gender.Male, DateTime.UtcNow, "f", "fff");
            }

            var actors = this.service.GetAll<AllActorViewModel>(10, 5).ToList();

            Assert.Equal(expectedCount, actors.Count());
        }
    }
}
