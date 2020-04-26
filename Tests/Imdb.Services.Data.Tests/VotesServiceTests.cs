namespace Imdb.Services.Data.Tests
{
    using System;
    using System.Threading.Tasks;

    using Imdb.Data;
    using Imdb.Data.Models;
    using Imdb.Data.Repositories;
    using Microsoft.EntityFrameworkCore;
    using Xunit;

    public class VotesServiceTests
    {
        private DbContextOptionsBuilder<ApplicationDbContext> options;
        private EfRepository<Vote> repository;
        private VotesService service;

        public VotesServiceTests()
        {
            this.options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            this.repository = new EfRepository<Vote>(new ApplicationDbContext(this.options.Options));
            this.service = new VotesService(this.repository);
        }

        [Fact]
        public async Task HundredVotesShouldCountOnce()
        {
            for (int i = 1; i <= 100; i++)
            {
                await this.service.VoteAsync("1", "1", i % 11);
            }

            var votesCount = await this.service.MovieVotesCount("1");
            Assert.Equal(1, votesCount);
        }

        [Fact]
        public async Task GetUserVoteShouldWorkCorrectly()
        {
            var expected = 5;

            await this.service.VoteAsync("1", "1", 5);
            var actual = await this.service.GetUserRatingForMovie("1", "1");

            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task UserShouldBeAbleToChangeVote()
        {
            var expected = 8;

            await this.service.VoteAsync("1", "1", 5);
            await this.service.VoteAsync("1", "1", 8);

            var actualVote = await this.service.GetUserRatingForMovie("1", "1");
            Assert.Equal(expected, actualVote);
        }

        [Theory]
        [InlineData(1, 2, 3)]
        [InlineData(6, 7, 6)]
        [InlineData(0, 0, 1)]
        [InlineData(9, 9, 10)]
        [InlineData(0, 0, 0)]
        [InlineData(10, 10, 10)]
        public async Task MovieRatingShouldWorkCorrectly(int firstRating, int secondRating, int thirdRating)
        {
            double expectedRating = (firstRating + secondRating + thirdRating) / 3.0;

            await this.service.VoteAsync("1", "2", firstRating);
            await this.service.VoteAsync("2", "2", secondRating);
            await this.service.VoteAsync("3", "2", thirdRating);

            var actualRating = await this.service.MovieRating("2");

            Assert.Equal(expectedRating, actualRating);
        }

        [Fact]
        public async Task MovieVotesCountShouldWorkCorrectly()
        {
            var expected = 2;

            for (int i = 0; i < 100; i++)
            {
                await this.service.VoteAsync("1", "1", i % 11);
                await this.service.VoteAsync("2", "1", (i + 5) % 11);
            }

            var actual = await this.service.MovieVotesCount("1");

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("2")]
        public async Task GetUserVoteShouldReturnNull(string userId)
        {
            await this.service.VoteAsync("1", "1", 10);
            var actual = await this.service.GetUserRatingForMovie(userId, "1");

            Assert.Null(actual);
        }
    }
}
