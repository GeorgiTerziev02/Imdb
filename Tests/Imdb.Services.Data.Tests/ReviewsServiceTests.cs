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
        private DbContextOptionsBuilder<ApplicationDbContext> options;
        private EfDeletableEntityRepository<Review> repository;
        private ReviewsService service;

        public ReviewsServiceTests()
        {
            this.options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            this.repository = new EfDeletableEntityRepository<Review>(new ApplicationDbContext(this.options.Options));
            this.service = new ReviewsService(this.repository);
        }

        [Fact]
        public async Task AddReviewWorksCorrectly()
        {
            var expected = 1;

            await this.service.AddAsync("1", "2", "fsdfsfs");
            var actual = await this.service.UsersReviews("1");

            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task ContainsReviewByIdShouldReturnTrue()
        {
            var id = await this.service.AddAsync("1", "1", "sffsf");
            var result = await this.service.ContainsReviewById(id);

            Assert.True(result);
        }

        [Theory]
        [InlineData("1111")]
        [InlineData("")]
        [InlineData(null)]
        public async Task ContainsReviewByIdShouldReturnFalse(string searchedId)
        {
            var id = await this.service.AddAsync("1", "1", "sffsf");
            var result = await this.service.ContainsReviewById(searchedId);

            Assert.False(result);
        }

        [Fact]
        public async Task UsersReviewsCountShouldWorkCorrectly()
        {
            var expected = 3;

            await this.service.AddAsync("1", "2", "fsdfsdfsdfsfsfs");
            await this.service.AddAsync("1", "1", "fsdfsdfsdfsfsfs");
            await this.service.AddAsync("1", "2", "fsdfsdfsdfsfsfs");

            var count = await this.service.UsersReviews("1");
            Assert.Equal(expected, count);
        }

        [Fact]
        public async Task RemoveByIdShouldWorkCorrectly()
        {
            var movieId = "2";
            var createdId = await this.service.AddAsync("1", movieId, "dfsfafasfsfs");
            var deletedId = await this.service.RemoveById(createdId);

            var contains = await this.service.ContainsReviewById(createdId);

            Assert.False(contains);
        }

        [Fact]
        public async Task RemoveByIdShouldReturnCorrectId()
        {
            var movieId = "2";
            var createdId = await this.service.AddAsync("1", movieId, "dfsfafasfsfs");
            var deletedId = await this.service.RemoveById(createdId);

            Assert.Equal(movieId, deletedId);
        }

        [Fact]
        public async Task PermissionShouldReturnFalse()
        {
            await this.service.AddAsync("1", "2", "fdsdfsfsfs");
            var result = await this.service.HasPermissionToPost("1");

            Assert.False(result);
        }

        [Fact]
        public async Task PermissionShouldReturnTrueAsync()
        {
            var result = await this.service.HasPermissionToPost("1");

            Assert.True(result);
        }
    }
}
