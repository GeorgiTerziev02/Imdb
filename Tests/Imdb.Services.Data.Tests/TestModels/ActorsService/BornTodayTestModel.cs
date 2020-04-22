namespace Imdb.Services.Data.Tests.TestModels.ActorsService
{
    using System;

    using Imdb.Data.Models;
    using Imdb.Services.Mapping;

    public class BornTodayTestModel : IMapFrom<Actor>
    {
        public DateTime? Born { get; set; }
    }
}
