namespace Imdb.Web.ViewModels.TvShows
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using AutoMapper;
    using Imdb.Data.Models;
    using Imdb.Services.Mapping;

    public class TvShowActorInfoViewModel : IMapFrom<MovieActor>, IHaveCustomMappings
    {
        public string ActorId { get; set; }

        public string ActorName { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<MovieActor, TvShowActorInfoViewModel>()
                .ForMember(x => x.ActorName, y => y.MapFrom(x => x.Actor.FirstName + x.Actor.LastName));
        }
    }
}
