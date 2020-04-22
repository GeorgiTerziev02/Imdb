namespace Imdb.Web.ViewModels.Home
{
    using System;

    using Imdb.Data.Models;
    using Imdb.Services.Mapping;

    public class BornTodayActorViewModel : IMapFrom<Actor>
    {
        public string Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime? Born { get; set; }

        public string ImageUrl { get; set; }
    }
}
