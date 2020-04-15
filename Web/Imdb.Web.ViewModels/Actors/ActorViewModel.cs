namespace Imdb.Web.ViewModels.Actors
{
    using AutoMapper;
    using Imdb.Data.Models;
    using Imdb.Services.Mapping;
    using System;

    public class ActorViewModel : IMapFrom<Actor>, IHaveCustomMappings
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public DateTime? Born { get; set; }

        public string ImageUrl { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Actor, ActorViewModel>()
                .ForMember(x => x.Name, y => y.MapFrom(x => x.FirstName + " " + x.LastName));
        }
    }
}
