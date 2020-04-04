using Imdb.Data.Common.Models;
using System;

namespace Imdb.Data.Models
{
    public class MovieActor : BaseModel<string>
    {
        public MovieActor()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        public string MovieId { get; set; }

        public Movie Movie { get; set; }

        public string ActorId { get; set; }

        public Actor Actor { get; set; }
    }
}