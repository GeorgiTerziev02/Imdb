namespace Imdb.Services.Data.Tests
{
    using System.Collections.Generic;
    using System.Linq;

    using Imdb.Data.Common.Repositories;
    using Imdb.Data.Models;
    using Moq;
    using Xunit;

    public class GenresServiceTests
    {
        private Mock<IRepository<Genre>> genresRepository;
        private Mock<IRepository<MovieGenre>> movieGenresRepository;

        public GenresServiceTests()
        {
            this.genresRepository = new Mock<IRepository<Genre>>();
            this.movieGenresRepository = new Mock<IRepository<MovieGenre>>();
        }

        [Fact]
        public void MovieContainsGenreShouldReturnTrue()
        {
            this.movieGenresRepository.Setup(x => x.AllAsNoTracking())
                .Returns(new List<MovieGenre>()
                {
                    new MovieGenre() { Id = 1, MovieId = "1", GenreId = 1 },
                    new MovieGenre() { Id = 2, MovieId = "2", GenreId = 2 },
                }.AsQueryable<MovieGenre>());

            var genresService = new GenresService(
                this.genresRepository.Object, this.movieGenresRepository.Object);

            var result = genresService.MovieContainsGenre(1, "1");
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
        public void MovieContainsGenreShouldReturnFalse(int genreId, string movieId)
        {
            this.movieGenresRepository.Setup(x => x.AllAsNoTracking())
                    .Returns(new List<MovieGenre>()
                    {
                        new MovieGenre() { Id = 1, MovieId = "1", GenreId = 1 },
                        new MovieGenre() { Id = 2, MovieId = "2", GenreId = 2 },
                    }.AsQueryable<MovieGenre>());

            var genresService = new GenresService(
                this.genresRepository.Object, this.movieGenresRepository.Object);

            var result = genresService.MovieContainsGenre(genreId, movieId);
            Assert.False(result);
        }

        [Fact]
        public void GetNameShouldWorkCorrectly()
        {
            var firstExpected = "one";
            var secondExpected = "two";
            var thirdExpected = "three";

            this.genresRepository.Setup(x => x.AllAsNoTracking())
                .Returns(new List<Genre>()
                {
                    new Genre() { Id = 1, Name = "one", },
                    new Genre() { Id = 2, Name = "two", },
                    new Genre() { Id = 3, Name = "three", },
                }.AsQueryable<Genre>);

            var genresService = new GenresService(
                this.genresRepository.Object, this.movieGenresRepository.Object);

            Assert.Equal(firstExpected, genresService.GetGenreName(1));
            Assert.Equal(secondExpected, genresService.GetGenreName(2));
            Assert.Equal(thirdExpected, genresService.GetGenreName(3));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(10)]
        [InlineData(-10)]
        public void GetNameShouldReturnNull(int genreId)
        {
            this.genresRepository.Setup(x => x.AllAsNoTracking())
                .Returns(new List<Genre>()
                {
                     new Genre() { Id = 1, Name = "one", },
                     new Genre() { Id = 2, Name = "two", },
                     new Genre() { Id = 3, Name = "three", },
                }.AsQueryable<Genre>);

            var genresService = new GenresService(
                this.genresRepository.Object, this.movieGenresRepository.Object);

            var result = genresService.GetGenreName(genreId);
            Assert.Null(result);
        }
    }
}
