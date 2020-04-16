﻿namespace Imdb.Web.ViewModels.Admin.Administration
{
    using AutoMapper;
    using Imdb.Data.Models;
    using Imdb.Services.Mapping;

    public class ActorInfoViewModel : IMapFrom<MovieActor>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string ActorId { get; set; }

        public string Name { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<MovieActor, ActorInfoViewModel>()
                .ForMember(x => x.Name, y => y.MapFrom(x => x.Actor.FirstName + " " + x.Actor.LastName));
        }
    }
}
