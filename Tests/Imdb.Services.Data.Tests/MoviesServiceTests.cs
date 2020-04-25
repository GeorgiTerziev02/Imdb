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
    using Imdb.Services.Data.Tests.TestModels.MoviesService;
    using Imdb.Services.Mapping;

    using Microsoft.EntityFrameworkCore;
    using Moq;
    using Xunit;

    public class MoviesServiceTests : IDisposable
    {
        private Mock<IRepository<MovieActor>> movieActorRepository;
        private Mock<IRepository<MovieImage>> movieImageRepository;
        private Mock<IDeletableEntityRepository<Movie>> movieRepository;

        public MoviesServiceTests()
        {
            this.movieActorRepository = new Mock<IRepository<MovieActor>>();
            this.movieImageRepository = new Mock<IRepository<MovieImage>>();
            this.movieRepository = new Mock<IDeletableEntityRepository<Movie>>();
            AutoMapperConfig.RegisterMappings(typeof(MovieByIdTestModel).Assembly);
            AutoMapperConfig.RegisterMappings(typeof(FindMovieTestModel).Assembly);
        }

        [Fact]
        public async Task AddActorShouldWorkCorrectly()
        {
            var expectedCount = 2;
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var repository = new EfRepository<MovieActor>(new ApplicationDbContext(options.Options));
            var service = new MoviesService(
                this.movieRepository.Object, repository, this.movieImageRepository.Object);

            await service.AddActorAsync("3", "er");
            await service.AddActorAsync("1", "ere");

            var actualtCount = await repository.AllAsNoTracking().CountAsync();

            Assert.Equal(expectedCount, actualtCount);
        }

        [Fact]
        public async Task AddMovieShouldWorkCorrectly()
        {
            var expectedCount = 1;
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var repository = new EfDeletableEntityRepository<Movie>(new ApplicationDbContext(options.Options));
            var service = new MoviesService(
                repository, this.movieActorRepository.Object, this.movieImageRepository.Object);

            var model = new MovieInputTestModel()
            {
                Title = "rere",
                Description = "erer",
                Budget = 1,
                DirectorId = "ff",
                LanguageId = 1,
                Duration = "00:12",
                GeneralImageUrl = "re",
                ReleaseDate = DateTime.UtcNow,
                Gross = 1,
                Trailer = "f",
            };

            var id = await service.AddMovie<MovieInputTestModel>(model);

            var actualtCount = await repository.AllAsNoTracking().CountAsync();
            var contains = repository.AllAsNoTracking().Any(x => x.Id == id);

            Assert.Equal(expectedCount, actualtCount);
            Assert.True(contains);
        }

        [Theory]
        [InlineData("pop", "fff")]
        [InlineData("1", "2")]
        public async Task MovieContainsActorShouldReturnTrueAsync(string movieId, string actorId)
        {
            this.movieActorRepository.Setup(x => x.AllAsNoTracking())
                    .Returns(new List<MovieActor>()
                    {
                            new MovieActor() { MovieId = "pop", ActorId = "fff", },
                            new MovieActor() { MovieId = "1", ActorId = "2", },
                    }.AsQueryable<MovieActor>());
            var movieService = new MoviesService(
                this.movieRepository.Object, this.movieActorRepository.Object, this.movieImageRepository.Object);

            var result = await movieService.ContainsActor(movieId, actorId);

            Assert.True(result);
        }

        [Theory]
        [InlineData("pop", "2")]
        [InlineData("1", "fff")]
        [InlineData("fdsafsdfa", "fasfasf")]
        [InlineData(null, null)]
        [InlineData("", "")]
        [InlineData("", "dfs")]
        public async Task MovieContainsActorShouldReturnFalseAsync(string movieId, string actorId)
        {
            this.movieActorRepository.Setup(x => x.AllAsNoTracking())
                    .Returns(new List<MovieActor>()
                    {
                            new MovieActor() { MovieId = "pop", ActorId = "fff", },
                            new MovieActor() { MovieId = "1", ActorId = "2", },
                    }.AsQueryable<MovieActor>());
            var movieService = new MoviesService(
                this.movieRepository.Object, this.movieActorRepository.Object, this.movieImageRepository.Object);

            var result = await movieService.ContainsActor(movieId, actorId);

            Assert.False(result);
        }

        [Theory]
        [InlineData("abc")]
        [InlineData("a")]
        [InlineData("ab")]
        [InlineData("b")]
        [InlineData("mo")]
        [InlineData("m")]
        [InlineData("momo")]
        public async Task FindShouldWorkCorrectlyAsync(string word)
        {
            int expectedCount = 1;
            this.movieRepository.Setup(x => x.AllAsNoTracking())
                    .Returns(new List<Movie>()
                    {
                            new Movie() { Id = "1", Title = "abc", },
                            new Movie() { Id = "2", Title = "bca", },
                            new Movie() { Id = "3", Title = "momo", },
                    }.AsQueryable<Movie>());
            var movieService = new MoviesService(
                this.movieRepository.Object, this.movieActorRepository.Object, this.movieImageRepository.Object);

            var suggestions = await movieService.Find<FindMovieTestModel>(word);
            var actualCount = suggestions.Count();

            Assert.Equal(expectedCount, actualCount);
        }

        [Theory]
        [InlineData("null1")]
        [InlineData("1z")]
        [InlineData("zddv")]
        [InlineData("rewrgegege")]
        public async Task FindShouldReturnEmptyAsync(string word)
        {
            this.movieRepository.Setup(x => x.AllAsNoTracking())
                    .Returns(new List<Movie>()
                    {
                            new Movie() { Id = "1", Title = "abc", },
                            new Movie() { Id = "2", Title = "bca", },
                            new Movie() { Id = "3", Title = "momo", },
                    }.AsQueryable<Movie>());
            var movieService = new MoviesService(
                this.movieRepository.Object, this.movieActorRepository.Object, this.movieImageRepository.Object);

            var suggestions = await movieService.Find<FindMovieTestModel>(word);

            Assert.Empty(suggestions);
        }

        [Theory]
        [InlineData("a")]
        [InlineData("ab")]
        [InlineData("abc")]
        [InlineData("b")]
        [InlineData("bca")]
        [InlineData("bc")]
        [InlineData("m")]
        [InlineData("mo")]
        [InlineData("mom")]
        [InlineData("momo")]
        public async Task NameSuggestionShouldWorkCorrectlyAsync(string word)
        {
            int expectedCount = 1;
            this.movieRepository.Setup(x => x.AllAsNoTracking())
                    .Returns(new List<Movie>()
                    {
                            new Movie() { Id = "1", Title = "abc", },
                            new Movie() { Id = "2", Title = "bca", },
                            new Movie() { Id = "3", Title = "momo", },
                    }.AsQueryable<Movie>());
            var movieService = new MoviesService(
                this.movieRepository.Object, this.movieActorRepository.Object, this.movieImageRepository.Object);

            var suggestions = await movieService.NamesSuggestion(word);
            var actualCount = suggestions.Count();

            Assert.Equal(expectedCount, actualCount);
        }

        [Theory]
        [InlineData("null")]
        [InlineData("z")]
        [InlineData("zdvdvdfvdv")]
        [InlineData("rewergergergegege")]
        public async Task NameSuggestionShouldReturnEmptyAsync(string word)
        {
            this.movieRepository.Setup(x => x.AllAsNoTracking())
                    .Returns(new List<Movie>()
                    {
                            new Movie() { Id = "1", Title = "abc", },
                            new Movie() { Id = "2", Title = "bca", },
                            new Movie() { Id = "3", Title = "momo", },
                    }.AsQueryable<Movie>());
            var movieService = new MoviesService(
                this.movieRepository.Object, this.movieActorRepository.Object, this.movieImageRepository.Object);

            var suggestions = await movieService.NamesSuggestion(word);

            Assert.Empty(suggestions);
        }

        [Theory]
        [InlineData("fsdfsdfsdfsdfs")]
        [InlineData("a")]
        [InlineData(null)]
        [InlineData("")]
        public async Task GetAllShouldWorkCorrectlyAsync(string sorting)
        {
            var expectedTitle = "a";
            var expectedSecondTitle = "c";
            var expectedCount = 2;
            this.movieRepository.Setup(x => x.All())
                .Returns(new List<Movie>()
                {
                    new Movie() { Id = "1", Title = "a", IsTvShow = false, },
                    new Movie() { Id = "2", Title = "b", IsTvShow = true, },
                    new Movie() { Id = "4", Title = "d", IsTvShow = false, },
                    new Movie() { Id = "3", Title = "c", IsTvShow = false, },
                }.AsQueryable<Movie>());
            var moviesService = new MoviesService(
                this.movieRepository.Object, this.movieActorRepository.Object, this.movieImageRepository.Object);

            var movies = (await moviesService.GetAll<GetAllMovieTestModel>(0, 2, sorting)).ToList();
            var actualCount = movies.Count();

            Assert.Equal(expectedCount, actualCount);
            Assert.Equal(expectedTitle, movies[0].Title);
            Assert.Equal(expectedSecondTitle, movies[1].Title);
        }

        [Fact]
        public async Task GetAllShouldReturnOnlyMoviesAsync()
        {
            var expectedCount = 1;
            this.movieRepository.Setup(x => x.All())
                .Returns(new List<Movie>()
                {
                    new Movie() { Id = "1", Title = "a", IsTvShow = true, },
                    new Movie() { Id = "2", Title = "b", IsTvShow = true, },
                    new Movie() { Id = "4", Title = "d", IsTvShow = true, },
                    new Movie() { Id = "3", Title = "c", IsTvShow = false, },
                }.AsQueryable<Movie>());
            var moviesService = new MoviesService(
                this.movieRepository.Object, this.movieActorRepository.Object, this.movieImageRepository.Object);

            var movies = (await moviesService.GetAll<GetAllMovieTestModel>(0, 2, string.Empty)).ToList();
            var actualCount = movies.Count();

            Assert.Equal(expectedCount, actualCount);
        }

        [Theory]
        [InlineData(4)]
        [InlineData(343)]
        public async Task GetAllShouldReturnLessThanWantedAsync(int take)
        {
            var expectedCount = 2;
            this.movieRepository.Setup(x => x.All())
                .Returns(new List<Movie>()
                {
                    new Movie() { Id = "1", Title = "a", IsTvShow = false, },
                    new Movie() { Id = "2", Title = "b", IsTvShow = false, },
                    new Movie() { Id = "3", Title = "c", IsTvShow = true, },
                }.AsQueryable<Movie>());
            var moviesService = new MoviesService(
                this.movieRepository.Object, this.movieActorRepository.Object, this.movieImageRepository.Object);

            var movies = (await moviesService.GetAll<GetAllMovieTestModel>(0, take, string.Empty)).ToList();
            var actualCount = movies.Count();
            var result = actualCount < take;

            Assert.Equal(expectedCount, actualCount);
            Assert.True(result);
        }

        [Fact]
        public async Task GetAllShouldOrderByTitleDescAsync()
        {
            var expectedTitle = "d";
            var expectedSecondTitle = "c";
            this.movieRepository.Setup(x => x.All())
                .Returns(new List<Movie>()
                {
                    new Movie() { Id = "1", Title = "a", IsTvShow = false, },
                    new Movie() { Id = "2", Title = "b", IsTvShow = true, },
                    new Movie() { Id = "4", Title = "d", IsTvShow = false, },
                    new Movie() { Id = "3", Title = "c", IsTvShow = false, },
                }.AsQueryable<Movie>());
            var moviesService = new MoviesService(
                this.movieRepository.Object, this.movieActorRepository.Object, this.movieImageRepository.Object);

            var movies = (await moviesService.GetAll<GetAllMovieTestModel>(0, 2, "name_desc")).ToList();

            Assert.Equal(expectedTitle, movies[0].Title);
            Assert.Equal(expectedSecondTitle, movies[1].Title);
        }

        [Fact]
        public async Task GetAllShouldOrderByReleaseDateAsync()
        {
            var expectedTitle = "d";
            var expectedSecondTitle = "c";
            this.movieRepository.Setup(x => x.All())
                .Returns(new List<Movie>()
                {
                    new Movie() { Id = "1", Title = "a", IsTvShow = false, ReleaseDate = DateTime.UtcNow.AddDays(100), },
                    new Movie() { Id = "2", Title = "b", IsTvShow = true, ReleaseDate = DateTime.UtcNow, },
                    new Movie() { Id = "3", Title = "c", IsTvShow = false, ReleaseDate = DateTime.UtcNow.AddDays(-100), },
                    new Movie() { Id = "4", Title = "d", IsTvShow = false, ReleaseDate = DateTime.UtcNow.AddDays(-200), },
                }.AsQueryable<Movie>());
            var moviesService = new MoviesService(
                this.movieRepository.Object, this.movieActorRepository.Object, this.movieImageRepository.Object);

            var movies = (await moviesService.GetAll<GetAllMovieTestModel>(0, 2, "Date")).ToList();

            Assert.Equal(expectedTitle, movies[0].Title);
            Assert.Equal(expectedSecondTitle, movies[1].Title);
        }

        [Fact]
        public async Task GetAllShouldOrderByReleaseDateDesc()
        {
            var expectedTitle = "d";
            var expectedSecondTitle = "c";
            this.movieRepository.Setup(x => x.All())
                .Returns(new List<Movie>()
                {
                    new Movie() { Id = "1", Title = "a", IsTvShow = false, ReleaseDate = DateTime.UtcNow.AddDays(-100), },
                    new Movie() { Id = "2", Title = "b", IsTvShow = true, ReleaseDate = DateTime.UtcNow, },
                    new Movie() { Id = "3", Title = "c", IsTvShow = false, ReleaseDate = DateTime.UtcNow.AddDays(100), },
                    new Movie() { Id = "4", Title = "d", IsTvShow = false, ReleaseDate = DateTime.UtcNow.AddDays(200), },
                }.AsQueryable<Movie>());
            var moviesService = new MoviesService(
                this.movieRepository.Object, this.movieActorRepository.Object, this.movieImageRepository.Object);

            var movies = (await moviesService.GetAll<GetAllMovieTestModel>(0, 2, "date_desc")).ToList();

            Assert.Equal(expectedTitle, movies[0].Title);
            Assert.Equal(expectedSecondTitle, movies[1].Title);
        }

        [Fact]
        public async Task GetAllShouldOrderByRatingAsync()
        {
            var expectedTitle = "d";
            var expectedSecondTitle = "c";
            this.movieRepository.Setup(x => x.All())
                .Returns(new List<Movie>()
                {
                    new Movie() { Title = "a", IsTvShow = false, Votes = new List<Vote>() { new Vote() { Rating = 4, }, } },
                    new Movie() { Title = "b", IsTvShow = true, Votes = new List<Vote>() { new Vote() { Rating = 3, }, } },
                    new Movie() { Title = "c", IsTvShow = false, Votes = new List<Vote>() { new Vote() { Rating = 2, }, } },
                    new Movie() { Title = "d", IsTvShow = false, Votes = new List<Vote>() { new Vote() { Rating = 1, }, } },
                }.AsQueryable<Movie>());
            var moviesService = new MoviesService(
                this.movieRepository.Object, this.movieActorRepository.Object, this.movieImageRepository.Object);

            var movies = (await moviesService.GetAll<GetAllMovieTestModel>(0, 2, "Rating")).ToList();

            Assert.Equal(expectedTitle, movies[0].Title);
            Assert.Equal(expectedSecondTitle, movies[1].Title);
        }

        [Fact]
        public async Task GetAllShouldOrderByRatingDescAsync()
        {
            var expectedTitle = "d";
            var expectedSecondTitle = "c";
            this.movieRepository.Setup(x => x.All())
                .Returns(new List<Movie>()
                {
                    new Movie() { Title = "a", IsTvShow = false, Votes = new List<Vote>() { new Vote() { Rating = 7, }, } },
                    new Movie() { Title = "b", IsTvShow = true, Votes = new List<Vote>() { new Vote() { Rating = 8, }, } },
                    new Movie() { Title = "c", IsTvShow = false, Votes = new List<Vote>() { new Vote() { Rating = 9, }, } },
                    new Movie() { Title = "d", IsTvShow = false, Votes = new List<Vote>() { new Vote() { Rating = 10, }, } },
                }.AsQueryable<Movie>());
            var moviesService = new MoviesService(
                this.movieRepository.Object, this.movieActorRepository.Object, this.movieImageRepository.Object);

            var movies = (await moviesService.GetAll<GetAllMovieTestModel>(0, 2, "rating_desc")).ToList();

            Assert.Equal(expectedTitle, movies[0].Title);
            Assert.Equal(expectedSecondTitle, movies[1].Title);
        }

        [Theory]
        [InlineData("4")]
        [InlineData("5")]
        [InlineData("6")]
        public async Task GetByIdShouldWorkCorrectlyAsync(string id)
        {
            AutoMapperConfig.RegisterMappings(typeof(MovieByIdTestModel).Assembly);
            this.movieRepository.Setup(x => x.All())
                    .Returns(new List<Movie>()
                    {
                            new Movie() { Id = "4" },
                            new Movie() { Id = "5" },
                            new Movie() { Id = "6" },
                    }.AsQueryable<Movie>());
            var movieService = new MoviesService(
                this.movieRepository.Object, this.movieActorRepository.Object, this.movieImageRepository.Object);

            var result = await movieService.GetById<MovieByIdTestModel>(id);
            Assert.Equal(id, result.Id);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("fasdgadgaergseg")]
        [InlineData("11")]
        public void GetByIdShouldReturnNull(string id)
        {
            this.movieRepository.Setup(x => x.All())
                    .Returns(new List<Movie>()
                    {
                            new Movie() { Id = "1" },
                            new Movie() { Id = "2" },
                            new Movie() { Id = "3" },
                    }.AsQueryable<Movie>());
            var movieService = new MoviesService(
                this.movieRepository.Object, this.movieActorRepository.Object, this.movieImageRepository.Object);

            var result = movieService.GetById<MovieByIdTestModel>(id);
            Assert.Null(result);
        }

        [Fact]
        public void GetTopMoviesShouldWorkCorrectly()
        {
            var expectedCount = 2;
            this.movieRepository.Setup(x => x.AllAsNoTracking())
                .Returns(new List<Movie>()
                {
                    new Movie() { Title = "a", IsTvShow = true, Votes = new List<Vote>() { new Vote() { Rating = 4, } } },
                    new Movie() { Title = "c", IsTvShow = false, Votes = new List<Vote>() { new Vote() { Rating = 6, } } },
                    new Movie() { Title = "d", IsTvShow = true, Votes = new List<Vote>() { new Vote() { Rating = 3, } } },
                    new Movie() { Title = "b", IsTvShow = false, Votes = new List<Vote>() { new Vote() { Rating = 7, } } },
                    new Movie() { Title = "f", IsTvShow = false, Votes = new List<Vote>() { new Vote() { Rating = 9, } } },
                }.AsQueryable<Movie>());
            var movieService = new MoviesService(
                this.movieRepository.Object, this.movieActorRepository.Object, this.movieImageRepository.Object);

            var movies = movieService.GetTopMovies<TopMovieTestModel>(2);
            var actualCount = 2;
            Assert.Equal(expectedCount, actualCount);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(5)]
        [InlineData(10)]
        public async Task GetTopMoviesShouldReturnEmptyAsync(int count)
        {
            this.movieRepository.Setup(x => x.AllAsNoTracking())
                .Returns(new List<Movie>()
                {
                    new Movie() { Title = "a", IsTvShow = true, Votes = new List<Vote>() { new Vote() { Rating = 4, } } },
                    new Movie() { Title = "c", IsTvShow = true, Votes = new List<Vote>() { new Vote() { Rating = 6, } } },
                    new Movie() { Title = "d", IsTvShow = true, Votes = new List<Vote>() { new Vote() { Rating = 3, } } },
                    new Movie() { Title = "b", IsTvShow = true, Votes = new List<Vote>() { new Vote() { Rating = 7, } } },
                    new Movie() { Title = "f", IsTvShow = true, Votes = new List<Vote>() { new Vote() { Rating = 9, } } },
                }.AsQueryable<Movie>());
            var movieService = new MoviesService(
                this.movieRepository.Object, this.movieActorRepository.Object, this.movieImageRepository.Object);

            var movies = await movieService.GetTopMovies<TopMovieTestModel>(count);

            Assert.Empty(movies);
        }

        [Fact]
        public async Task GetTopMoviesShouldReturnOrderedListAsync()
        {
            var expectedTitle = "b";
            var expectedSecondTitle = "d";
            this.movieRepository.Setup(x => x.AllAsNoTracking())
                .Returns(new List<Movie>()
                {
                    new Movie() { Title = "a", IsTvShow = false, Votes = new List<Vote>() { new Vote() { Rating = 4, } } },
                    new Movie() { Title = "c", IsTvShow = false, Votes = new List<Vote>() { new Vote() { Rating = 6, } } },
                    new Movie() { Title = "d", IsTvShow = false, Votes = new List<Vote>() { new Vote() { Rating = 9, } } },
                    new Movie() { Title = "b", IsTvShow = false, Votes = new List<Vote>() { new Vote() { Rating = 10, } } },
                    new Movie() { Title = "f", IsTvShow = false, Votes = new List<Vote>() { new Vote() { Rating = 7, } } },
                }.AsQueryable<Movie>());
            var movieService = new MoviesService(
                this.movieRepository.Object, this.movieActorRepository.Object, this.movieImageRepository.Object);

            var movies = (await movieService.GetTopMovies<TopMovieTestModel>(2)).ToList();

            Assert.Equal(expectedTitle, movies[0].Title);
            Assert.Equal(expectedSecondTitle, movies[1].Title);
        }

        [Fact]
        public async Task GetTopMovieShouldOrderByVotesCountAsync()
        {
            var expectedTitle = "a";
            this.movieRepository.Setup(x => x.AllAsNoTracking())
                .Returns(new List<Movie>()
                {
                    new Movie()
                    {
                        Title = "a",
                        IsTvShow = false,
                        Votes = new List<Vote>() { new Vote() { Rating = 10, }, new Vote() { Rating = 8, } },
                    },
                    new Movie()
                    {
                        Title = "c",
                        IsTvShow = false,
                        Votes = new List<Vote>() { new Vote() { Rating = 9, } },
                    },
                }.AsQueryable<Movie>());
            var movieService = new MoviesService(
                this.movieRepository.Object, this.movieActorRepository.Object, this.movieImageRepository.Object);

            var movies = (await movieService.GetTopMovies<TopMovieTestModel>(1)).ToList();

            Assert.Equal(expectedTitle, movies[0].Title);
        }

        [Fact]
        public async Task GetTotalCountShouldWorkCorrectlyAsync()
        {
            int expectedCount = 3;
            this.movieRepository.Setup(x => x.AllAsNoTracking())
                    .Returns(new List<Movie>()
                    {
                            new Movie() { Id = "1", IsTvShow = false, },
                            new Movie() { Id = "2", IsTvShow = false, },
                            new Movie() { Id = "3", IsTvShow = false, },
                    }.AsQueryable<Movie>());
            var movieService = new MoviesService(
                this.movieRepository.Object, this.movieActorRepository.Object, this.movieImageRepository.Object);

            var actualCount = await movieService.GetTotalCount();
            Assert.Equal(expectedCount, actualCount);
        }

        [Fact]
        public async Task GetTotalCountShouldReturnZeroAsync()
        {
            int expectedCount = 0;
            this.movieRepository.Setup(x => x.AllAsNoTracking())
                    .Returns(new List<Movie>()
                    {
                            new Movie() { Id = "1", IsTvShow = true, },
                            new Movie() { Id = "2", IsTvShow = true, },
                            new Movie() { Id = "3", IsTvShow = true, },
                    }.AsQueryable<Movie>());
            var movieService = new MoviesService(
                this.movieRepository.Object, this.movieActorRepository.Object, this.movieImageRepository.Object);

            var actualCount = await movieService.GetTotalCount();
            Assert.Equal(expectedCount, actualCount);
        }

        [Theory]
        [InlineData("1")]
        [InlineData("2")]
        [InlineData("3")]
        public async Task IsMovieValidShouldReturnTrueAsync(string id)
        {
            this.movieRepository.Setup(x => x.AllAsNoTracking())
                    .Returns(new List<Movie>()
                    {
                            new Movie() { Id = "1" },
                            new Movie() { Id = "2" },
                            new Movie() { Id = "3" },
                    }.AsQueryable<Movie>());
            var movieService = new MoviesService(
                this.movieRepository.Object, this.movieActorRepository.Object, this.movieImageRepository.Object);

            var result = await movieService.IsMovieIdValid(id);
            Assert.True(result);
        }

        [Theory]
        [InlineData("")]
        [InlineData("gdgsdgsgsg")]
        [InlineData(null)]
        public async Task IsMovieValidShouldReturnFalseAsync(string id)
        {
            this.movieRepository.Setup(x => x.AllAsNoTracking())
                    .Returns(new List<Movie>()
                    {
                            new Movie() { Id = "1" },
                            new Movie() { Id = "2" },
                            new Movie() { Id = "3" },
                    }.AsQueryable<Movie>());
            var movieService = new MoviesService(
                this.movieRepository.Object, this.movieActorRepository.Object, this.movieImageRepository.Object);

            var result = await movieService.IsMovieIdValid(id);
            Assert.False(result);
        }

        [Fact]
        public async Task UploadImagesShouldWorkCorrectly()
        {
            var expectedCount = 2;
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var repository = new EfRepository<MovieImage>(new ApplicationDbContext(options.Options));
            var service = new MoviesService(
                this.movieRepository.Object, this.movieActorRepository.Object, repository);

            await service.UploadImages("3", new List<string>() { "1", "2", });

            var actualtCount = await repository.AllAsNoTracking().CountAsync();

            Assert.Equal(expectedCount, actualtCount);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public async Task RemoveMovieActorShouldWorkCorretly(int id)
        {
            var expectedCount = 2;
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var repository = new EfRepository<MovieActor>(new ApplicationDbContext(options.Options));
            var service = new MoviesService(this.movieRepository.Object, repository, this.movieImageRepository.Object);

            await repository.AddAsync(new MovieActor() { Id = 1, });
            await repository.AddAsync(new MovieActor() { Id = 2, });
            await repository.AddAsync(new MovieActor() { Id = 3, });
            await repository.SaveChangesAsync();

            await service.RemoveMovieActor(id);

            var actualtCount = await repository.AllAsNoTracking().CountAsync();

            Assert.Equal(expectedCount, actualtCount);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(4)]
        [InlineData(314342)]
        public async Task RemoveMovieActorShouldNotDeleteAnything(int id)
        {
            var expectedCount = 3;
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var repository = new EfRepository<MovieActor>(new ApplicationDbContext(options.Options));
            var service = new MoviesService(this.movieRepository.Object, repository, this.movieImageRepository.Object);

            await repository.AddAsync(new MovieActor() { Id = 1, });
            await repository.AddAsync(new MovieActor() { Id = 2, });
            await repository.AddAsync(new MovieActor() { Id = 3, });
            await repository.SaveChangesAsync();

            await service.RemoveMovieActor(id);

            var actualtCount = await repository.AllAsNoTracking().CountAsync();

            Assert.Equal(expectedCount, actualtCount);
        }

        [Fact]
        public async Task AddTvShowShouldWorkCorrectly()
        {
            var expectedCount = 1;
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var repository = new EfDeletableEntityRepository<Movie>(new ApplicationDbContext(options.Options));
            var service = new MoviesService(
                repository, this.movieActorRepository.Object, this.movieImageRepository.Object);

            var id = await service.AddTvShowAsync(
                "tvshow",
                "fdsfs",
                TimeSpan.Zero,
                DateTime.UtcNow,
                12414,
                1,
                "d",
                "dfs",
                "fsfs");

            var actualCount = await repository.AllAsNoTracking().CountAsync();

            Assert.NotNull(id);
            Assert.Equal(expectedCount, actualCount);
        }

        [Fact]
        public async Task GetTopShouldWorkCorrectlyAsync()
        {
            var expectedCount = 2;
            var expectedTitle = "a";
            var expectedSecondTitle = "c";
            this.movieRepository.Setup(x => x.AllAsNoTracking())
                .Returns(new List<Movie>()
                {
                    new Movie()
                    {
                        Title = "a",
                        IsTvShow = false,
                        Votes = new List<Vote>() { new Vote() { Rating = 10, }, },
                    },
                    new Movie()
                    {
                        Title = "c",
                        IsTvShow = true,
                        Votes = new List<Vote>() { new Vote() { Rating = 9, } },
                    },
                    new Movie()
                    {
                        Title = "b",
                        IsTvShow = true,
                        Votes = new List<Vote>() { new Vote() { Rating = 4, } },
                    },
                }.AsQueryable<Movie>());
            var movieService = new MoviesService(
                this.movieRepository.Object, this.movieActorRepository.Object, this.movieImageRepository.Object);

            var movies = (await movieService.GetTop<GetTopTestModel>(2)).ToList();
            var actualCount = movies.Count();

            Assert.Equal(expectedCount, actualCount);
            Assert.Equal(expectedTitle, movies[0].Title);
            Assert.Equal(expectedSecondTitle, movies[1].Title);
        }

        [Fact]
        public async Task GetTopShouldReturnTvShowsAsync()
        {
            var expectedCount = 2;
            this.movieRepository.Setup(x => x.AllAsNoTracking())
                .Returns(new List<Movie>()
                {
                    new Movie()
                    {
                        Title = "a",
                        IsTvShow = false,
                        Votes = new List<Vote>() { new Vote() { Rating = 10, }, },
                    },
                    new Movie()
                    {
                        Title = "c",
                        IsTvShow = true,
                        Votes = new List<Vote>() { new Vote() { Rating = 9, } },
                    },
                }.AsQueryable<Movie>());
            var movieService = new MoviesService(
                this.movieRepository.Object, this.movieActorRepository.Object, this.movieImageRepository.Object);

            var movies = (await movieService.GetTop<GetTopTestModel>(2)).ToList();
            var actualCount = movies.Count();

            Assert.Equal(expectedCount, actualCount);
        }

        [Fact]
        public async Task GetTopShouldOrderByRatingAsync()
        {
            var expectedTitle = "c";
            var expectedSecondTitle = "a";
            this.movieRepository.Setup(x => x.AllAsNoTracking())
                .Returns(new List<Movie>()
                {
                    new Movie()
                    {
                        Title = "a",
                        IsTvShow = false,
                        Votes = new List<Vote>() { new Vote() { Rating = 9, }, },
                    },
                    new Movie()
                    {
                        Title = "c",
                        IsTvShow = true,
                        Votes = new List<Vote>() { new Vote() { Rating = 10, } },
                    },
                    new Movie()
                    {
                        Title = "b",
                        IsTvShow = true,
                        Votes = new List<Vote>() { new Vote() { Rating = 4, } },
                    },
                }.AsQueryable<Movie>());
            var movieService = new MoviesService(
                this.movieRepository.Object, this.movieActorRepository.Object, this.movieImageRepository.Object);

            var movies = (await movieService.GetTop<GetTopTestModel>(2)).ToList();

            Assert.Equal(expectedTitle, movies[0].Title);
            Assert.Equal(expectedSecondTitle, movies[1].Title);
        }

        [Fact]
        public async Task GetTopShouldOrderByVotesCountAsync()
        {
            var expectedTitle = "c";
            var expectedSecondTitle = "a";
            this.movieRepository.Setup(x => x.AllAsNoTracking())
                .Returns(new List<Movie>()
                {
                    new Movie()
                    {
                        Title = "a",
                        IsTvShow = false,
                        Votes = new List<Vote>() { new Vote() { Rating = 9, }, },
                    },
                    new Movie()
                    {
                        Title = "c",
                        IsTvShow = true,
                        Votes = new List<Vote>() { new Vote() { Rating = 8, }, new Vote() { Rating = 10, } },
                    },
                    new Movie()
                    {
                        Title = "b",
                        IsTvShow = true,
                        Votes = new List<Vote>() { new Vote() { Rating = 4, } },
                    },
                }.AsQueryable<Movie>());
            var movieService = new MoviesService(
                this.movieRepository.Object, this.movieActorRepository.Object, this.movieImageRepository.Object);

            var movies = (await movieService.GetTop<GetTopTestModel>(2)).ToList();

            Assert.Equal(expectedTitle, movies[0].Title);
            Assert.Equal(expectedSecondTitle, movies[1].Title);
        }

        [Fact]
        public async Task GetByGenreIdShouldWorkCorrectlyAsync()
        {
            var expectedCount = 3;
            this.movieRepository.Setup(x => x.AllAsNoTracking())
                .Returns(new List<Movie>()
                {
                    new Movie() { Id = "1", Genres = new List<MovieGenre>() { new MovieGenre() { GenreId = 1, } } },
                    new Movie() { Id = "2", Genres = new List<MovieGenre>() { new MovieGenre() { GenreId = 2, }, new MovieGenre() { GenreId = 1, } } },
                    new Movie() { Id = "4", Genres = new List<MovieGenre>() { new MovieGenre() { GenreId = 2, } } },
                    new Movie() { Id = "3", Genres = new List<MovieGenre>() { new MovieGenre() { GenreId = 1, } } },
                }.AsQueryable<Movie>());
            var moviesService = new MoviesService(
                this.movieRepository.Object, this.movieActorRepository.Object, this.movieImageRepository.Object);

            var result = await moviesService.GetByGenreId<MovieByGenreTestModel>(1);
            var actualCount = result.Count();

            Assert.Equal(expectedCount, actualCount);
        }

        [Theory]
        [InlineData(3)]
        [InlineData(0)]
        [InlineData(33330)]
        public async Task GetByGenreIdShouldReturnEmptyAsync(int id)
        {
            this.movieRepository.Setup(x => x.AllAsNoTracking())
                .Returns(new List<Movie>()
                {
                    new Movie() { Id = "1", Genres = new List<MovieGenre>() { new MovieGenre() { GenreId = 1, } } },
                    new Movie() { Id = "2", Genres = new List<MovieGenre>() { new MovieGenre() { GenreId = 1, } } },
                    new Movie() { Id = "4", Genres = new List<MovieGenre>() { new MovieGenre() { GenreId = 2, } } },
                    new Movie() { Id = "3", Genres = new List<MovieGenre>() { new MovieGenre() { GenreId = 1, } } },
                }.AsQueryable<Movie>());
            var moviesService = new MoviesService(
                this.movieRepository.Object, this.movieActorRepository.Object, this.movieImageRepository.Object);

            var result = await moviesService.GetByGenreId<MovieByGenreTestModel>(id);

            Assert.Empty(result);
        }

        public void Dispose()
        {
            this.movieRepository.Object.Dispose();
            this.movieImageRepository.Object.Dispose();
            this.movieActorRepository.Object.Dispose();
        }
    }
}
