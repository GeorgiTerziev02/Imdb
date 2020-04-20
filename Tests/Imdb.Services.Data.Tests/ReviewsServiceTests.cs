namespace Imdb.Services.Data.Tests
{
    using System;
    using System.Threading.Tasks;

    using Imdb.Data;
    using Imdb.Data.Models;
    using Imdb.Data.Repositories;
    using Microsoft.EntityFrameworkCore;
    using Xunit;

    public class ReviewsServiceTests
    {
        [Fact]
        public async Task AddReviewWorksCorrectly()
        {
            var expected = 1;
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var repository = new EfDeletableEntityRepository<Review>(new ApplicationDbContext(options.Options));
            var service = new ReviewsService(repository);

            await service.AddAsync("1", "2", "fsdfsfs");
            var actual = service.UsersReviews("1");

            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task ContainsReviewByIdShouldReturnTrue()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var repository = new EfDeletableEntityRepository<Review>(new ApplicationDbContext(options.Options));
            var service = new ReviewsService(repository);

            var id = await service.AddAsync("1", "1", "sffsf");
            var result = service.ContainsReviewById(id);

            Assert.True(result);
        }

        [Theory]
        [InlineData("1111")]
        [InlineData("")]
        [InlineData(null)]
        public async Task ContainsReviewByIdShouldReturnFalse(string searchedId)
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var repository = new EfDeletableEntityRepository<Review>(new ApplicationDbContext(options.Options));
            var service = new ReviewsService(repository);

            var id = await service.AddAsync("1", "1", "sffsf");
            var result = service.ContainsReviewById(searchedId);

            Assert.False(result);
        }

        [Fact]
        public async Task UsersReviewsCountShouldWorkCorrectly()
        {
            var expected = 3;
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                    .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var repository = new EfDeletableEntityRepository<Review>(new ApplicationDbContext(options.Options));
            var service = new ReviewsService(repository);

            await service.AddAsync("1", "2", "fsdfsdfsdfsfsfs");
            await service.AddAsync("1", "1", "fsdfsdfsdfsfsfs");
            await service.AddAsync("1", "2", "fsdfsdfsdfsfsfs");

            var count = service.UsersReviews("1");
            Assert.Equal(expected, count);
        }

        [Fact]
        public async Task RemoveByIdShouldWorkCorrectly()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                    .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var repository = new EfDeletableEntityRepository<Review>(new ApplicationDbContext(options.Options));
            var service = new ReviewsService(repository);

            var movieId = "2";
            var createdId = await service.AddAsync("1", movieId, "dfsfafasfsfs");
            var deletedId = await service.RemoveById(createdId);

            var contains = service.ContainsReviewById(createdId);

            Assert.False(contains);
        }

        [Fact]
        public async Task RemoveByIdShouldReturnCorrectId()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                    .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var repository = new EfDeletableEntityRepository<Review>(new ApplicationDbContext(options.Options));
            var service = new ReviewsService(repository);

            var movieId = "2";
            var createdId = await service.AddAsync("1", movieId, "dfsfafasfsfs");
            var deletedId = await service.RemoveById(createdId);

            Assert.Equal(movieId, deletedId);
        }

        [Fact]
        public async Task PermissionShouldReturnFalse()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                    .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var repository = new EfDeletableEntityRepository<Review>(new ApplicationDbContext(options.Options));
            var service = new ReviewsService(repository);

            await service.AddAsync("1", "2", "fdsdfsfsfs");
            var result = service.HasPermissionToPost("1");

            Assert.False(result);
        }

        [Fact]
        public void PermissionShouldReturnTrue()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                    .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var repository = new EfDeletableEntityRepository<Review>(new ApplicationDbContext(options.Options));
            var service = new ReviewsService(repository);

            var result = service.HasPermissionToPost("1");

            Assert.True(result);
        }
    }
}
