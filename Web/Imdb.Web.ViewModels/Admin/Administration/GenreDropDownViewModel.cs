
namespace Imdb.Web.ViewModels.Admin.Administration
{
    using Imdb.Data.Models;
    using Imdb.Services.Mapping;

    public class GenreDropDownViewModel : IMapFrom<Genre>
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
