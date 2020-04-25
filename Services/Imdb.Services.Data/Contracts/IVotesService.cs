namespace Imdb.Services.Data.Contracts
{
    using System.Threading.Tasks;

    public interface IVotesService
    {
        Task VoteAsync(string userId, string movieId, int rating);

        Task<int> MovieVotesCount(string movieId);

        Task<double> MovieRating(string movieId);

        Task<int?> GetUserRatingForMovie(string userId, string movieId);
    }
}
