﻿namespace Imdb.Services.Data.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Imdb.Data.Common.Repositories;
    using Imdb.Data.Models;
    using Imdb.Services.Data.Tests.TestModels.TvShowsService;
    using Imdb.Services.Mapping;
    using Moq;
    using Xunit;

    public class TvShowsServiceTests
    {
        private readonly Mock<IDeletableEntityRepository<Movie>> tvshowsRepository;

        public TvShowsServiceTests()
        {
            this.tvshowsRepository = new Mock<IDeletableEntityRepository<Movie>>();
            AutoMapperConfig.RegisterMappings(typeof(AllTvShowTestModel).Assembly);
        }

        [Theory]
        [InlineData("fsdfsdfsdfsdfs")]
        [InlineData("a")]
        [InlineData(null)]
        [InlineData("")]
        public void GetAllShouldWorkCorrectly(string sorting)
        {
            var expectedTitle = "a";
            var expectedSecondTitle = "b";

            var expectedCount = 2;
            this.tvshowsRepository.Setup(x => x.All())
                .Returns(new List<Movie>()
                {
                    new Movie() { Id = "1", Title = "a", IsTvShow = true, },
                    new Movie() { Id = "2", Title = "b", IsTvShow = true, },
                    new Movie() { Id = "3", Title = "c", IsTvShow = false, },
                }.AsQueryable<Movie>());
            var tvshowsService = new TvShowsService(this.tvshowsRepository.Object);

            var tvshows = tvshowsService.GetAll<AllTvShowTestModel>(0, 2, sorting).ToList();
            var actualCount = tvshows.Count();

            Assert.Equal(expectedCount, actualCount);
            Assert.Equal(expectedTitle, tvshows[0].Title);
            Assert.Equal(expectedSecondTitle, tvshows[1].Title);
        }

        [Theory]
        [InlineData(2)]
        [InlineData(2444)]
        [InlineData(24342)]
        public void GetAllShouldReturnLessThanWanted(int take)
        {
            var expectedCount = 1;
            this.tvshowsRepository.Setup(x => x.All())
                .Returns(new List<Movie>()
                {
                    new Movie() { Id = "1", Title = "a", IsTvShow = true, },
                    new Movie() { Id = "2", Title = "b", IsTvShow = true, },
                    new Movie() { Id = "3", Title = "c", IsTvShow = false, },
                }.AsQueryable<Movie>());
            var tvshowsService = new TvShowsService(this.tvshowsRepository.Object);

            var tvshows = tvshowsService.GetAll<AllTvShowTestModel>(1, take, string.Empty).ToList();
            var actualCount = tvshows.Count();

            Assert.Equal(expectedCount, actualCount);
        }

        [Fact]
        public void GetAllShouldOrderByTitleDesc()
        {
            var expectedTitle = "b";
            var expectedSecondTitle = "a";
            this.tvshowsRepository.Setup(x => x.All())
                .Returns(new List<Movie>()
                {
                    new Movie() { Id = "1", Title = "a", IsTvShow = true, },
                    new Movie() { Id = "2", Title = "b", IsTvShow = true, },
                    new Movie() { Id = "3", Title = "c", IsTvShow = false, },
                }.AsQueryable<Movie>());
            var tvshowsService = new TvShowsService(this.tvshowsRepository.Object);

            var tvshows = tvshowsService.GetAll<AllTvShowTestModel>(0, 2, "name_desc").ToList();

            Assert.Equal(expectedTitle, tvshows[0].Title);
            Assert.Equal(expectedSecondTitle, tvshows[1].Title);
        }

        [Fact]
        public void GetAllShouldOrderByDate()
        {
            var expectedTitle = "b";
            var expectedSecondTitle = "a";
            this.tvshowsRepository.Setup(x => x.All())
                .Returns(new List<Movie>()
                {
                    new Movie() { Id = "1", Title = "a", IsTvShow = true, ReleaseDate = DateTime.UtcNow.AddDays(-1), },
                    new Movie() { Id = "2", Title = "b", IsTvShow = true, ReleaseDate = DateTime.UtcNow.AddDays(-4), },
                    new Movie() { Id = "3", Title = "c", IsTvShow = false, },
                }.AsQueryable<Movie>());
            var tvshowsService = new TvShowsService(this.tvshowsRepository.Object);

            var tvshows = tvshowsService.GetAll<AllTvShowTestModel>(0, 2, "Date").ToList();

            Assert.Equal(expectedTitle, tvshows[0].Title);
            Assert.Equal(expectedSecondTitle, tvshows[1].Title);
        }

        [Fact]
        public void GetAllShouldOrderByDateDesc()
        {
            var expectedTitle = "b";
            var expectedSecondTitle = "a";
            this.tvshowsRepository.Setup(x => x.All())
                .Returns(new List<Movie>()
                {
                    new Movie() { Id = "1", Title = "a", IsTvShow = true, ReleaseDate = DateTime.UtcNow.AddDays(-4), },
                    new Movie() { Id = "2", Title = "b", IsTvShow = true, ReleaseDate = DateTime.UtcNow.AddDays(-1), },
                    new Movie() { Id = "3", Title = "c", IsTvShow = false, },
                }.AsQueryable<Movie>());
            var tvshowsService = new TvShowsService(this.tvshowsRepository.Object);

            var tvshows = tvshowsService.GetAll<AllTvShowTestModel>(0, 2, "date_desc").ToList();

            Assert.Equal(expectedTitle, tvshows[0].Title);
            Assert.Equal(expectedSecondTitle, tvshows[1].Title);
        }

        [Fact]
        public void GetAllShouldOrderByRating()
        {
            var expectedTitle = "b";
            var expectedSecondTitle = "a";
            this.tvshowsRepository.Setup(x => x.All())
                .Returns(new List<Movie>()
                {
                    new Movie() { Title = "a", IsTvShow = true, Votes = new List<Vote>() { new Vote() { Rating = 4, } } },
                    new Movie() { Title = "b", IsTvShow = true, Votes = new List<Vote>() { new Vote() { Rating = 3, } } },
                }.AsQueryable<Movie>());
            var tvshowsService = new TvShowsService(this.tvshowsRepository.Object);

            var tvshows = tvshowsService.GetAll<AllTvShowTestModel>(0, 2, "Rating").ToList();

            Assert.Equal(expectedTitle, tvshows[0].Title);
            Assert.Equal(expectedSecondTitle, tvshows[1].Title);
        }

        [Fact]
        public void GetAllShouldOrderByRatingDesc()
        {
            var expectedTitle = "b";
            var expectedSecondTitle = "a";
            this.tvshowsRepository.Setup(x => x.All())
                .Returns(new List<Movie>()
                {
                    new Movie() { Title = "a", IsTvShow = true, Votes = new List<Vote>() { new Vote() { Rating = 4, } } },
                    new Movie() { Title = "b", IsTvShow = true, Votes = new List<Vote>() { new Vote() { Rating = 6, } } },
                }.AsQueryable<Movie>());
            var tvshowsService = new TvShowsService(this.tvshowsRepository.Object);

            var tvshows = tvshowsService.GetAll<AllTvShowTestModel>(0, 2, "rating_desc").ToList();

            Assert.Equal(expectedTitle, tvshows[0].Title);
            Assert.Equal(expectedSecondTitle, tvshows[1].Title);
        }

        [Fact]
        public void GetCountShouldWorkCorrectly()
        {
            var expectedCount = 2;
            this.tvshowsRepository.Setup(x => x.AllAsNoTracking())
                .Returns(new List<Movie>()
                {
                    new Movie() { Id = "1", IsTvShow = true, },
                    new Movie() { Id = "2", IsTvShow = true, },
                    new Movie() { Id = "3", IsTvShow = false, },
                }.AsQueryable<Movie>());

            var tvshowsService = new TvShowsService(this.tvshowsRepository.Object);

            var actualCount = tvshowsService.GetCount();

            Assert.Equal(expectedCount, actualCount);
        }

        [Fact]
        public void GetCountShouldReturnZero()
        {
            var expectedCount = 0;
            this.tvshowsRepository.Setup(x => x.AllAsNoTracking())
                .Returns(new List<Movie>()
                {
                    new Movie() { Id = "1", IsTvShow = false, },
                    new Movie() { Id = "2", IsTvShow = false, },
                    new Movie() { Id = "3", IsTvShow = false, },
                }.AsQueryable<Movie>());

            var tvshowsService = new TvShowsService(this.tvshowsRepository.Object);

            var actualCount = tvshowsService.GetCount();

            Assert.Equal(expectedCount, actualCount);
        }
    }
}