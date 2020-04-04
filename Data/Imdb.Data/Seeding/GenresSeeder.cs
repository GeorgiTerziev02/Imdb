using Imdb.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imdb.Data.Seeding
{
    public class GenresSeeder : ISeeder
    {
        private readonly List<string> genreNames = new List<string>
        {
            "Action",
            "Adventure",
            "Comedy",
            "Crime",
            "Drama",
            "Fantasy",
            "Historical",
            "Historical fiction",
            "Horror",
            "Mystery",
            "Paranoid fiction",
            "Philosophical",
            "Political",
            "Romance",
            "Saga",
            "Satire",
            "Science fiction",
            "Social",
            "Speculative",
            "Thriller",
            "Urban",
            "Western",
        };

        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.Genres.Any())
            {
                return;
            }

            foreach (var genreName in this.genreNames)
            {
                var genre = new Genre()
                {
                    Name = genreName,
                };

                await dbContext.Genres.AddAsync(genre);
            }
        }
    }
}
