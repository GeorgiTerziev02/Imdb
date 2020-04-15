namespace Imdb.Web.ViewModels.Actors
{
    using System;
    using System.Collections.Generic;

    using Imdb.Data.Models;
    using Imdb.Data.Models.Enumerations;
    using Imdb.Services.Mapping;

    public class ActorByIdViewModel : IMapFrom<Actor>
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public Gender Gender { get; set; }

        public DateTime? Born { get; set; }

        public string ImageUrl { get; set; }

        public string Description { get; set; }

        public virtual IEnumerable<MovieOfActorViewModel> Movies { get; set; }
    }
}
