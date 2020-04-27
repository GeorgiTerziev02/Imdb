namespace Imdb.Web.ViewModels.Admin.Administration
{
    using Imdb.Services.Mapping;
    using OMDbApiNet.Model;

    public class OmdbDataProviderModel : IMapFrom<Item>
    {
        public string Type { get; set; }

        public string Title { get; set; }

        public string Released { get; set; }

        public string Runtime { get; set; }

        public string Genre { get; set; }

        public string Director { get; set; }

        public string Writer { get; set; }

        public string Plot { get; set; }

        public string Language { get; set; }

        public string Poster { get; set; }

        public string Actors { get; set; }
    }
}
