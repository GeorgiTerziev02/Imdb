﻿namespace Imdb.Services.Data
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

        public bool ContainsActor(string movieId, string actorId)
        {
            return this.movieActorsRepository
                .AllAsNoTracking()
                .Where(x => x.ActorId == actorId && x.MovieId == movieId)
                .FirstOrDefault() != null;
        }

        public IEnumerable<T> Find<T>(string name)
        {
            return this.moviesRepository
                .AllAsNoTracking()
                .Where(x => x.Title.StartsWith(name))
                .To<T>()
                .ToList();
        }

        public IEnumerable<string> NamesSuggestion(string name)
        {
            return this.moviesRepository
                .AllAsNoTracking()
                .Where(x => x.Title.StartsWith(name))
                .Select(x => x.Title)
                .ToList();
        }

        public IEnumerable<T> GetAll<T>(int skip, int itemsPerPage, string sorting)
        {
            var movies = this.moviesRepository
                .All()
                .Where(x => !x.IsTvShow);

            movies = sorting switch
            {
                "name_desc" => movies.OrderByDescending(m => m.Title),
                "Date" => movies.OrderBy(m => m.ReleaseDate),
                "date_desc" => movies.OrderByDescending(m => m.ReleaseDate),
                "rating_desc" => movies.OrderByDescending(
                    m => m.Votes.Average(x => x.Rating)).ThenByDescending(m => m.Votes.Count()),
                "Rating" => movies.OrderBy(m => m.Votes.Average(x => x.Rating)).ThenBy(m => m.Votes.Count()),
                _ => movies.OrderBy(m => m.Title),
            };
            return movies
                .Skip(skip)
                .Take(itemsPerPage)
                .To<T>()
                .ToList();
        }

        public T GetById<T>(string id)
        {
            return this.moviesRepository
                .All()
                .Where(x => x.Id == id)
                .To<T>()
                .FirstOrDefault();
        }

        public IEnumerable<T> GetTopMovies<T>(int count)
        {
            return this.moviesRepository
                .AllAsNoTracking()
                .Where(x => !x.IsTvShow)
                .OrderByDescending(x => x.Votes.Average(y => y.Rating))
                .ThenByDescending(x => x.Votes.Count())
                .Take(count)
                .To<T>()
                .ToList();
        }

        public int GetTotalCount()
        {
            return this.moviesRepository
                .AllAsNoTracking()
                .Where(x => !x.IsTvShow)
                .Count();
        }

        public bool IsMovieIdValid(string movieId)
        {
            return this.moviesRepository
                .AllAsNoTracking()
                .Any(x => x.Id == movieId);
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

        public IEnumerable<T> GetTop<T>(int count)
        {
            return this.moviesRepository
                .AllAsNoTracking()
                .OrderByDescending(x => x.Votes.Average(y => y.Rating))
                .ThenByDescending(x => x.Votes.Count())
                .Take(count)
                .To<T>()
                .ToList();
        }

        // TODO: Test
        public IEnumerable<T> GetByGenreId<T>(int genreId)
        {
            return this.moviesRepository
                .AllAsNoTracking()
                .Where(x => x.Genres.Any(y => y.GenreId == genreId))
                .To<T>()
                .ToList();
        }

        public async Task DeleteByIdAsync(string movieId)
        {
            var movie = this.moviesRepository
                .All()
                .FirstOrDefault(x => x.Id == movieId);

            this.moviesRepository.Delete(movie);
            await this.moviesRepository.SaveChangesAsync();
        }

        public T GetMovieToEdit<T>(string movieId)
        {
            return this.moviesRepository
                .All()
                .Where(x => x.Id == movieId)
                .To<T>()
                .FirstOrDefault();
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
