namespace Imdb.Services.Data.Tests.TestModels.LanguageService
{
    using Imdb.Data.Models;
    using Imdb.Services.Mapping;

    public class LanguageTestModel : IMapFrom<Language>
    {
        public string Name { get; set; }
    }
}
