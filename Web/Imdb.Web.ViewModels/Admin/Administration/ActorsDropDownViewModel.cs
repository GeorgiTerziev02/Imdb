namespace Imdb.Web.ViewModels.Admin.Administration
{
    using AutoMapper;
    using Imdb.Data.Models;
    using Imdb.Services.Mapping;

    public class ActorsDropDownViewModel : IMapFrom<Actor>, IHaveCustomMappings
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Actor, ActorsDropDownViewModel>()
                 .ForMember(x => x.Name, y => y.MapFrom(x => x.FirstName + " " + x.LastName));
        }
    }
}