namespace Imdb.Web.Controllers
{
    using System.Security.Claims;
    using System.Threading.Tasks;

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

            return this.Redirect($"/Movies/ById/{input.MovieId}");
        }
    }
}
