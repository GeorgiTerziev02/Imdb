namespace Imdb.Services.Data
{
    using System.Linq;
    using System.Threading.Tasks;

    using Imdb.Data.Common.Repositories;
    using Imdb.Data.Models;
    using Imdb.Services.Data.Contracts;

    public class VotesService : IVotesService
    {
        private readonly IRepository<Vote> votesRepository;

        public VotesService(IRepository<Vote> votesRepository)
        {
            this.votesRepository = votesRepository;
        }

        public async Task VoteAsync(string userId, string movieId, int rating)
        {
            var vote = this.votesRepository.All().FirstOrDefault(x => x.UserId == userId && x.MovieId == movieId);

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

        public double MovieRating(string movieId)
        {
            return this.votesRepository.AllAsNoTracking().Where(x => x.MovieId == movieId).Average(x => x.Rating);
        }

        public int MovieVotesCount(string movieId)
        {
            return this.votesRepository.AllAsNoTracking().Where(x => x.MovieId == movieId).Count();
        }
    }
}
