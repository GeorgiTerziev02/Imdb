namespace Imdb.Web.Controllers
{
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Imdb.Common;
    using Imdb.Services.Data.Contracts;
    using Imdb.Web.ViewModels.Review;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    public class ReviewsController : Controller
    {
        private readonly IReviewsService reviewsService;

        public ReviewsController(IReviewsService reviewsService)
        {
            this.reviewsService = reviewsService;
        }

        // TODO: User can post review once every hour
        [HttpPost]
        public async Task<IActionResult> Add(AddReviewInputViewModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest();
            }

            if (!this.User.Identity.IsAuthenticated)
            {
                return this.BadRequest();
            }

            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            await this.reviewsService.AddAsync(userId, input.MovieId, input.Content);
            this.TempData["InfoMessage"] = GlobalConstants.ThankYorForReview;

            return this.Redirect($"/Movies/ById/{input.MovieId}");
        }

        [HttpPost]
        public async Task<IActionResult> Remove(string reviewId)
        {
            if (string.IsNullOrWhiteSpace(reviewId))
            {
                return this.BadRequest();
            }

            if (!this.reviewsService.ContainsReviewById(reviewId))
            {
                return this.BadRequest();
            }

            // TODO: Check if exists
            var movieId = await this.reviewsService.RemoveById(reviewId);
            this.TempData["InfoMessage"] = GlobalConstants.SuccessfullDelete;

            return this.Redirect($"/Movies/ById/{movieId}");
        }
    }
}
