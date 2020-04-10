namespace Imdb.Web.ViewModels.Admin.Administration
{
    using AutoMapper;
    using Imdb.Data.Models;
    using Imdb.Services.Mapping;

    public class DirectorDropDownViewModel : IMapFrom<Director>, IHaveCustomMappings
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Director, DirectorDropDownViewModel>()
                .ForMember(x => x.Name, y => y.MapFrom(x => x.FirstName + " " + x.LastName));
        }
    }
}