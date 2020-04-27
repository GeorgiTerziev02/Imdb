namespace Imdb.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Imdb.Common;
    using Imdb.Data.Common.Repositories;
    using Imdb.Data.Models;
    using Imdb.Services.Data.Contracts;
    using Imdb.Services.Mapping;
    using Microsoft.EntityFrameworkCore;

    public class MoviesService : IMoviesService
    {
        private readonly IDeletableEntityRepository<Movie> moviesRepository;
        private readonly IRepository<MovieActor> movieActorsRepository;
        private readonly IRepository<MovieImage> movieImagesRepository;

        public MoviesService(
            IDeletableEntityRepository<Movie> moviesRepository,
            IRepository<MovieActor> movieActorsRepository,
            IRepository<MovieImage> movieImagesRepository)
        {
            this.moviesRepository = moviesRepository;
            this.movieActorsRepository = movieActorsRepository;
            this.movieImagesRepository = movieImagesRepository;
        }

        public async Task AddActorAsync(string movieId, string actorId)
        {
            var model = new MovieActor()
            {
                ActorId = actorId,
                MovieId = movieId,
            };

            await this.movieActorsRepository.AddAsync(model);
            await this.movieActorsRepository.SaveChangesAsync();
        }

        public async Task<string> AddMovie<T>(T model)
        {
            var newMovie = AutoMapperConfig.MapperInstance.Map<Movie>(model);
            newMovie.IsTvShow = false;
            if (newMovie.Trailer != null)
            {
                newMovie.Trailer = newMovie.Trailer.Replace(GlobalConstants.YoutubeEmbed, string.Empty);
            }

            await this.moviesRepository.AddAsync(newMovie);
            await this.moviesRepository.SaveChangesAsync();

            return newMovie.Id;
        }

        public async Task<bool> ContainsActor(string movieId, string actorId)
        {
            return (await this.movieActorsRepository
                .AllAsNoTracking()
                .Where(x => x.ActorId == actorId && x.MovieId == movieId)
                .FirstOrDefaultAsync()) != null;
        }

        public async Task<bool> ContainsTitle(string title)
        {
            return await this.moviesRepository
                .AllAsNoTracking()
                .AnyAsync(x => x.Title == title);
        }

        public async Task<IEnumerable<T>> Find<T>(string name)
        {
            return await this.moviesRepository
                .AllAsNoTracking()
                .Where(x => x.Title.StartsWith(name))
                .To<T>()
                .ToListAsync();
        }

        public async Task<IEnumerable<string>> NamesSuggestion(string name)
        {
            return await this.moviesRepository
                .AllAsNoTracking()
                .Where(x => x.Title.StartsWith(name))
                .Select(x => x.Title)
                .ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAll<T>(int skip, int itemsPerPage, string sorting)
        {
            return sorting switch
            {
                "name_desc" => await this.moviesRepository
                .All()
                .Where(x => !x.IsTvShow)
                .OrderByDescending(m => m.Title)
                .Skip(skip)
                .Take(itemsPerPage)
                .To<T>()
                .ToListAsync(),
                "Date" => await this.moviesRepository
                .All()
                .Where(x => !x.IsTvShow)
                .OrderBy(m => m.ReleaseDate)
                .Skip(skip)
                .Take(itemsPerPage)
                .To<T>()
                .ToListAsync(),
                "date_desc" => await this.moviesRepository
                .All()
                .Where(x => !x.IsTvShow)
                .OrderByDescending(m => m.ReleaseDate)
                .Skip(skip)
                .Take(itemsPerPage)
                .To<T>()
                .ToListAsync(),
                "rating_desc" => await this.moviesRepository
                .All()
                .Where(x => !x.IsTvShow)
                .OrderByDescending(m => m.Votes.Average(x => x.Rating))
                .ThenByDescending(m => m.Votes.Count())
                .Skip(skip)
                .Take(itemsPerPage)
                .To<T>()
                .ToListAsync(),
                "Rating" => await this.moviesRepository
                .All()
                .Where(x => !x.IsTvShow)
                .OrderBy(m => m.Votes.Average(x => x.Rating))
                .ThenBy(m => m.Votes.Count())
                .Skip(skip)
                .Take(itemsPerPage)
                .To<T>()
                .ToListAsync(),
                _ => await this.moviesRepository
                .All()
                .Where(x => !x.IsTvShow)
                .OrderBy(m => m.Title)
                .Skip(skip)
                .Take(itemsPerPage)
                .To<T>()
                .ToListAsync(),
            };
        }

        public async Task<T> GetById<T>(string id)
        {
            return await this.moviesRepository
                .All()
                .Where(x => x.Id == id)
                .To<T>()
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<T>> GetTopMovies<T>(int count)
        {
            return await this.moviesRepository
                .AllAsNoTracking()
                .Where(x => !x.IsTvShow)
                .OrderByDescending(x => x.Votes.Average(y => y.Rating))
                .ThenByDescending(x => x.Votes.Count())
                .Take(count)
                .To<T>()
                .ToListAsync();
        }

        public async Task<int> GetTotalCount()
        {
            return await this.moviesRepository
                .AllAsNoTracking()
                .Where(x => !x.IsTvShow)
                .CountAsync();
        }

        public async Task<bool> IsMovieIdValid(string movieId)
        {
            return await this.moviesRepository
                .AllAsNoTracking()
                .AnyAsync(x => x.Id == movieId);
        }

        public async Task UploadImages(string movieId, IEnumerable<string> imageUrls)
        {
            foreach (var url in imageUrls)
            {
                var movieImage = new MovieImage()
                {
                    MovieId = movieId,
                    ImageUrl = url,
                };

                await this.movieImagesRepository.AddAsync(movieImage);
            }

            await this.movieImagesRepository.SaveChangesAsync();
        }

        public async Task RemoveMovieActor(int id)
        {
            var movieActor = this.movieActorsRepository
                .All()
                .FirstOrDefault(x => x.Id == id);

            if (movieActor != null)
            {
                this.movieActorsRepository.Delete(movieActor);
                await this.movieActorsRepository.SaveChangesAsync();
            }
        }

        public async Task<string> AddTvShowAsync(
            string title,
            string description,
            TimeSpan? duration,
            DateTime? releaseDate,
            int? episodesCount,
            int languageId,
            string directorId,
            string generalImageUrl,
            string trailer)
        {
            if (trailer != null)
            {
                trailer = trailer.Replace(GlobalConstants.YoutubeEmbed, string.Empty);
            }

            var tvshow = new Movie()
            {
                Title = title,
                Description = description,
                Duration = duration,
                ReleaseDate = releaseDate,
                EpisodesCount = episodesCount,
                LanguageId = languageId,
                DirectorId = directorId,
                GeneralImageUrl = generalImageUrl,
                Trailer = trailer,
                IsTvShow = true,
            };

            await this.moviesRepository.AddAsync(tvshow);
            await this.moviesRepository.SaveChangesAsync();

            return tvshow.Id;
        }

        public async Task<IEnumerable<T>> GetTop<T>(int count)
        {
            return await this.moviesRepository
                .AllAsNoTracking()
                .OrderByDescending(x => x.Votes.Average(y => y.Rating))
                .ThenByDescending(x => x.Votes.Count())
                .Take(count)
                .To<T>()
                .ToListAsync();
        }

        // TODO: Test
        public async Task<IEnumerable<T>> GetByGenreId<T>(int genreId)
        {
            return await this.moviesRepository
                .AllAsNoTracking()
                .Where(x => x.Genres.Any(y => y.GenreId == genreId))
                .To<T>()
                .ToListAsync();
        }

        public async Task DeleteByIdAsync(string movieId)
        {
            var movieActors = await this.movieActorsRepository.All().Where(x => x.MovieId == movieId).ToListAsync();

            foreach (var movieActor in movieActors)
            {
                this.movieActorsRepository.Delete(movieActor);
            }

            await this.movieActorsRepository.SaveChangesAsync();

            var movieImages = await this.movieImagesRepository.All().Where(x => x.MovieId == movieId).ToListAsync();

            foreach (var movieImage in movieImages)
            {
                this.movieImagesRepository.Delete(movieImage);
            }

            await this.movieImagesRepository.SaveChangesAsync();

            var movie = this.moviesRepository
                .All()
                .FirstOrDefault(x => x.Id == movieId);

            this.moviesRepository.Delete(movie);
            await this.moviesRepository.SaveChangesAsync();
        }

        public async Task<T> GetMovieToEdit<T>(string movieId)
        {
            return await this.moviesRepository
                .All()
                .Where(x => x.Id == movieId)
                .To<T>()
                .FirstOrDefaultAsync();
        }

        public async Task EditMovieAsync(
            string id,
            string title,
            string description,
            long? gross,
            decimal? budget,
            string directorId,
            int languageId,
            string duration,
            DateTime? releaseDate,
            string trailer)
        {
            var movie = this.moviesRepository
                .All()
                .FirstOrDefault(x => x.Id == id);

            if (trailer.Contains(GlobalConstants.YoutubeEmbed))
            {
                trailer = trailer.Replace(GlobalConstants.YoutubeEmbed, string.Empty);
            }

            movie.Title = title;
            movie.Description = description;
            movie.Gross = gross;
            movie.Budget = budget;
            movie.DirectorId = directorId;
            movie.LanguageId = languageId;
            movie.Duration = TimeSpan.Parse(duration);
            movie.ReleaseDate = releaseDate;
            movie.Trailer = trailer;

            this.moviesRepository.Update(movie);
            await this.moviesRepository.SaveChangesAsync();
        }
    }
}
