namespace Imdb.Web.ViewModels.Admin.Administration
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;

    using Imdb.Data.Models;
    using Imdb.Services.Mapping;
    using Microsoft.AspNetCore.Http;

    public class AddImagesInputViewModel : IMapFrom<Movie>
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public bool IsTvShow { get; set; }

        public IEnumerable<IFormFile> Images { get; set; }
    }
}
