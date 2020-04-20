namespace Imdb.Services.Data.Tests
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Imdb.Data;
    using Imdb.Data.Models;
    using Imdb.Data.Repositories;
    using Imdb.Services.Data.Tests.TestModels.LanguageService;
    using Imdb.Services.Mapping;
    using Microsoft.EntityFrameworkCore;
    using Xunit;

    public class LanguageServiceTests
    {
        [Fact]
        public async Task AddLanguageShouldWorkCorrectly()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var repository = new EfRepository<Language>(new ApplicationDbContext(options.Options));
            var service = new LanguageService(repository);
            var expected = 3;

            await service.AddLanguage("a");
            await service.AddLanguage("ab");
            await service.AddLanguage("ac");
            var actual = repository.AllAsNoTracking().Count();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task GetAllReturnsExpectedCount()
        {
            var expected = 100;
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var repository = new EfRepository<Language>(new ApplicationDbContext(options.Options));
            var service = new LanguageService(repository);

            for (int i = 0; i < expected; i++)
            {
                await service.AddLanguage($"{i}");
            }

            AutoMapperConfig.RegisterMappings(typeof(LanguageTestModel).Assembly);
            var languages = service.GetAll<LanguageTestModel>();

            Assert.Equal(expected, languages.Count());
        }

        [Fact]
        public async Task GetAllOrdersLanguages()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var repository = new EfRepository<Language>(new ApplicationDbContext(options.Options));
            var service = new LanguageService(repository);

            for (int i = 25; i >= 0; i--)
            {
                var name = (char)(i + 97);

                await service.AddLanguage(name.ToString());
            }

            AutoMapperConfig.RegisterMappings(typeof(LanguageTestModel).Assembly);
            var languages = service.GetAll<LanguageTestModel>().ToList();
            var firstLangName = languages[0].Name;
            var thirdLangName = languages[2].Name;
            var lastLangName = languages[25].Name;
            Assert.Equal("a", firstLangName);
            Assert.Equal("c", thirdLangName);
            Assert.Equal("z", lastLangName);
        }
    }
}
