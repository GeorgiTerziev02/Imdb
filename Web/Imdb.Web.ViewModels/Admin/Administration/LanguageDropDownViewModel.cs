namespace Imdb.Web.ViewModels.Admin.Administration
{
    using Imdb.Data.Models;
    using Imdb.Services.Mapping;

    public class LanguageDropDownViewModel : IMapFrom<Language>
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}