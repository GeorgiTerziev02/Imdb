namespace Imdb.Web.ViewComponents
{
    using System.Linq;
    using System.Security.Claims;

    using Imdb.Services.Data.Contracts;
    using Imdb.Web.ViewModels.RecommendedMovies;
    using Microsoft.AspNetCore.Mvc;

    [ViewComponent(Name = "Recommended")]
    public class RecommendedViewComponent : ViewComponent
    {
        private const int Count = 3;
        private readonly IWatchlistsService watchlistsService;
        private readonly IMoviesService moviesService;

        public RecommendedViewComponent(
            IWatchlistsService watchlistsService,
            IMoviesService moviesService)
        {
            this.watchlistsService = watchlistsService;
            this.moviesService = moviesService;
        }

        public IViewComponentResult Invoke()
        {
            var recommendedList = new RecommendedListViewModel();
            if (this.User.Identity.IsAuthenticated == false)
            {
                recommendedList.Entities =
                    this.moviesService.GetTopMovies<RecommendEntityViewModel>(Count);
            }
            else
            {
                var claimsIdentity = (ClaimsIdentity)this.User.Identity;
                var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
                var mostPopularGenreId = this.watchlistsService.MostWatchedGenreId(userId);

                if (mostPopularGenreId == -1)
                {
                    recommendedList.Entities =
                        this.moviesService.GetTopMovies<RecommendEntityViewModel>(Count);
                }
                else
                {
                    recommendedList.Entities = this.watchlistsService.Recommend<RecommendEntityViewModel>(userId, mostPopularGenreId, Count);
                }

                //if (recommendedList.Entities.Count() == 0)
                //{
                //    recommendedList.Entities =
                //        this.moviesService.GetTopMovies<RecommendEntityViewModel>(Count);
                //}
            }

            return this.View(recommendedList);
        }
    }
}
