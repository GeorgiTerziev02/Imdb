using AutoMapper;
using Imdb.Data.Models;
using Imdb.Services.Mapping;
using System;
using System.Collections.Generic;
using System.Text;

namespace Imdb.Web.ViewModels.Movies
{
    public class MovieActorsInfoViewModel : IMapFrom<MovieActor>, IHaveCustomMappings
    {
        public string ActorId { get; set; }

        public string ActorName { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<MovieActor, MovieActorsInfoViewModel>()
                .ForMember(x => x.ActorName, y => y.MapFrom(x => x.Actor.FirstName + x.Actor.LastName));
        }
    }
}
