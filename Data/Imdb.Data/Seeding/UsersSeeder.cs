﻿namespace Imdb.Data.Seeding
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Imdb.Common;
    using Imdb.Data.Models;
    using Imdb.Data.Models.Enumerations;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.DependencyInjection;

    public class UsersSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            await SeedUserAsync(userManager, "GeorgiTerziev", "123456");
        }

        private static async Task SeedUserAsync(UserManager<ApplicationUser> userManager, string userName, string password)
        {
            var user = await userManager.FindByNameAsync(userName);
            if (user == null)
            {
                user = new ApplicationUser()
                {
                    UserName = userName,
                    PhoneNumber = "+359000000000",
                    Email = "georgiterziev2002@abv.bg",
                    FirstName = "Georgi",
                    LastName = "Terziev",
                    EmailConfirmed = true,
                    Gender = Gender.Male,
                    PhoneNumberConfirmed = true,
                };

                var result = await userManager.CreateAsync(user, password);
                await userManager.AddToRoleAsync(user, GlobalConstants.AdministratorRoleName);
                if (!result.Succeeded)
                {
                    throw new Exception(string.Join(Environment.NewLine, result.Errors.Select(e => e.Description)));
                }
            }
        }
    }
}
