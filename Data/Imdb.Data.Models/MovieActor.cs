namespace Imdb.Data.Models
{
    using System;

    using Imdb.Data.Common.Models;

    public class MovieActor : BaseModel<int>
    {
        public string MovieId { get; set; }

        public Movie Movie { get; set; }

        public string ActorId { get; set; }

        public Actor Actor { get; set; }
    }
}
