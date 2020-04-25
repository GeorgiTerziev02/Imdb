namespace Imdb.Services.Data
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Imdb.Data.Common.Repositories;
    using Imdb.Data.Models;
    using Imdb.Services.Data.Contracts;
    using Microsoft.EntityFrameworkCore;

    public class ReviewsService : IReviewsService
    {
        private readonly IDeletableEntityRepository<Review> reviewsRepository;

        public ReviewsService(IDeletableEntityRepository<Review> reviewsRepository)
        {
            this.reviewsRepository = reviewsRepository;
        }

        public async Task<string> AddAsync(string userId, string movieId, string content)
        {
            var review = new Review()
            {
                UserId = userId,
                MovieId = movieId,
                Content = content,
            };

            await this.reviewsRepository.AddAsync(review);
            await this.reviewsRepository.SaveChangesAsync();

            return review.Id;
        }

        public async Task<bool> ContainsReviewById(string reviewId)
        {
            return (await this.reviewsRepository
                .AllAsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == reviewId)) != null;
        }

        public async Task<bool> HasPermissionToPost(string userId)
        {
            var lastReview = await this.reviewsRepository
                .AllAsNoTracking()
                .OrderByDescending(x => x.CreatedOn)
                .FirstOrDefaultAsync(x => x.UserId == userId);

            if (lastReview == null)
            {
                return true;
            }

            TimeSpan timeSpan = DateTime.UtcNow.Subtract(lastReview.CreatedOn);

            if (timeSpan.TotalHours < 1)
            {
                return false;
            }

            return true;
        }

        public async Task<int> UsersReviews(string userId)
        {
            return await this.reviewsRepository
                .AllAsNoTracking()
                .Where(x => x.UserId == userId)
                .CountAsync();
        }

        public async Task<string> RemoveById(string reviewId)
        {
            var review = this.reviewsRepository
                .All()
                .First(x => x.Id == reviewId);

            this.reviewsRepository.Delete(review);
            await this.reviewsRepository.SaveChangesAsync();

            return review.MovieId;
        }
    }
}
