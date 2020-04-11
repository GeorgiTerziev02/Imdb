namespace Imdb.Web.ViewModels.Home
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Text;
    using System.Text.RegularExpressions;
    using Imdb.Data.Models;
    using Imdb.Services.Mapping;

    public class TopMovieViewModel : IMapFrom<Movie>
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string ShortDescription
        {
            get
            {
                var content = WebUtility.HtmlDecode(Regex.Replace(this.Description, @"<[^>]+>", string.Empty));
                return content.Length > 30
                        ? content.Substring(0, 30) + "..."
                        : content;
            }
        }

        public string GeneralImageUrl { get; set; }
    }
}
