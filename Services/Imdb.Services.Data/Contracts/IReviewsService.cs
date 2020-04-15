namespace Imdb.Services.Data.Contracts
{
    using System;
    using System.Threading.Tasks;

    public interface IReviewsService
    {
        Task AddAsync(string userId, string movieId, string content);

        bool ContainsReviewById(string reviewId);

        Task<string> RemoveById(string reviewId);

        bool HasPermissionToPost(string userId);
    }
}
