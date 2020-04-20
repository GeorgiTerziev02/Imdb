using Imdb.Data.Models;
using Imdb.Services.Mapping;

namespace Te
{
    public class LanguageTestModel : IMapFrom<Language>
    {
        public string Title { get; set; }
    }
}
