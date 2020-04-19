namespace Imdb.Web.ViewModels.Admin.Administration
{
    using AutoMapper;
    using Imdb.Data.Models;
    using Imdb.Services.Mapping;

    public class GenreInfoViewModel : IMapFrom<MovieGenre>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public int GenreId { get; set; }

        public string GenreName { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<MovieGenre, GenreInfoViewModel>()
                .ForMember(x => x.GenreName, y => y.MapFrom(x => x.Genre.Name));
        }
    }
}
