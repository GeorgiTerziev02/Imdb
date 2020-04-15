namespace Imdb.Web.ViewModels.Actors
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class AllActorsListViewModel
    {
        public IEnumerable<ActorViewModel> Actors { get; set; }
    }
}
