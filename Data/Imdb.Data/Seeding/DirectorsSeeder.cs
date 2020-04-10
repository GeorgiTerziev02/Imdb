namespace Imdb.Data.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Imdb.Common;
    using Imdb.Data.Models;
    using Imdb.Data.Models.Enumerations;

    public class DirectorsSeeder : ISeeder
    {
        private readonly List<string> names = new List<string>
        {
            "Horace Warren",
            "Allan Sutton",
            "Lydia Bowers",
            "Luis Chambers",
            "Clifton Curry",
            "Leroy Wolfe",
            "Joyce Lindsey",
            "Edna Thomas",
            "Marta Graham",
            "Vanessa Mcdonald",
            "Pauline Mccormick",
            "Jamie Burgess",
            "Hilda Stokes",
            "Dewey Adams",
            "Cassandra Burton",
            "Rosemarie Garcia",
            "Toby Oliver",
            "Everett Gordon",
            "Leigh Benson",
            "Floyd Lamb",
            "Ruth Harris",
            "Marlon Howell",
            "Margie Fowler",
            "Eleanor Gregory",
            "Tomas Simmons",
            "Harold Pope",
            "Ross Quinn",
            "Derrick Kelley",
            "Billie Romero",
            "Maggie Chavez",
            "Christie Wilson",
            "Ethel Olson",
            "Amos Potter",
            "Dominic Spencer",
            "Ray Frank",
            "Bryan Nelson",
            "Darrin Stanley",
            "Gerard Thornton",
            "Travis Buchanan",
            "Harvey Mcgee",
            "Terrance Mckenzie",
            "Eileen Bowman",
            "Stephanie Wilkins",
            "Evelyn Burns",
            "Herman Wood",
            "Mable Rivera",
            "Corey Mann",
            "Rene Sparks",
            "Guy Baldwin",
            "Holly Hunt",
            "Mabel Robbins",
            "Shannon Houston",
            "Kathryn Peterson",
            "Dallas Gibbs",
            "Taylor Vargas",
            "Tommy Aguilar",
            "Walter Reyes",
            "Randy Love",
            "Santos Clark",
            "Wilbur Dean",
            "Lila Ramsey",
            "Paulette Allison",
            "Pablo Perez",
            "Richard Rose",
            "Tom Nichols",
            "Bethany Jacobs",
            "Kathy Mason",
            "Antonio Carlson",
            "Audrey Baker",
            "Wanda Kelly",
            "Arlene Mendoza",
            "Beulah Hines",
            "Lorraine Barker",
            "Sergio Jordan",
            "Melody Perry",
            "Brandi Ortiz",
            "Maurice Wise",
            "Andy Holland",
            "Rochelle Patterson",
            "Kerry Hopkins",
            "Blake Figueroa",
            "Laurie Fox",
            "Frederick Crawford",
            "Paul Hogan",
            "Jay Hughes",
            "Beth Carter",
            "Janice Lane",
            "Gustavo Robertson",
            "Bridget Scott",
            "Lana Chandler",
            "Phyllis Mckinney",
            "Norma Jackson",
            "Sandra Mullins",
            "Daryl Gonzales",
            "Esther Powers",
            "Harry Harper",
            "Emily Jenkins",
            "Roberto Hammond",
            "Micheal Rodgers",
            "Loretta Pierce",
        };

        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.Directors.Any())
            {
                return;
            }

            foreach (var name in this.names)
            {
                var nameSplit = name.Split(' ', StringSplitOptions.RemoveEmptyEntries).ToArray();
                var random = new Random();
                random.Next(1, 2);

                var director = new Director()
                {
                    FirstName = nameSplit[0],
                    LastName = nameSplit[1],
                    Born = DateTime.Now,
                    Gender = random.Next(0, 2) == 1 ? Gender.Male : Gender.Female,
                    Description = random.Next(0, 2) == 1 ? "Handsomeeee" : "Beautifuul",
                    ImageUrl = GlobalConstants.DefaulProfilePicture,
                };

                await dbContext.AddAsync(director);
            }
        }
    }
}
