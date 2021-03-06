﻿namespace Imdb.Data.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Imdb.Data.Models;

    public class LanguagesSeeder : ISeeder
    {
        private readonly List<string> languages = new List<string>
        {
            "English",
            "Bulgarian",
            "Chinese",
            "Deutsch",
            "Arabic",
            "Spanish",
            "Russian",
        };

        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.Languages.Any())
            {
                return;
            }

            foreach (var languageName in this.languages)
            {
                var language = new Language()
                {
                    Name = languageName,
                };

                await dbContext.Languages.AddAsync(language);
            }
        }
    }
}
