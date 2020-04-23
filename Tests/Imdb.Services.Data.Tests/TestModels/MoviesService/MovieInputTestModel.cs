namespace Imdb.Services.Data.Tests.TestModels.MoviesService
{
    using System;

    using Imdb.Data.Models;
    using Imdb.Services.Mapping;

    public class MovieInputTestModel : IMapTo<Movie>
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public string Duration { get; set; }

        public long? Gross { get; set; }

        public decimal? Budget { get; set; }

        public DateTime? ReleaseDate { get; set; }

        public string GeneralImageUrl { get; set; }

        public string Trailer { get; set; }

        public int LanguageId { get; set; }

        public string DirectorId { get; set; }
    }
}
