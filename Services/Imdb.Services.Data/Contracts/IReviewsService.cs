namespace Imdb.Services.Data.Contracts
{
    using System.Threading.Tasks;

    public interface IReviewsService
    {
        Task<string> AddAsync(string userId, string movieId, string content);

        Task<bool> ContainsReviewById(string reviewId);

        Task<string> RemoveById(string reviewId);

        Task<bool> HasPermissionToPost(string userId);

        Task<int> UsersReviews(string userId);
    }
}
