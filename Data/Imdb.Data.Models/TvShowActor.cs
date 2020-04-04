namespace Imdb.Data.Models
{
    using System;

    using Imdb.Data.Common.Models;

    public class TvShowActor : BaseModel<string>
    {
        public TvShowActor()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        public string TvShowId { get; set; }

        public TvShow TvShow { get; set; }

        public string ActorId { get; set; }

        public Actor Actor { get; set; }
    }
}