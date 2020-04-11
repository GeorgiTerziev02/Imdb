namespace Imdb.Web.Controllers
{
    using System.Threading.Tasks;

    using Imdb.Data.Models;
    using Imdb.Services.Data.Contracts;
    using Imdb.Web.ViewModels.Votes;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class VotesController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IVotesService votesService;

        public VotesController(UserManager<ApplicationUser> userManager, IVotesService votesService)
        {
            this.userManager = userManager;
            this.votesService = votesService;
        }

        [HttpPost]
        public async Task<ActionResult<MovieVoteResponseModel>> Post(MovieVoteInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest();
            }

            var userId = this.userManager.GetUserId(this.User);
            await this.votesService.VoteAsync(userId, input.MovieId, input.Rating);
            var response = new MovieVoteResponseModel()
            {
                Rating = this.votesService.MovieRating(input.MovieId),
                VotesCount = this.votesService.MovieVotesCount(input.MovieId),
            };

            return response;
        }
    }
}
