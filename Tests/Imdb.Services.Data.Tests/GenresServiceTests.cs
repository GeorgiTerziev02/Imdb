namespace Imdb.Services.Data.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Imdb.Data;
    using Imdb.Data.Common.Repositories;
    using Imdb.Data.Models;
    using Imdb.Data.Repositories;
    using Imdb.Services.Data.Tests.TestModels.GenresServie;
    using Imdb.Services.Mapping;
    using Microsoft.EntityFrameworkCore;
    using Moq;
    using Xunit;

    public class GenresServiceTests
    {
        private readonly Mock<IRepository<Genre>> genresRepository;
        private readonly Mock<IRepository<MovieGenre>> movieGenresRepository;

        public GenresServiceTests()
        {
            this.genresRepository = new Mock<IRepository<Genre>>();
            this.movieGenresRepository = new Mock<IRepository<MovieGenre>>();
        }

        [Fact]
        public async Task MovieContainsGenreShouldReturnTrue()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                    .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var repository = new EfRepository<MovieGenre>(new ApplicationDbContext(options.Options));
            var service = new GenresService(this.genresRepository.Object, repository);
            await repository.AddAsync(new MovieGenre() { GenreId = 1, MovieId = "1" });
            await repository.SaveChangesAsync();

            var result = await service.MovieContainsGenre(1, "1");
            Assert.True(result);
        }

        [Theory]
        [InlineData(0, "")]
        [InlineData(0, null)]
        [InlineData(0, "2")]
        [InlineData(0, "1")]
        [InlineData(1, "1erwtwetwt")]
        [InlineData(2, "1erwtwetwt")]
        [InlineData(10, "1")]
        [InlineData(10, "2")]
        public async Task MovieContainsGenreShouldReturnFalseAsync(int genreId, string movieId)
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                    .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var repository = new EfRepository<MovieGenre>(new ApplicationDbContext(options.Options));
            var service = new GenresService(this.genresRepository.Object, repository);
            await repository.AddAsync(new MovieGenre() { GenreId = 1, MovieId = "1" });
            await repository.SaveChangesAsync();

            var result = await service.MovieContainsGenre(genreId, movieId);
            Assert.False(result);
        }

        [Fact]
        public async Task GetNameShouldWorkCorrectly()
        {
            var firstExpected = "one";
            var secondExpected = "two";
            var thirdExpected = "three";

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                    .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var repository = new EfRepository<Genre>(new ApplicationDbContext(options.Options));
            var service = new GenresService(repository, this.movieGenresRepository.Object);
            await repository.AddAsync(new Genre() { Id = 1, Name = "one" });
            await repository.AddAsync(new Genre() { Id = 2, Name = "two" });
            await repository.AddAsync(new Genre() { Id = 3, Name = "three" });
            await repository.SaveChangesAsync();

            var actualFirstName = await service.GetGenreName(1);
            var actualSecondName = await service.GetGenreName(2);
            var actualThirdName = await service.GetGenreName(3);

            Assert.Equal(firstExpected, actualFirstName);
            Assert.Equal(secondExpected, actualSecondName);
            Assert.Equal(thirdExpected, actualThirdName);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(10)]
        [InlineData(-10)]
        public async Task GetNameShouldReturnNull(int genreId)
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                    .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var repository = new EfRepository<Genre>(new ApplicationDbContext(options.Options));
            var service = new GenresService(repository, this.movieGenresRepository.Object);
            await repository.AddAsync(new Genre() { Id = 1, Name = "one" });
            await repository.AddAsync(new Genre() { Id = 2, Name = "two" });
            await repository.AddAsync(new Genre() { Id = 3, Name = "three" });
            await repository.SaveChangesAsync();

            var result = await service.GetGenreName(genreId);
            Assert.Null(result);
        }

        [Fact]
        public async Task GetAllShouldWorkCorrectly()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                    .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var repository = new EfRepository<Genre>(new ApplicationDbContext(options.Options));
            var service = new GenresService(repository, this.movieGenresRepository.Object);
            await repository.AddAsync(new Genre() { Id = 1, Name = "one" });
            await repository.AddAsync(new Genre() { Id = 2, Name = "two" });
            await repository.AddAsync(new Genre() { Id = 3, Name = "three" });
            await repository.SaveChangesAsync();

            var expectedCount = 3;

            AutoMapperConfig.RegisterMappings(typeof(GenreAllTestModel).Assembly);
            var genres = await service.GetAll<GenreAllTestModel>();

            Assert.Equal(expectedCount, genres.Count());
        }

        [Fact]
        public async Task GetAllShouldReturnEmptyListAsync()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                    .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var repository = new EfRepository<Genre>(new ApplicationDbContext(options.Options));
            var service = new GenresService(repository, this.movieGenresRepository.Object);

            AutoMapperConfig.RegisterMappings(typeof(GenreAllTestModel).Assembly);
            var genres = await service.GetAll<GenreAllTestModel>();

            Assert.Empty(genres);
        }

        [Fact]
        public async Task AddGenreToMovieShouldWorkCorrectly()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                    .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var repository = new EfRepository<MovieGenre>(new ApplicationDbContext(options.Options));
            var service = new GenresService(this.genresRepository.Object, repository);
            var expectedCount = 2;

            await service.AddGenreToMovie(1, "1");
            await service.AddGenreToMovie(2, "2");
            var actualCount = await repository.AllAsNoTracking().CountAsync();

            Assert.Equal(expectedCount, actualCount);
        }

        [Fact]
        public async Task RemoveGenreFromMovieShouldWorkCorrectly()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                    .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var repository = new EfRepository<MovieGenre>(new ApplicationDbContext(options.Options));
            var service = new GenresService(this.genresRepository.Object, repository);
            var expectedCount = 1;
            var expectedId = 1;

            await repository.AddAsync(new MovieGenre { Id = 1, MovieId = "1", GenreId = 1 });
            await repository.AddAsync(new MovieGenre { Id = 2, MovieId = "111", GenreId = 11 });
            await repository.SaveChangesAsync();

            var returnedId = await service.RemoveGenreFromMovie(1);
            var actualCount = await repository.AllAsNoTracking().CountAsync();

            Assert.Equal(expectedCount, actualCount);
            Assert.Equal(expectedId, returnedId);
        }

        [Theory]
        [InlineData(100)]
        [InlineData(0)]
        [InlineData(-10)]
        public async Task RemoveGenreFromMovieShouldReturnNull(int id)
        {
            this.movieGenresRepository.Setup(x => x.AllAsNoTracking())
                .Returns(new List<MovieGenre>()
                {
                     new MovieGenre() { Id = 1 },
                     new MovieGenre() { Id = 2 },
                     new MovieGenre() { Id = 3 },
                }.AsQueryable<MovieGenre>);

            var genresService = new GenresService(
                this.genresRepository.Object, this.movieGenresRepository.Object);
            var result = await genresService.RemoveGenreFromMovie(id);

            Assert.Null(result);
        }
    }
}
