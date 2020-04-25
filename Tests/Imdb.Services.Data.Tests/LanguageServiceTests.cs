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
        private DbContextOptionsBuilder<ApplicationDbContext> options;
        private EfRepository<Language> repository;
        private LanguageService service;

        public LanguageServiceTests()
        {
            this.options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            this.repository = new EfRepository<Language>(new ApplicationDbContext(this.options.Options));
            this.service = new LanguageService(this.repository);
            AutoMapperConfig.RegisterMappings(typeof(LanguageTestModel).Assembly);
        }

        [Fact]
        public async Task AddLanguageShouldWorkCorrectly()
        {
            var expected = 3;

            await this.service.AddLanguage("a");
            await this.service.AddLanguage("ab");
            await this.service.AddLanguage("ac");
            var actual = this.repository.AllAsNoTracking().Count();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task GetAllReturnsExpectedCount()
        {
            var expected = 100;

            for (int i = 0; i < 100; i++)
            {
                await this.service.AddLanguage($"{i}");
            }

            var languages = await this.service.GetAll<LanguageTestModel>();

            Assert.Equal(expected, languages.Count());
        }

        [Fact]
        public async Task GetAllOrdersLanguages()
        {
            for (int i = 25; i >= 0; i--)
            {
                var name = (char)(i + 97);

                await this.service.AddLanguage(name.ToString());
            }

            var languages = (await this.service.GetAll<LanguageTestModel>()).ToList();
            var firstLangName = languages[0].Name;
            var thirdLangName = languages[2].Name;
            var lastLangName = languages[25].Name;
            Assert.Equal("a", firstLangName);
            Assert.Equal("c", thirdLangName);
            Assert.Equal("z", lastLangName);
        }
    }
}
