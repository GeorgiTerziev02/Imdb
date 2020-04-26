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
    using Imdb.Services.Data.Tests.TestModels.WatchlistService;
    using Imdb.Services.Mapping;
    using Microsoft.EntityFrameworkCore;
    using MockQueryable.Moq;
    using Moq;
    using Xunit;

    public class WatchlistServiceTests
    {
        private Mock<IDeletableEntityRepository<Movie>> moviesRepository;
        private Mock<IRepository<UserMovie>> watchlistsRepository;

        public WatchlistServiceTests()
        {
            this.moviesRepository = new Mock<IDeletableEntityRepository<Movie>>();
            this.watchlistsRepository = new Mock<IRepository<UserMovie>>();
            AutoMapperConfig.RegisterMappings(typeof(GetAllMovieTestModel).Assembly);
        }

        [Fact]
        public void MostFrequentShouldWorkCorrectly()
        {
            var expected = 7;

            var array = new int[] { 5, 5, 6, 6, 7, 7, 7, 7, 8, 8, 8, 1, 1, 1 };
            var result = WatchlistService.MostFrequent(array, array.Length);

            Assert.Equal(expected, result);
        }

        [Fact]
        public void MostFrequentShouldReturnMinusOne()
        {
            var expected = -1;

            var array = new int[] { };
            var result = WatchlistService.MostFrequent(array, array.Length);

            Assert.Equal(expected, result);
        }

        [Fact]
        public async Task AddToWatchlistShouldWorkCorrectly()
        {
            var expectedCount = 2;
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var repository = new EfRepository<UserMovie>(new ApplicationDbContext(options.Options));
            var service = new WatchlistService(repository, this.moviesRepository.Object);

            await service.AddToWatchlistAsync("11", "22");
            await service.AddToWatchlistAsync("gg", "gg");
            var actualCount = await repository.AllAsNoTracking().CountAsync();

            Assert.Equal(expectedCount, actualCount);
        }

        [Fact]
        public async Task GetAllShouldWorkCorrectlyAsync()
        {
            var expectedCount = 2;
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var repository = new EfRepository<UserMovie>(new ApplicationDbContext(options.Options));
            var service = new WatchlistService(repository, this.moviesRepository.Object);
            await repository.AddAsync(new UserMovie() { UserId = "1", MovieId = "a" });
            await repository.AddAsync(new UserMovie() { UserId = "2", MovieId = "b", Movie = new Movie() { Title = "p", } });
            await repository.AddAsync(new UserMovie() { UserId = "2", MovieId = "c", Movie = new Movie() { Title = "d", } });
            await repository.SaveChangesAsync();

            var result = await service.GetAll<GetAllMovieTestModel>("2", 0, 2, "Name");
            var actualCount = result.Count();

            Assert.Equal(expectedCount, actualCount);
        }

        [Theory]
        [InlineData("")]
        [InlineData("rerwtwtw")]
        [InlineData(null)]
        public async Task GetAllShouldWorkReturnEmptyAsync(string userId)
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var repository = new EfRepository<UserMovie>(new ApplicationDbContext(options.Options));
            var service = new WatchlistService(repository, this.moviesRepository.Object);
            await repository.AddAsync(new UserMovie() { UserId = "1", MovieId = "a" });
            await repository.AddAsync(new UserMovie() { UserId = "2", MovieId = "b", Movie = new Movie() { Title = "p", } });
            await repository.AddAsync(new UserMovie() { UserId = "2", MovieId = "c", Movie = new Movie() { Title = "d", } });
            await repository.SaveChangesAsync();

            var result = await service.GetAll<GetAllMovieTestModel>(userId, 0, 2, "Name");

            Assert.Empty(result);
        }

        [Fact]
        public async Task GetAllShouldOrderByNameDescAsync()
        {
            var expectedTitle = "p";
            var expectedSecondTitle = "d";
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
    .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var repository = new EfRepository<UserMovie>(new ApplicationDbContext(options.Options));
            var service = new WatchlistService(repository, this.moviesRepository.Object);
            await repository.AddAsync(new UserMovie() { UserId = "1", MovieId = "a" });
            await repository.AddAsync(new UserMovie() { UserId = "2", MovieId = "b", Movie = new Movie() { Title = "p", } });
            await repository.AddAsync(new UserMovie() { UserId = "2", MovieId = "c", Movie = new Movie() { Title = "d", } });
            await repository.AddAsync(new UserMovie() { UserId = "2", MovieId = "r", Movie = new Movie() { Title = "a", } });
            await repository.SaveChangesAsync();

            var result = (await service.GetAll<GetAllMovieTestModel>("2", 0, 3, "name_desc")).ToList();

            Assert.Equal(result[0].MovieTitle, expectedTitle);
            Assert.Equal(result[1].MovieTitle, expectedSecondTitle);
        }

        [Fact]
        public async Task GetAllShouldOrderByReleaseDateAsync()
        {
            var expectedTitle = "c";
            var expectedSecondTitle = "b";

            var userMovies = new List<UserMovie>()
                    {
                            new UserMovie()
                            {
                                UserId = "1",
                                MovieId = "a",
                            },
                            new UserMovie()
                            {
                                UserId = "2",
                                MovieId = "b",
                                Movie = new Movie()
                                {
                                    Title = "a",
                                    ReleaseDate = DateTime.UtcNow.AddDays(-100),
                                },
                            },
                            new UserMovie()
                            {
                                UserId = "2",
                                MovieId = "x",
                                Movie = new Movie()
                                {
                                    Title = "b",
                                    ReleaseDate = DateTime.UtcNow.AddDays(-200),
                                },
                            },
                            new UserMovie()
                            {
                                UserId = "2",
                                MovieId = "p",
                                Movie = new Movie()
                                {
                                    Title = "c",
                                    ReleaseDate = DateTime.UtcNow.AddDays(-300),
                                },
                            },
                    };
            var userMoviesMoq = userMovies.AsQueryable().BuildMock();
            this.watchlistsRepository.Setup(x => x.All())
                    .Returns(userMoviesMoq.Object);
            var service = new WatchlistService(this.watchlistsRepository.Object, this.moviesRepository.Object);

            var result = (await service.GetAll<GetAllMovieTestModel>("2", 0, 3, "Date")).ToList();

            Assert.Equal(result[0].MovieTitle, expectedTitle);
            Assert.Equal(result[1].MovieTitle, expectedSecondTitle);
        }

        [Fact]
        public async Task GetAllShouldOrderByReleaseDateDescAsync()
        {
            var expectedTitle = "a";
            var expectedSecondTitle = "b";

            var userMovies = new List<UserMovie>()
                    {
                            new UserMovie()
                            {
                                UserId = "1",
                                MovieId = "a",
                            },
                            new UserMovie()
                            {
                                UserId = "2",
                                MovieId = "b",
                                Movie = new Movie()
                                {
                                    Title = "a",
                                    ReleaseDate = DateTime.UtcNow.AddDays(-100),
                                },
                            },
                            new UserMovie()
                            {
                                UserId = "2",
                                MovieId = "x",
                                Movie = new Movie()
                                {
                                    Title = "b",
                                    ReleaseDate = DateTime.UtcNow.AddDays(-200),
                                },
                            },
                            new UserMovie()
                            {
                                UserId = "2",
                                MovieId = "p",
                                Movie = new Movie()
                                {
                                    Title = "c",
                                    ReleaseDate = DateTime.UtcNow.AddDays(-300),
                                },
                            },
                    };
            var userMoviesMock = userMovies.AsQueryable().BuildMock();
            this.watchlistsRepository.Setup(x => x.All())
                    .Returns(userMoviesMock.Object);
            var service = new WatchlistService(this.watchlistsRepository.Object, this.moviesRepository.Object);

            var result = (await service.GetAll<GetAllMovieTestModel>("2", 0, 3, "date_desc")).ToList();

            Assert.Equal(result[0].MovieTitle, expectedTitle);
            Assert.Equal(result[1].MovieTitle, expectedSecondTitle);
        }

        [Fact]
        public async Task GetAllShouldOrderByRatingAsync()
        {
            var expectedTitle = "a";
            var expectedSecondTitle = "c";

            var userMovies = new List<UserMovie>()
                    {
                            new UserMovie()
                            {
                                UserId = "1",
                                MovieId = "a",
                            },
                            new UserMovie()
                            {
                                UserId = "2",
                                MovieId = "b",
                                Movie = new Movie()
                                {
                                    Title = "a",
                                    Votes = new List<Vote>()
                                    {
                                        new Vote() { Rating = 6, },
                                    },
                                },
                            },
                            new UserMovie()
                            {
                                UserId = "2",
                                MovieId = "x",
                                Movie = new Movie()
                                {
                                    Title = "b",
                                    Votes = new List<Vote>()
                                    {
                                        new Vote() { Rating = 10, },
                                    },
                                },
                            },
                            new UserMovie()
                            {
                                UserId = "2",
                                MovieId = "p",
                                Movie = new Movie()
                                {
                                    Title = "c",
                                    Votes = new List<Vote>()
                                    {
                                        new Vote() { Rating = 8, },
                                    },
                                },
                            },
                    };
            var userMoviesMock = userMovies.AsQueryable().BuildMock();
            this.watchlistsRepository.Setup(x => x.All())
                    .Returns(userMoviesMock.Object);
            var service = new WatchlistService(this.watchlistsRepository.Object, this.moviesRepository.Object);

            var result = (await service.GetAll<GetAllMovieTestModel>("2", 0, 3, "Rating")).ToList();

            Assert.Equal(result[0].MovieTitle, expectedTitle);
            Assert.Equal(result[1].MovieTitle, expectedSecondTitle);
        }

        [Fact]
        public async Task GetAllShouldOrderByRatingDescAsync()
        {
            var expectedTitle = "b";
            var expectedSecondTitle = "c";

            var userMovies = new List<UserMovie>()
                    {
                            new UserMovie()
                            {
                                UserId = "1",
                                MovieId = "a",
                            },
                            new UserMovie()
                            {
                                UserId = "2",
                                MovieId = "b",
                                Movie = new Movie()
                                {
                                    Title = "a",
                                    Votes = new List<Vote>()
                                    {
                                        new Vote() { Rating = 6, },
                                    },
                                },
                            },
                            new UserMovie()
                            {
                                UserId = "2",
                                MovieId = "x",
                                Movie = new Movie()
                                {
                                    Title = "b",
                                    Votes = new List<Vote>()
                                    {
                                        new Vote() { Rating = 10, },
                                    },
                                },
                            },
                            new UserMovie()
                            {
                                UserId = "2",
                                MovieId = "p",
                                Movie = new Movie()
                                {
                                    Title = "c",
                                    Votes = new List<Vote>()
                                    {
                                        new Vote() { Rating = 8, },
                                    },
                                },
                            },
                    };
            var userMoviesMock = userMovies.AsQueryable().BuildMock();
            this.watchlistsRepository.Setup(x => x.All())
                    .Returns(userMoviesMock.Object);
            var service = new WatchlistService(this.watchlistsRepository.Object, this.moviesRepository.Object);

            var result = (await service.GetAll<GetAllMovieTestModel>("2", 0, 3, "rating_desc")).ToList();

            Assert.Equal(result[0].MovieTitle, expectedTitle);
            Assert.Equal(result[1].MovieTitle, expectedSecondTitle);
        }

        [Fact]
        public async Task RemoveFromWatchlistShouldWorkCorrectly()
        {
            var expectedCount = 1;
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var repository = new EfRepository<UserMovie>(new ApplicationDbContext(options.Options));
            var service = new WatchlistService(repository, this.moviesRepository.Object);

            await repository.AddAsync(new UserMovie() { UserId = "11", MovieId = "22" });
            await repository.AddAsync(new UserMovie() { UserId = "gg", MovieId = "gg" });
            await repository.SaveChangesAsync();

            await service.RemoveFromWatchlistAsync("11", "22");
            var actualCount = await repository.AllAsNoTracking().CountAsync();

            Assert.Equal(expectedCount, actualCount);
        }

        [Theory]
        [InlineData("1", "a")]
        [InlineData("2", "b")]
        [InlineData("3", "c")]
        public async Task WatchlistMovieExistsShouldReturnTrueAsync(string userId, string movieId)
        {
            var userMovies = new List<UserMovie>()
                    {
                            new UserMovie() { UserId = "1", MovieId = "a" },
                            new UserMovie() { UserId = "2", MovieId = "b" },
                            new UserMovie() { UserId = "3", MovieId = "c" },
                    };
            var userMoviesMock = userMovies.AsQueryable().BuildMock();
            this.watchlistsRepository.Setup(x => x.AllAsNoTracking())
                    .Returns(userMoviesMock.Object);
            var service = new WatchlistService(this.watchlistsRepository.Object, this.moviesRepository.Object);

            var result = await service.WatchlistMovieExists(userId, movieId);

            Assert.True(result);
        }

        [Theory]
        [InlineData("1", "b")]
        [InlineData("2", "c")]
        [InlineData("3", "a")]
        [InlineData("", "")]
        [InlineData("", null)]
        public async Task WatchlistMovieExistsShouldReturnFalseAsync(string userId, string movieId)
        {
            var userMovies = new List<UserMovie>()
                    {
                            new UserMovie() { UserId = "1", MovieId = "a" },
                            new UserMovie() { UserId = "2", MovieId = "b" },
                            new UserMovie() { UserId = "3", MovieId = "c" },
                    };
            var userMoviesMock = userMovies.AsQueryable().BuildMock();
            this.watchlistsRepository.Setup(x => x.AllAsNoTracking())
                    .Returns(userMoviesMock.Object);
            var service = new WatchlistService(this.watchlistsRepository.Object, this.moviesRepository.Object);

            var result = await service.WatchlistMovieExists(userId, movieId);

            Assert.False(result);
        }

        [Fact]
        public async Task GetCountShouldWorkCorrectlyAsync()
        {
            var expectedCount = 2;
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                    .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var repository = new EfRepository<UserMovie>(new ApplicationDbContext(options.Options));
            var service = new WatchlistService(repository, this.moviesRepository.Object);
            await repository.AddAsync(new UserMovie() { UserId = "1", MovieId = "a" });
            await repository.AddAsync(new UserMovie() { UserId = "2", MovieId = "b", Movie = new Movie() { Title = "p", } });
            await repository.AddAsync(new UserMovie() { UserId = "2", MovieId = "c", Movie = new Movie() { Title = "d", } });
            await repository.SaveChangesAsync();

            var actualCount = await service.GetCount("2");

            Assert.Equal(expectedCount, actualCount);
        }

        [Fact]
        public async Task GetCountShouldReturnZeroAsync()
        {
            var expectedCount = 0; 
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
         .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var repository = new EfRepository<UserMovie>(new ApplicationDbContext(options.Options));
            var service = new WatchlistService(repository, this.moviesRepository.Object);
            await repository.AddAsync(new UserMovie() { UserId = "1", MovieId = "a" });
            await repository.AddAsync(new UserMovie() { UserId = "3", MovieId = "b" });
            await repository.AddAsync(new UserMovie() { UserId = "3", MovieId = "c" });
            await repository.SaveChangesAsync();

            var actualCount = await service.GetCount("2");

            Assert.Equal(expectedCount, actualCount);
        }

        [Fact]
        public async Task GetMostWatchGenreIdShouldWorkCorrectlyAsync()
        {
            var expected = 1;

            var userMovies = new List<UserMovie>()
                    {
                            new UserMovie() { UserId = "1", MovieId = "a" },
                            new UserMovie()
                            {
                                UserId = "3",
                                MovieId = "b",
                                Movie = new Movie()
                                {
                                    Genres = new List<MovieGenre>()
                                    {
                                        new MovieGenre() { GenreId = 1 },
                                    },
                                },
                            },
                            new UserMovie()
                            {
                                UserId = "3",
                                MovieId = "b",
                                Movie = new Movie()
                                {
                                    Genres = new List<MovieGenre>()
                                    {
                                        new MovieGenre() { GenreId = 1 },
                                        new MovieGenre() { GenreId = 2 },
                                    },
                                },
                            },
                    };
            var mock = userMovies.AsQueryable().BuildMock();
            this.watchlistsRepository.Setup(x => x.AllAsNoTracking())
                .Returns(mock.Object);
            var service = new WatchlistService(this.watchlistsRepository.Object, this.moviesRepository.Object);

            var actual = await service.MostWatchedGenreId("3");

            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task GetMostWatchGenreIdShouldReturnMinusOneAsync()
        {
            var expected = -1;
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
          .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var repository = new EfRepository<UserMovie>(new ApplicationDbContext(options.Options));
            var service = new WatchlistService(repository, this.moviesRepository.Object);
            await repository.AddAsync(new UserMovie() { UserId = "1", MovieId = "a" });
            await repository.AddAsync(new UserMovie() { UserId = "3", MovieId = "b" });
            await repository.AddAsync(new UserMovie() { UserId = "3", MovieId = "c" });
            await repository.SaveChangesAsync();

            var actual = await service.MostWatchedGenreId("2");

            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task RecommendShouldWorkCorrectlyAsync()
        {
            var expectedCount = 2;

            var userMovies = new List<UserMovie>()
                    {
                            new UserMovie()
                            {
                                UserId = "2",
                                MovieId = "b",
                                Movie = new Movie()
                                {
                                    Title = "a",
                                    Genres = new List<MovieGenre>()
                                    {
                                        new MovieGenre() { GenreId = 1, },
                                    },
                                },
                            },
                            new UserMovie()
                            {
                                UserId = "2",
                                MovieId = "x",
                                Movie = new Movie()
                                {
                                    Title = "b",
                                    Genres = new List<MovieGenre>()
                                    {
                                        new MovieGenre() { GenreId = 1, },
                                    },
                                },
                            },
                            new UserMovie()
                            {
                                UserId = "2",
                                MovieId = "p",
                                Movie = new Movie()
                                {
                                    Title = "c",
                                    Genres = new List<MovieGenre>()
                                    {
                                        new MovieGenre() { GenreId = 1, },
                                    },
                                },
                            },
                    };
            var userMoviesMock = userMovies.AsQueryable().BuildMock();
            this.watchlistsRepository.Setup(x => x.AllAsNoTracking())
                    .Returns(userMoviesMock.Object);

            var movies = new List<Movie>()
                    {
                            new Movie()
                            {
                                Title = "m",
                                Genres = new List<MovieGenre>()
                                    {
                                        new MovieGenre() { GenreId = 1, },
                                    },
                                Votes = new List<Vote>()
                                {
                                    new Vote() { Rating = 10, },
                                },
                            },
                            new Movie()
                            {
                                Title = "n",
                                Genres = new List<MovieGenre>()
                                    {
                                        new MovieGenre() { GenreId = 1, },
                                    },
                                Votes = new List<Vote>()
                                {
                                    new Vote() { Rating = 10, },
                                },
                            },
                    };
            var moviesMock = movies.AsQueryable().BuildMock();
            this.moviesRepository.Setup(x => x.AllAsNoTracking())
                    .Returns(moviesMock.Object);
            var service = new WatchlistService(this.watchlistsRepository.Object, this.moviesRepository.Object);

            var result = (await service.Recommend<RecommendMovieTestModel>("2", 1, 2)).ToList();
            var actualCount = result.Count();

            Assert.Equal(expectedCount, actualCount);
            Assert.NotEmpty(result);
        }

        [Fact]
        public async Task RecommendShouldNotRecommendSameMoviesAndReturnEmptyListAsync()
        {
            var userMovies = new List<UserMovie>()
                    {
                            new UserMovie()
                            {
                                UserId = "2",
                                MovieId = "b",
                                Movie = new Movie()
                                {
                                    Title = "a",
                                    Genres = new List<MovieGenre>()
                                    {
                                        new MovieGenre() { GenreId = 1, },
                                    },
                                },
                            },
                            new UserMovie()
                            {
                                UserId = "2",
                                MovieId = "x",
                                Movie = new Movie()
                                {
                                    Title = "b",
                                    Genres = new List<MovieGenre>()
                                    {
                                        new MovieGenre() { GenreId = 1, },
                                    },
                                },
                            },
                            new UserMovie()
                            {
                                UserId = "2",
                                MovieId = "p",
                                Movie = new Movie()
                                {
                                    Title = "c",
                                    Genres = new List<MovieGenre>()
                                    {
                                        new MovieGenre() { GenreId = 1, },
                                    },
                                },
                            },
                    };
            var userMoviesMock = userMovies.AsQueryable().BuildMock();
            this.watchlistsRepository.Setup(x => x.AllAsNoTracking())
                    .Returns(userMoviesMock.Object);

            var movies = new List<Movie>()
                    {
                            new Movie()
                            {
                                Id = "b",
                                Title = "m",
                                Genres = new List<MovieGenre>()
                                    {
                                        new MovieGenre() { GenreId = 1, },
                                    },
                                Votes = new List<Vote>()
                                {
                                    new Vote() { Rating = 10, },
                                },
                            },
                            new Movie()
                            {
                                Id = "p",
                                Title = "n",
                                Genres = new List<MovieGenre>()
                                    {
                                        new MovieGenre() { GenreId = 1, },
                                    },
                                Votes = new List<Vote>()
                                {
                                    new Vote() { Rating = 10, },
                                },
                            },
                    };
            var moviesMock = movies.AsQueryable().BuildMock();
            this.moviesRepository.Setup(x => x.AllAsNoTracking())
                    .Returns(moviesMock.Object);
            var service = new WatchlistService(this.watchlistsRepository.Object, this.moviesRepository.Object);

            var result = (await service.Recommend<RecommendMovieTestModel>("2", 1, 2)).ToList();

            Assert.Empty(result);
        }

        [Fact]
        public async Task RandomRecommendShouldWorkCorrectlyAsync()
        {
            var expectedCount = 1;

            var userMovies = new List<UserMovie>()
                    {
                            new UserMovie()
                            {
                                UserId = "2",
                                MovieId = "b",
                                Movie = new Movie()
                                {
                                    Title = "a",
                                    Genres = new List<MovieGenre>()
                                    {
                                        new MovieGenre() { GenreId = 1, },
                                    },
                                },
                            },
                    };
            var userMoviesMock = userMovies.AsQueryable().BuildMock();
            this.watchlistsRepository.Setup(x => x.AllAsNoTracking())
                    .Returns(userMoviesMock.Object);

            var movies = new List<Movie>()
                    {
                            new Movie()
                            {
                                Id = "b",
                                Title = "m",
                                Genres = new List<MovieGenre>()
                                    {
                                        new MovieGenre() { GenreId = 1, },
                                    },
                                Votes = new List<Vote>()
                                {
                                    new Vote() { Rating = 10, },
                                },
                            },
                            new Movie()
                            {
                                Title = "n",
                                Genres = new List<MovieGenre>()
                                    {
                                        new MovieGenre() { GenreId = 6, },
                                    },
                                Votes = new List<Vote>()
                                {
                                    new Vote() { Rating = 10, },
                                },
                            },
                    };
            var moviesMock = movies.AsQueryable().BuildMock();
            this.moviesRepository.Setup(x => x.AllAsNoTracking())
                    .Returns(moviesMock.Object);
            var service = new WatchlistService(this.watchlistsRepository.Object, this.moviesRepository.Object);

            var result = (await service.RandomRecommend<RecommendMovieTestModel>("2", 2)).ToList();
            var actualCount = result.Count();

            Assert.Equal(expectedCount, actualCount);
            Assert.NotEmpty(result);
        }
    }
}
