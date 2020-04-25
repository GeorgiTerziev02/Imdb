namespace Imdb.Services.Data
{
    using System.Linq;
    using System.Threading.Tasks;

    using Imdb.Data.Common.Repositories;
    using Imdb.Data.Models;
    using Imdb.Services.Data.Contracts;
    using Microsoft.EntityFrameworkCore;

    public class VotesService : IVotesService
    {
        private readonly IRepository<Vote> votesRepository;

        public VotesService(IRepository<Vote> votesRepository)
        {
            this.votesRepository = votesRepository;
        }

        public async Task VoteAsync(string userId, string movieId, int rating)
        {
            var vote = this.votesRepository
                .All()
                .FirstOrDefault(x => x.UserId == userId && x.MovieId == movieId);

            if (vote == null)
            {
                vote = new Vote()
                {
                    UserId = userId,
                    MovieId = movieId,
                };

                await this.votesRepository.AddAsync(vote);
            }

            vote.Rating = rating;
            await this.votesRepository.SaveChangesAsync();
        }

        public async Task<double> MovieRating(string movieId)
        {
            return await this.votesRepository
                .AllAsNoTracking()
                .Where(x => x.MovieId == movieId)
                .AverageAsync(x => x.Rating);
        }

        public async Task<int> MovieVotesCount(string movieId)
        {
            return await this.votesRepository
                .AllAsNoTracking()
                .Where(x => x.MovieId == movieId)
                .CountAsync();
        }

        public async Task<int?> GetUserRatingForMovie(string userId, string movieId)
        {
            return (await this.votesRepository
                .AllAsNoTracking()
                .FirstOrDefaultAsync(x => x.UserId == userId && x.MovieId == movieId))?.Rating;
        }
    }
}
