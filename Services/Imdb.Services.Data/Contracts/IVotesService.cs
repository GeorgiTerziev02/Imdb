namespace Imdb.Services.Data.Contracts
{
    using System.Threading.Tasks;

    public interface IVotesService
    {
        Task VoteAsync(string userId, string movieId, int rating);

        int MovieVotesCount(string movieId);

        double MovieRating(string movieId);
    }
}
