using Imdb.Common;
using Imdb.Data.Models;
using Imdb.Data.Models.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imdb.Data.Seeding
{
    public class ActorsSeeder : ISeeder
    {
        private readonly List<string> names = new List<string>
        {
            "Megan Boone",
            "Harrold Cooper",
            "Dustin Balala",
            "Loky Lok",
            "Boomer Boomer",
            "Leroy Wolfe",
            "Joyce Lindsey",
            "Edna Thomas",
            "Marta Gordon",
            "Vanessa Mcdonald",
            "Pauline Mccormick",
            "Jamie Burgess",
            "Hilda Stokes",
            "Leonardi DiCaprio",
            "Cassandra Burton",
            "Rosemarie Boomer",
            "Toby Boomer",
            "Everett Gordon",
            "Leigh Benson",
            "Floyd Boomer",
            "Ruth Boomer",
            "Marlon Howell",
            "Margie Fowler",
            "Eleanor Gregory",
            "Tomas Simmons",
            "Harold Hope",
            "Ross XDDDD",
            "Derrick Monroe",
            "Billie Romero",
            "Maggie Alavez",
            "Christie Milson",
            "Ethel Dolson",
            "Amos Reotter",
            "Dominic Mornton",
            "Ray Franklin",
            "Bryan Delson",
            "Darrin Gordon",
            "Gerard Thornton",
            "Travis Uchanan",
            "Harvey Cgee",
            "Terrance Ordon",
            "Tommy Gordon",
            "Stephanie Otter",
            "Tommy Otter",
            "Tommy Wood",
            "Mable Mivera",
            "Corey Nann",
            "Rene Larks",
            "Tommy Xaldwin",
            "Holly Hunt",
            "Mabel Robbins",
            "Shannon Houston",
            "Kathryn Peterson",
            "Dallas Gibbs",
            "Taylor Pargas",
            "Tommy Reyes",
            "Walter Reyes",
            "Tommy Aguilar",
            "Santos Aguilar",
            "Wilbur Dean",
            "Lila Ramsey",
            "Paulette Allison",
            "Pablo Perez",
            "Richard Rose",
            "Andrian Nichols",
            "Bethany Jacobs",
            "Kathy Mason",
            "Andrian Carlson",
            "Andrian Baker",
            "Tommy Kelly",
            "Tommy Mendoza",
            "Beulah Hines",
            "Tommy Barker",
            "Sergio Jordan",
            "Melody Perry",
            "George Ortiz",
            "George Wise",
            "Andy Holland",
            "Rochelle Patterson",
            "Kerry Hopkins",
            "Andrian Figueroa",
            "George Fox",
            "Andrian Crawford",
            "George Hogan",
            "Jay Hughes",
            "Beth Carter",
            "Andrian Lane",
            "Gustavo Robertson",
            "Bridget Scott",
            "Lana Chandler",
            "Phyllis Mckinney",
            "Norma Jackson",
            "Sandra Holland",
            "Daryl Gonzales",
            "Esther Holland",
            "Harry Holland",
            "Emily Jenkins",
            "Sandra Gonzales",
            "Sandra Rodgers",
            "Loretta Gonzales",
        };

        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {

            foreach (var name in this.names)
            {
                string[] nameSplit = name.Split(' ', StringSplitOptions.RemoveEmptyEntries).ToArray();
                var random = new Random();
                random.Next(1, 2);

                var actor = new Actor()
                {
                    FirstName = nameSplit[0],
                    LastName = nameSplit[1],
                    Born = DateTime.Now,
                    Gender = random.Next(0, 2) == 1 ? Gender.Male : Gender.Female,
                    Description = random.Next(0, 2) == 1 ? "Handsomeeee" : "Beautifuul",
                    ImageUrl = GlobalConstants.DefaulProfilePicture,
                };

                await dbContext.Actors.AddAsync(actor);
            }
        }
    }
}
