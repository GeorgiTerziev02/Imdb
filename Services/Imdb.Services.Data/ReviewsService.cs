namespace Imdb.Services.Data
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Imdb.Data.Common.Repositories;
    using Imdb.Data.Models;
    using Imdb.Services.Data.Contracts;

    public class ReviewsService : IReviewsService
    {
        private readonly IDeletableEntityRepository<Review> reviewsRepository;

        public ReviewsService(IDeletableEntityRepository<Review> reviewsRepository)
        {
            this.reviewsRepository = reviewsRepository;
        }

        public async Task AddAsync(string userId, string movieId, string content)
        {
            var review = new Review()
            {
                UserId = userId,
                MovieId = movieId,
                Content = content,
            };

            await this.reviewsRepository.AddAsync(review);
            await this.reviewsRepository.SaveChangesAsync();
        }

        public bool ContainsReviewById(string reviewId)
        {
            return this.reviewsRepository.AllAsNoTracking().FirstOrDefault(x => x.Id == reviewId) != null;
        }

        public bool HasPermissionToPost(string userId)
        {
            var lastReview = this.reviewsRepository.AllAsNoTracking().OrderByDescending(x => x.CreatedOn).FirstOrDefault(x => x.UserId == userId);

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

        public async Task<string> RemoveById(string reviewId)
        {
            var review = this.reviewsRepository.All().First(x => x.Id == reviewId);
            this.reviewsRepository.Delete(review);
            await this.reviewsRepository.SaveChangesAsync();

            return review.MovieId;
        }
    }
}
